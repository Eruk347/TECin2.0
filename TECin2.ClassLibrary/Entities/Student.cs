namespace TECin2.ClassLibrary.Entities
{
    public class Student
    {
        public required string Id { get; set; }
        public string? Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public bool Deactivated { get; set; }
        public Group? Group { get; set; }//måske
        public DateOnly? LastCheckIn { get; set; }
        public List<CheckInStatus> CheckInStatuses { get; set; } = [];
        public string? CPR { get; set; }
    }
}
