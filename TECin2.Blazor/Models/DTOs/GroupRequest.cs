using System.ComponentModel.DataAnnotations;

namespace TECin2.Blazor.Models.DTOs
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
        public required TimeOnly ArrivalTime { get; set; }

        public TimeOnly? IsLatebuffer { get; set; }

        public string? IsLateMessage { get; set; }

        public WorkHoursInDay? WorkHoursInDay { get; set; }

        public bool FlexibleArrivalEnabled { get; set; }

        public TimeOnly? FlexibleAmount { get; set; }
    }
}
