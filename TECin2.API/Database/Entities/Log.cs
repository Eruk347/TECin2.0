using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(350)")]
        public required string Message { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateAndTime { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public required string User { get; set; }
    }
}
