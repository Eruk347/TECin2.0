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
        public string? LastCheckin { get; set; }
        public bool Deactivated { get; set; }
        public required StudentGroupResponse Group { get; set; }
        public List<UserNotationResponse>? UserNotations { get; set; }
    }
    public class StudentGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ArrivalTime { get; set; }
        public string? IsLateBuffer { get; set; }
        public string? DepartureTime { get; set; }
        public bool FlexibleArrival { get; set; }
        public string? FlexibleTime { get; set; }
    }
}
