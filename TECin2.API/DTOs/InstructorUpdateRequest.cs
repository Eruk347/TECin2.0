using System.ComponentModel.DataAnnotations;
using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class InstructorUpdateRequest
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Range(10000000, 99999999, ErrorMessage = "Telefonnumme skal være 8 cifre langt")]
        public int? Phonenumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        public string? Password { get; set; }

        //[Compare("Password", ErrorMessage = "Passwords skal være ens")]
        public string? ConfirmPassword { get; set; }

        public List<Group>? Groups { get; set; }

        [Required]
        public int RoleId { get; set; }

        public List<Setting>? Settings { get; set; }

        public bool Deactivated { get; set; }
    }
}
