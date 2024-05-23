using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkspaceMicroservice.Interfaces;
using WorkspaceMicroservice.Service;
using Shared.WebSocket.Extensions;
using Shared.Interfaces;
using System.Net.WebSockets;
using Shared.Interfaces.Options;
using Microsoft.Extensions.Options;

namespace WorkspaceMicroservice.Controllers {
    [Route("api/workspace")]
    [ApiController]
    public class WorkspaceController(IWorkspaceService workspaceService, IDirectoryService directoryService, IFileService fileService, ILogger<WorkspaceController> logger, IOptions<IWorkspaceOptions> workspaceOptions, IOptions<IApiGatewayOptions> apiGatewayOptions) : ControllerBase {
        private readonly IWorkspaceService _workspaceService = workspaceService;
        private readonly IFileService _fileService = fileService;
        private readonly IDirectoryService _directoryService = directoryService;
        private readonly ILogger<WorkspaceController> _logger = logger;
        private readonly IWorkspaceOptions _workspaceOptions = workspaceOptions.Value;
        private readonly IApiGatewayOptions _apiGatewayOptions = apiGatewayOptions.Value;

        [HttpPost("create")]
        public async Task<ActionResult<IWorkspace>> CreateWorkspace([FromBody] ICreateWorkspaceHttpRequest body) {
            var workspace = await _workspaceService.CreateWorkspaceAsync(Convert.ToInt32(Request.Headers[_apiGatewayOptions.Http.Headers.UserId]), body.Name);
            if (workspace == null) {
                return BadRequest(new IError {
                    Message = $"You cannot create more than {_workspaceOptions.MaxWorkspaces} workspaces"
                });
            }

            if (!_directoryService.CreateDirectory(workspace.Guid)) {
                return BadRequest(new IError {
                    Message = "The workspace folder has already exists"
                });
            }

            return Ok(new IWorkspace {
                Id = workspace.Id,
                Name = workspace.Name,
                Size = _directoryService.GetDirectorySize(workspace.Guid)
            });
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<IWorkspace>>> GetWorkspaces() {
            var workspaces = await _workspaceService.GetUserWorkspacesAsync(Convert.ToInt32(Request.Headers[_apiGatewayOptions.Http.Headers.UserId]));

            return Ok(workspaces.Select(model => new IWorkspace {
                Id = model.Id,
                Name = model.Name,
                Size = _directoryService.GetDirectorySize(model.Guid)
            }));
        }

        [HttpGet("open")]
        public async Task OpenWorkspace(int id) {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                
                try {
                    var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);
                    if (workspace == null) {
                        await webSocket.SendAsync<IWorkspaceError>(new() {
                            Message = "Workspace not found"
                        });

                        throw new Exception("Workspace not found");
                    }

                    if (workspace.UserId != Convert.ToInt32(Request.Headers[_apiGatewayOptions.Http.Headers.UserId])) {
                        await webSocket.SendAsync<IWorkspaceError>(new() {
                            Message = "This is not your workspace"
                        });

                        throw new Exception("This is not your workspace");
                    }

                    while (true) {
                        var receiveMessage = await webSocket.ReceiveAsync<IWorkspaceRequest>();

                        try {
                            switch (receiveMessage.Type) {
                                case "get-struct": {
                                    await webSocket.SendAsync(_workspaceService.GetWorkspaceStuct(workspace.Guid));
                                    break;
                                } case "get-file-content": {
                                    await webSocket.SendAsync<IWorkspaceItem>(new() {
                                        Type = "file",
                                        Name = receiveMessage.Message,
                                        Content = _fileService.ReadFile(receiveMessage.Message)
                                    });
                                    break;
                                } default: {
                                    await webSocket.SendAsync<IWorkspaceError>(new() {
                                        Message = "Unknown message type"
                                    });
                                    break;
                                }
                            }
                        } catch (Exception error) {
                            await webSocket.SendAsync<IWorkspaceError>(new() {
                                Message = "Bad message struct"
                            });
                        }
                    }
                } catch (Exception error) {
                    _logger.Log(LogLevel.Information, error.Message);
                } finally {
                    if (webSocket.State == WebSocketState.Open) {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    }
                }
            } else {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpPost("rename")]
        public async Task<ActionResult<IWorkspace>> RenameWorkspace([FromBody] IRenameWorkspaceHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.Id);
            if (workspace == null) {
                return BadRequest(new IError {
                    Message = "Workspace not found"
                });
            }

            if (workspace.UserId != Convert.ToInt32(Request.Headers[_apiGatewayOptions.Http.Headers.UserId])) {
                return BadRequest(new IError {
                    Message = "This is not your workspace"
                });
            }

            workspace.Name = body.NewName;
            var newWorkspace = await _workspaceService.UpdateWorkspaceAsync(workspace);
            return Ok(new IWorkspace {
                Id = newWorkspace.Id,
                Name = newWorkspace.Name,
                Size = _directoryService.GetDirectorySize(workspace.Guid)
            });
        }

        [HttpPost("delete")]
        public async Task<ActionResult<IWorkspace>> DeleteWorkspace([FromBody] IDeleteWorkspaceHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.Id);
            if (workspace == null) {
                return BadRequest(new IError {
                    Message = "Workspace not found"
                });
            }

            if (workspace.UserId != Convert.ToInt32(Request.Headers[_apiGatewayOptions.Http.Headers.UserId])) {
                return BadRequest(new IError {
                    Message = "This workspace is not owned by this user"
                });
            }

            if (!_directoryService.DeleteDirectory(workspace.Guid)) {
                return BadRequest(new IError {
                    Message = "Workspace folder does not exist"
                });
            }

            var deletedWorkspace = await _workspaceService.DeleteWorkspaceAsync(workspace);
            return Ok(new IWorkspace {
                Id = deletedWorkspace.Id,
                Name = deletedWorkspace.Name,
                Size = 0
            });
        }
    }
}
