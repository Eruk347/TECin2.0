namespace TECin2.Blazor.Models
{
    public class Instructor
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public required string UserName { get; set; }
        public string? Password { get; set; }
        public int PrimaryGroupId { get; set; }
        public required List<Group> Groups { get; set; }
        public bool IsStudent { get; set; }
        public required Role Role { get; set; }
    }
}
