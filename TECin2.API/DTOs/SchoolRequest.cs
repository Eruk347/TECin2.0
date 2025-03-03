using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class SchoolRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public bool Deactivated { get; set; }

        public string? Principal { get; set; }
    }
}
