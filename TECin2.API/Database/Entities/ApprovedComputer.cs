using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TECin2.API.Database.Entities
{
    public class ApprovedComputer
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(17)")]
        public required string MACaddress { get; set; }

        [ForeignKey("ComputerLocation")]
        [Column(TypeName = "int")]
        public int ComputerLocationId { get; set; }

        public ComputerLocation? ComputerLocation { get; set; }
        
        [Column(TypeName = "int")]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }
    }
}
