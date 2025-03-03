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
        public required TimeOnly Arrival { get; set; }
        public DateOnly? LastCheckin { get; set; }
        public TimeOnly? Departure { get; set; }
    }
}
