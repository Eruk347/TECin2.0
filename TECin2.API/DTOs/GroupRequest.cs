using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class GroupRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public bool Deactivated { get; set; }

        [Required]
        public required string ArrivalTime { get; set; }

        [Required]
        public string? IsLatebuffer { get; set; }

        public string? IsLateMessage { get; set; }

        public string? DepartureTime { get; set; }

        public bool FlexibleArrival { get; set; }

        public string? FlexibleTime { get; set; }
    }
}
