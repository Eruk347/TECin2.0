using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.API.Database.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar(36)")]
        public required string Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string Username { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public string? Salt { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string FirstName { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public required string LastName { get; set; }

        [Column(TypeName = "int")]
        public int? Phonenumber { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string? Email { get; set; }

        [Column(TypeName = "binary")]
        public bool Deactivated { get; set; }

        public virtual ICollection<Group>? Groups { get; set; }

        public List<Setting>? Settings { get; set; }

        [Column(TypeName = "binary")]
        public bool IsStudent { get; set; }

        [ForeignKey("Role")]
        [Column(TypeName = "int")]
        public int RoleId { get; set; }

        public required Role Role { get; set; }

        [Column(TypeName = "date")]
        public DateOnly? LastCheckin { get; set; }


    }
}
