using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Interfaces.Options;
using WorkspaceMicroservice.Contexts;
using WorkspaceMicroservice.Interfaces;
using WorkspaceMicroservice.Models;

namespace WorkspaceMicroservice.Service {
    public interface IWorkspaceService {
        public Task<WorkspaceModel?> CreateWorkspaceAsync(int userId, string name);
        public Task<WorkspaceModel?> GetWorkspaceByIdAsync(int id);
        public Task<WorkspaceModel> UpdateWorkspaceAsync(WorkspaceModel newModel);
        public Task<IEnumerable<WorkspaceModel>> GetUserWorkspacesAsync(int userId);
        public Task<WorkspaceModel> DeleteWorkspaceAsync(WorkspaceModel model);

        public IEnumerable<IWorkspaceItem> GetWorkspaceStuct(string path);
    }

    public class WorkspaceService(ApplicationContext context, IOptions<IWorkspaceOptions> workspaceOptions) : IWorkspaceService {
        private readonly ApplicationContext _context = context;
        private readonly IWorkspaceOptions _workspaceOptions = workspaceOptions.Value;

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

        public IEnumerable<IWorkspaceItem> GetWorkspaceStuct(string path) {
            var workspaceItems = new List<IWorkspaceItem>();

            foreach (var directory in Directory.GetDirectories(path)) {
                workspaceItems.Add(new() {
                    Type = "directory",
                    Name = directory,
                    Content = null,
                    Items = GetWorkspaceStuct(directory)
                });
            }

            foreach (var file in Directory.GetFiles(path)) {
                workspaceItems.Add(new() {
                    Type = "file",
                    Name = file,
                    Content = null,
                    Items = null
                });
            }

            return workspaceItems;
        }
    }
}
