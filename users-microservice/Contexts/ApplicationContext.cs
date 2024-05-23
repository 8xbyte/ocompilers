using Microsoft.EntityFrameworkCore;
using UsersMicroservice.Models;

namespace UsersMicroservice.Contexts {
    // Контекст приложения
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        // Модель пользователей
        public DbSet<UserModel> Users { get; set; }
    }
}
