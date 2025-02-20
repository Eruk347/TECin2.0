using System.ComponentModel.DataAnnotations;

namespace TECin2.API.DTOs
{
    public class InstruktorRequest
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

        public int GroupId { get; set; }

        [Required]
        public bool IsStudent { get; set; }
        
        [Required]
        public int RoleId { get; set; }

        public List<string>? Settings { get; set; }

    }
}
