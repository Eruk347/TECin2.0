namespace TECin2.API.DTOs
{
    public class CheckInResponseLong
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public int? Phonenumber { get; set; }
        public required string Arrival { get; set; }
        public string? LastCheckin { get; set; }
        public string? Checkout { get; set; }
    }
}
