using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TECin2.API.Database.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string Name { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? Description { get; set; }

        public ICollection<User>? Users { get; set; }

        [Column(TypeName = "binary")]
        public bool Deactivated { get; set; }
    }
}
