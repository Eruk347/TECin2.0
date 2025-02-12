using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class WorkHoursInDay
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly Monday { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly Tuesday { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly Wednesday { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly Thursday { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly Friday { get; set; }
    }
}
