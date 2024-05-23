using Microsoft.EntityFrameworkCore;
using WorkspaceMicroservice.Models;

namespace WorkspaceMicroservice.Contexts {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<WorkspaceModel> Workspaces { get; set; }
    }
}
