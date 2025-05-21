using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TECin2.API.Database.Entities
{
    public class ComputerLocation
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public required string Location { get; set; }

        public List<ApprovedComputer>? ApprovedComputers { get; set; } = [];
    }
}
