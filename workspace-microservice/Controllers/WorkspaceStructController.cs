using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Options;
using Shared.Interfaces;
using WorkspaceMicroservice.Interfaces;
using Microsoft.Extensions.Logging;
using WorkspaceMicroservice.Service;
using Microsoft.Extensions.Options;

namespace WorkspaceMicroservice.Controllers {
    [Route("api/workspace/struct")]
    [ApiController]
    public class WorkspaceStructController(IWorkspaceService workspaceService, IDirectoryService directoryService, IFileService fileService, ILogger<WorkspaceController> logger, IOptions<IWorkspaceOptions> workspaceOptions, IOptions<IApiGatewayOptions> apiGatewayOptions) : ControllerBase {
        private readonly IWorkspaceService _workspaceService = workspaceService;
        private readonly IFileService _fileService = fileService;
        private readonly IDirectoryService _directoryService = directoryService;
        private readonly ILogger<WorkspaceController> _logger = logger;
        private readonly IWorkspaceOptions _workspaceOptions = workspaceOptions.Value;
        private readonly IApiGatewayOptions _apiGatewayOptions = apiGatewayOptions.Value;

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<IWorkspaceStructItem>>> GetWorkspaceStruct(int id) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);
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

            return Ok(_workspaceService.GetWorkspaceStruct(workspace.Guid));
        }

        [HttpPost("file/create")]
        public async Task<ActionResult<ICreateFileHttpResponse>> CreateFile([FromBody] ICreateFileHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isCreated = _fileService.CreateFile($"{workspace.Guid}/{body.Path}");

            if (!isCreated) {
                return BadRequest(new IError {
                    Message = "File is already exists"
                });
            }

            return Ok(new ICreateFileHttpResponse {
                Path = body.Path
            });
        }

        [HttpPost("file/read")]
        public async Task<ActionResult<IReadFileHttpResponse>> ReadFile([FromBody] IReadFileHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var content = _fileService.ReadFile(body.Path);

            if (content == null) {
                return BadRequest(new IError {
                    Message = "File not found"
                });
            }

            return Ok(new IReadFileHttpResponse {
                Path = body.Path,
                Content = content
            });
        }

        [HttpPost("file/write")]
        public async Task<ActionResult<IWriteFileHttpResponse>> WriteFile([FromBody] IWriteFileHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _fileService.WriteFile($"{workspace.Guid}/{body.Path}", body.Content);

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "File not found"
                });
            }

            return Ok(new IWriteFileHttpResponse {
                Path = body.Path,
            });
        }

        [HttpPost("file/rename")]
        public async Task<ActionResult<IRenameFileHttpResponse>> RenameFile([FromBody] IRenameFileHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _fileService.RenameFile($"{workspace.Guid}/{body.OldPath}", $"{workspace.Guid}/{body.NewPath}");

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "File not found or path exists"
                });
            }

            return Ok(new IRenameFileHttpResponse {
                Path = body.NewPath
            });
        }

        [HttpPost("file/delete")]
        public async Task<ActionResult<IDeleteFileHttpResponse>> DeleteFile([FromBody] IDeleteFileHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _fileService.DeleteFile($"{workspace.Guid}/{body.Path}");

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "File not found or path exists"
                });
            }

            return Ok(new IDeleteFileHttpResponse {
                Path = body.Path
            });
        }

        [HttpPost("directory/create")]
        public async Task<ActionResult<ICreateDirectoryHttpResponse>> CreateDirectory([FromBody] ICreateDirectoryHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _directoryService.CreateDirectory($"{workspace.Guid}/{body.Path}");

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "File is already exists"
                });
            }

            return Ok(new ICreateDirectoryHttpResponse {
                Path = body.Path
            });
        }

        [HttpPost("directory/rename")]
        public async Task<ActionResult<IRenameDirectoryHttpResponse>> RenameDirectory([FromBody] IRenameDirectoryHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _directoryService.RenameDirectory($"{workspace.Guid}/{body.OldPath}", $"{workspace.Guid}/{body.NewPath}");

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "Directory not found or path exists"
                });
            }

            return Ok(new IRenameDirectoryHttpResponse {
                Path = body.NewPath
            });
        }

        [HttpPost("directory/rename")]
        public async Task<ActionResult<IDeleteDirectoryHttpResponse>> RenameDirectory([FromBody] IDeleteDirectoryHttpRequest body) {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(body.WorkspaceId);
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

            var isWrite = _directoryService.DeleteDirectory($"{workspace.Guid}/{body.Path}");

            if (!isWrite) {
                return BadRequest(new IError {
                    Message = "Directory not found"
                });
            }

            return Ok(new IDeleteDirectoryHttpResponse {
                Path = body.Path
            });
        }
    }
}
