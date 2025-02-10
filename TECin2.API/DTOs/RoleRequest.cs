using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class RoleRequest
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required string Description { get; set; }

        [Required]
        public bool Deactivated { get; set; }

        [Required]
        public int Rank { get;set; }
    }
}
