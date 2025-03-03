using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class StudentRequest
    {
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        //[Range(10000000,99999999,ErrorMessage ="Telefonnumme skal være 8 cifre langt")]
        public int? Phonenumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        public string? CPR { get; set; }//kan ikke være required for put(update) ellers skal cpr nummer tastes ind ved opdateringer

        public int GroupId { get; set; }
        [Required]
        public bool IsStudent { get; set; }

        public int RoleId { get; set; }

        public bool Deactivated { get; set; }

        public DateOnly? LastCheckin { get; set; }

    }
}
