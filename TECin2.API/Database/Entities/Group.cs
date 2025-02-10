using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string Name { get; set; }

        [Column(TypeName = "binary")]
        public bool Deactivated { get; set; }

        [ForeignKey("Department")]
        [Column(TypeName = "int")]
        public required int DepartmentId { get; set; }
        public Department? Department { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly ArrivalTime { get; set; }

        [ForeignKey("DepartureTimes")]
        [Column(TypeName = "int")]
        public int? DepartureTimesId { get; set; }
        public DepartureTimes? DepartureTimes { get; set; }

        //[Column(TypeName = "nvarchar(100)")]
        //public required string DepartureTime { get; set; }

        [Column(TypeName = "binary")]
        public bool FlexibleArrival { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public TimeOnly? FlexibleTime { get; set; }

        [Column(TypeName = "time")]
        public TimeOnly? IsLateBuffer { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? IsLateMessage { get; set; }

        public virtual ICollection<User>? Users { get; set; }
    }
}
