using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkspaceMicroservice.Models {
    // Базовая модель
    public class BaseModel {
        [Key]
        [Required]
        [Column("id")]
        // Id
        public int Id { get; set; }
    }
}
