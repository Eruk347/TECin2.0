using System.ComponentModel.DataAnnotations;
using TECin2.API.Database.Entities;

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
        public required TimeOnly ArrivalTime { get; set; }

        public bool IsLateMessageEnabled { get; set; }

        public TimeOnly? IsLatebuffer { get; set; }

        public string? IsLateMessage { get; set; }

        public bool CheckoutRequired { get; set; }

        public WorkHoursInDay? WorkHoursInDay { get; set; }

        public bool FlexibleArrivalEnabled { get; set; }

        public TimeSpan? FlexibleAmount { get; set; }
    }
}
