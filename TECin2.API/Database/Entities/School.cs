using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class School
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string Name { get; set; }

        public ICollection<Department>? Departments { get; set; }

        [Column(TypeName = "bit")]
        public bool Deactivated { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public string? Principal { get; set; }
    }
}
