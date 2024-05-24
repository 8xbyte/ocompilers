using Microsoft.AspNetCore.Mvc;
using PythonInterpreterMicroservice.Services;
using PythonInterpreterMicroservice.Interfaces;
using Shared.WebSocket.Extensions;
using System.Net.WebSockets;
using Shared.Services;

namespace PythonInterpreterMicroservice.Controllers {
    [Route("api/interpreter/python")]
    [ApiController]
    public class InterpreterController(IRuntimeService runtimeService, ILogger<InterpreterController> logger) : ControllerBase {
        private readonly IRuntimeService _runtimeService = runtimeService;
        private readonly ILogger<InterpreterController> _logger = logger;

        [HttpGet()]
        public async Task Run(bool isDebug = false) {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var guid = Guid.NewGuid();

                _logger.Log(LogLevel.Information, $"Start interpreter session with guid: {guid}");
                if (isDebug) {
                    _logger.Log(LogLevel.Information, $"Enable debug mode");
                }

                try {
                    var files = await webSocket.ReceiveAsync<IEnumerable<IFile>>();
                    _logger.Log(LogLevel.Information, "Files received");

                    Directory.CreateDirectory(guid.ToString());

                    IFile executableFile = null;
                    foreach (var file in files) {
                        if (file.IsExecutable) {
                            executableFile = file;
                        }
                        System.IO.File.WriteAllText($"{guid}/{file.Name}", file.Content);
                        _logger.Log(LogLevel.Information, $"Written content in {guid}/{file.Name}");
                    }

                    if (executableFile != null) {
                        var processExitCode = await _runtimeService.RunAsync($"{guid}", executableFile.Name, isDebug, new ProcessConfig {
                            StartAction = async (isStart) => {
                                _logger.Log(LogLevel.Information, isStart ? "Process start" : "Failed run process");

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = isStart ? "runtime-start" : "runtime-failed",
                                    Message = isStart ? "Process start" : "Failed run process"
                                });
                            },
                            OutputAction = async (outputString, inputStream) => {
                                _logger.Log(LogLevel.Information, outputString);

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = "runtime-stdout",
                                    Message = outputString
                                });
                            },
                            ErrorAction = async (errorString, inputStream) => {
                                _logger.Log(LogLevel.Information, errorString);

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = "runtime-stderr",
                                    Message = errorString
                                });
                            },
                            InputAction = async inputStream => {
                                var inputMessage = await webSocket.ReceiveAsync<IRuntimeMessage>();
                                if (inputMessage?.Type == "runtime-stdin") {
                                    await inputStream.WriteAsync(inputMessage.Message);
                                } else {
                                    await webSocket.SendAsync(new IRuntimeMessage {
                                        Type = "runtime-stdin",
                                        Message = "Now allow only runtime-stdin messages"
                                    });
                                }
                            },
                            EndAction = async (exitCode) => {
                                _logger.Log(LogLevel.Information, $"Process exit with code {exitCode}");

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = "runtime-end",
                                    Message = $"Process exit with code {exitCode}"
                                });
                            }
                        });
                    } else {
                        _logger.Log(LogLevel.Information, "Not found executable file");

                        await webSocket.SendAsync(new IRuntimeMessage {
                            Type = "runtime-failed",
                            Message = "Not found executable file"
                        });
                    }
                } catch (Exception error) {
                    _logger.Log(LogLevel.Information, error.Message);
                } finally {
                    if (webSocket.State == WebSocketState.Open) {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    }

                    if (Directory.Exists(guid.ToString())) {
                        Directory.Delete(guid.ToString(), true);
                    }

                    _logger.Log(LogLevel.Information, $"End interpreter session with guid: {guid}");
                }

            } else {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
