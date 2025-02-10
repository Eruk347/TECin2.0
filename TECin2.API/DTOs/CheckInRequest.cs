using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class CheckInRequest
    {
        [Required]
        public required string CPR_number {get;set;}

        [Required]
        public DateTime CheckinTime {get;set;}
    }
}
