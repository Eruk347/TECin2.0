using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TECin2.ClassLibrary;

namespace TECin2.Blazor.Models
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
