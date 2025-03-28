using System.ComponentModel.DataAnnotations;

namespace TECin2.Blazor.Models.DTOs
{
    public class InstructorRequest
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        public int? Phonenumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public List<Group>? Groups { get; set; }

        [Required]
        public bool IsStudent { get; set; }

        [Required]
        public int RoleId { get; set; }

        public List<Setting>? Settings { get; set; }

        public bool Deactivated { get; set; }

    }
}
