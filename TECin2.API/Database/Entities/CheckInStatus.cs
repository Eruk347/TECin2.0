using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class CheckInStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column(TypeName = "nvarchar(36)")]
        public required string User_Id { get; set; }
        public User? User { get; set; }

        [Column(TypeName = "date")]
        public required DateOnly ArrivalDate { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly ArrivalTime { get; set; }

        [Column(TypeName = "time")]
        public TimeOnly? Departure { get; set; }
    }
}
