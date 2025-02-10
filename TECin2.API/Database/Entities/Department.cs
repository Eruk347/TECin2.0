using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string Name { get; set; }

        public ICollection<Group>? Groups { get; set; }

        [Column(TypeName = "binary")]
        public bool Deactivated { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public string? DepartmentHead { get; set; }

        [ForeignKey("School")]
        [Column(TypeName = "int")]
        public int SchoolId { get; set; }
        public School? School { get; set; }
    }
}
