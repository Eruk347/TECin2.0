using System.ComponentModel.DataAnnotations;

namespace TECin2.ClassLibrary.DTOs
{
    public class DepartmentRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public bool Deactivated { get; set; }

        public string? DepartmentHead { get; set; }

        public int SchoolId { get; set; }
    }
}
