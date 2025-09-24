using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class WorkHoursInDay
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column(TypeName = "time")]
        public required TimeSpan Monday { get; set; }

        [Column(TypeName = "time")]
        public required TimeSpan Tuesday { get; set; }

        [Column(TypeName = "time")]
        public required TimeSpan Wednesday { get; set; }

        [Column(TypeName = "time")]
        public required TimeSpan Thursday { get; set; }

        [Column(TypeName = "time")]
        public required TimeSpan Friday { get; set; }
    }
}
