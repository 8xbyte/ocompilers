using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersMicroservice.Models {
    [Table("users")]
    [Index(nameof(Email), IsUnique = true)]
    // Модель пользователя
    public class UserModel : BaseModel {
        [Required]
        [EmailAddress]
        [Column("email")]
        // Email
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        [Column("password")]
        // Хэшированный пароль
        public string Password { get; set; }
    }
}
