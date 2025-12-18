using System.ComponentModel.DataAnnotations;

namespace TECin2.ClassLibrary.DTOs
{
    public class StudentRequest
    {
        public string? Username { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        public int? Phonenumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        public int GroupId { get; set; }

        [Required]
        public bool IsStudent { get; set; }

        public int RoleId { get; set; }

        public bool Deactivated { get; set; }

        public DateOnly? LastCheckin { get; set; }

        public string? CPR { get; set; }//kan ikke være required for put(update) ellers skal cpr nummer tastes ind ved opdateringer

    }
}
