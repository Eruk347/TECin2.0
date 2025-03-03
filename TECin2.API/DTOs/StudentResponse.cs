using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class StudentResponse
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public required string Username { get; set; }
        public DateOnly? LastCheckin { get; set; }
        public bool Deactivated { get; set; }
        public required StudentGroupResponse Group { get; set; }
        public List<CheckInResponseLong>? CheckInResponses { get; set; }
    }
    public class StudentGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
        public TimeOnly? IsLateBuffer { get; set; }
        public WorkHoursInDay? WorkHoursInDay { get; set; }
        public bool FlexibleArrivalEnabled { get; set; }
        public TimeOnly? FlexibleAmount { get; set; }
    }
}
