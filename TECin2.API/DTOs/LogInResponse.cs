using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class LogInResponse//vi sender group så arrivaltime er ikke nødvendig. ved at sende Token og userId, kan vi hente resten af brugeren
    {
        public required string Token { get; set; }
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public required string Username { get; set; }
        public int RoleRank { get; set; }
        public List<LogInSettingResponse> Settings { get; set; } = [];
        public List<LogInGroupResponse> Groups { get; set; } = [];
    }

    public class LogInSettingResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Deactivated { get; set; }
    }

    public class LogInGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
        public bool FlexibleArrivalEnabled { get; set; }
        public TimeOnly? FlexibleAmount { get; set; }
        public TimeOnly? IsLateBuffer { get; set; }
        public string? IsLateMessage { get; set; }
        public WorkHoursInDay? WorkHoursInDay { get; set; }
    }
}
