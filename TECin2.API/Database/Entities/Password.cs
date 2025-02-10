using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class Password//Lav login som det næste
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar(36)")]
        public required string Id { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public required string Cipher { get; set; }

        [Column(TypeName = "int")]
        public int? Salt { get; set; }
    }
}
