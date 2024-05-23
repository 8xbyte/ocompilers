using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkspaceMicroservice.Models {
    [Table("workspaces")]
    [Index(nameof(Guid), IsUnique = true)]
    public class WorkspaceModel : BaseModel {
        [Required]
        [Column("userId")]
        public int UserId { get; set; }

        [Required]
        [StringLength(36)]
        [Column("guid")]
        public string Guid { get; set; }

        [Required]
        [StringLength(64)]
        [Column("name")]
        public string Name { get; set; }
    }
}
