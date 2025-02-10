using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class DepartmentRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public bool Deactivated { get; set; }

        [Required]
        public string? DepartmentHead { get; set; }
    }
}
