using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class SecurityNumb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar(36)")]
        public required string Id { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public required string Cipher { get; set; }
    }
}
