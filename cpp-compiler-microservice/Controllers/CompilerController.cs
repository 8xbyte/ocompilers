using CppCompilerMicroservice.Interfaces;
using CppCompilerMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Shared.WebSocket.Extensions;
using System.Text.RegularExpressions;
using Shared.Services;

namespace CppCompilerMicroservice.Controllers {
    [Route("api/compilers/cpp")]
    [ApiController]
    public class CompilerController(IBuildService buildService, IRuntimeService runtimeService, ILogger<CompilerController> logger) : ControllerBase {
        private readonly IBuildService _buildService = buildService;
        private readonly IRuntimeService _runtimeService = runtimeService;
        private readonly ILogger<CompilerController> _logger = logger;

        [HttpGet()]
        public async Task BuildAndRun(bool isDebug = false) {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var guid  = Guid.NewGuid();

                _logger.Log(LogLevel.Information, $"Start compiler session with guid: {guid}");
                if (isDebug) {
                    _logger.Log(LogLevel.Information, $"Enable debug mode");
                }

                try {
                    var files = await webSocket.ReceiveAsync<IEnumerable<IFile>>();
                    _logger.Log(LogLevel.Information, "Files received");

                    Directory.CreateDirectory(guid.ToString());

                    foreach (var file in files) {
                        System.IO.File.WriteAllText($"{guid}/{file.Name}", file.Content);
                        _logger.Log(LogLevel.Information, $"Written content in {guid}/{file.Name}");
                    }

                    var isBuildSuccess = true;
                    await webSocket.SendAsync(new IBuildMessage {
                        Stage = "build-start",
                        Message = "Build start with GCC"
                    });

                    await _buildService.BuildAsync(guid.ToString(), isDebug, new ProcessConfig {
                        ErrorAction = async (errorString, inputStream) => {
                            isBuildSuccess = false;
                            _logger.Log(LogLevel.Information, errorString);
                            await webSocket.SendAsync(new IBuildMessage {
                                Stage = "build-error",
                                Message = errorString
                            });
                        }
                    });

                    await webSocket.SendAsync(new IBuildMessage {
                        Stage = isBuildSuccess ? "build-success" : "build-failed",
                        Message = isBuildSuccess ? "Build success" : "Build failed"
                    });

                    if (isBuildSuccess) {
                        _logger.Log(LogLevel.Information, "Build success");

                        var runtimeExitCode = await _runtimeService.RunAsync(guid.ToString(), isDebug, new ProcessConfig {
                            StartAction = async (isStart) => {
                                _logger.Log(LogLevel.Information, isStart ? "Process start" : "Failed run process");

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = isStart ? "runtime-start" : "runtime-failed",
                                    Message = isStart ? "Process start" : "Failed run process"
                                });
                            },
                            OutputAction = async (outputString, inputStream) => {
                                _logger.Log(LogLevel.Information, outputString);

                                if (isDebug && inputStream != null) {
                                    string first_pattern = @"\[Inferior 1 \(process (\d+)\) exited with code (\d+)\]";
                                    string second_pattern = @"\[Inferior 1 \(process (\d+)\) exited normally\]";
                                    if (Regex.Match(outputString, first_pattern).Success || Regex.Match(outputString, second_pattern).Success) {
                                        await inputStream.WriteAsync("quit\n");
                                    }
                                }

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = "runtime-stdout",
                                    Message = outputString
                                });
                            },
                            ErrorAction = async (errorString, inputStream) => {
                                _logger.Log(LogLevel.Information, errorString);

                                if (isDebug && inputStream != null) {
                                    string first_pattern = @"\[Inferior 1 \(process (\d+)\) exited with code (\d+)\]";
                                    string second_pattern = @"\[Inferior 1 \(process (\d+)\) exited normally\]";
                                    if (Regex.Match(errorString, first_pattern).Success || Regex.Match(errorString, second_pattern).Success) {
                                        await inputStream.WriteAsync("quit\n");
                                    }
                                }

                                await webSocket.SendAsync(new IRuntimeMessage {
                                    Type = "runtime-stderr",
                                    Message = errorString
                                });
                            },
                            InputAction = async (inputStream) => {
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

                    _logger.Log(LogLevel.Information, $"End compiler session with guid: {guid}");
                }

            } else {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
