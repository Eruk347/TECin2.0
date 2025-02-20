using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(32)")]
        public string? Name { get; set; }

        [Column(TypeName ="nvarchar(250)")]
        public string? Description { get; set; }

        [Column(TypeName ="int")]
        public int Rank { get; set; }

        [Column(TypeName = "bit")]
        public bool Deactivated { get; set; }
    }
}
