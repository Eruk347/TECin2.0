using System.ComponentModel.DataAnnotations;
using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class InstructorRequest
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public int? Phonenumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        public string? Password { get; set; }

        public int PrimaryGroupId { get; set; }

        public List<int>? Groups { get; set; }
        //public List<Group>? Groups { get; set; }

        [Required]
        public bool IsStudent { get; set; } = false;

        [Required]
        public int RoleId { get; set; }

        public List<Setting>? Settings { get; set; }

        public bool Deactivated { get; set; }

    }
}
