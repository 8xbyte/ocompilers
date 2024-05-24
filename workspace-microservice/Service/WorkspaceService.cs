using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Interfaces.Options;
using System.IO;
using WorkspaceMicroservice.Contexts;
using WorkspaceMicroservice.Interfaces;
using WorkspaceMicroservice.Models;
using ZstdSharp.Unsafe;

namespace WorkspaceMicroservice.Service {
    public interface IWorkspaceService {
        public Task<WorkspaceModel?> CreateWorkspaceAsync(int userId, string name);
        public Task<WorkspaceModel?> GetWorkspaceByIdAsync(int id);
        public Task<WorkspaceModel> UpdateWorkspaceAsync(WorkspaceModel newModel);
        public Task<IEnumerable<WorkspaceModel>> GetUserWorkspacesAsync(int userId);
        public Task<WorkspaceModel> DeleteWorkspaceAsync(WorkspaceModel model);

        public IEnumerable<IWorkspaceStructItem> GetWorkspaceStruct(string path);
    }

    public class WorkspaceService(ApplicationContext context, IFileService fileService, IOptions<IWorkspaceOptions> workspaceOptions) : IWorkspaceService {
        private readonly ApplicationContext _context = context;
        private readonly IWorkspaceOptions _workspaceOptions = workspaceOptions.Value;
        private readonly IFileService _fileService = fileService;

        public async Task<WorkspaceModel?> CreateWorkspaceAsync(int userId, string name) {
            var userWorkspaces = await GetUserWorkspacesAsync(userId);
            if (userWorkspaces.Count() == _workspaceOptions.MaxWorkspaces) {
                return null;
            }

            var guid = Guid.NewGuid();
            var workspace = await _context.Workspaces.AddAsync(new WorkspaceModel {
                Guid = guid.ToString(),
                UserId = userId,
                Name = name
            });
            await _context.SaveChangesAsync();

            return workspace.Entity;
        }

        public async Task<WorkspaceModel?> GetWorkspaceByIdAsync(int id) {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(model => model.Id == id);
            return workspace;
        }

        public async Task<WorkspaceModel> UpdateWorkspaceAsync(WorkspaceModel newModel) {
            var newWorkspace = _context.Workspaces.Update(newModel);
            await _context.SaveChangesAsync();
            return newWorkspace.Entity;
        }

        public async Task<IEnumerable<WorkspaceModel>> GetUserWorkspacesAsync(int userId) {
            return await _context.Workspaces.Where(model => model.UserId == userId).ToListAsync();
        }

        public async Task<WorkspaceModel> DeleteWorkspaceAsync(WorkspaceModel model) {
            var workspace = _context.Workspaces.Remove(model);
            await _context.SaveChangesAsync();
            return workspace.Entity;
        }

        public IEnumerable<IWorkspaceStructItem> GetWorkspaceStruct(string path) {
            var workspaceItems = new List<IWorkspaceStructItem>();

            foreach (var directory in Directory.GetDirectories(path)) {
                workspaceItems.Add(new() {
                    Type = "directory",
                    Name = Path.GetDirectoryName(directory),
                    Items = GetWorkspaceStruct(directory)
                });
            }

            foreach (var file in Directory.GetFiles(path)) {
                workspaceItems.Add(new() {
                    Type = "file",
                    Name = Path.GetFileName(file),
                    Items = null,
                    Content = _fileService.ReadFile(file)
                });
            }

            return workspaceItems;
        }
    }
}
