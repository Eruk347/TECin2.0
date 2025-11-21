using TECin2.ClassLibrary.Entities;

namespace TECin2.API.DTOs
{
    public class GroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
        public bool FlexibleArrivalEnabled { get; set; }
        public TimeSpan? FlexibleAmount { get; set; }
        public bool IsLateMessageEnabled { get; set; }
        public TimeOnly? IsLateBuffer { get; set; }
        public string? IsLateMessage { get; set; }
        public bool CheckoutRequired { get; set; }
        public WorkHoursInDay? WorkHoursInDay { get; set; }
        public required GroupDepartmentResponse Department { get; set; }
        public List<GroupUsersResponse?> Students { get; set; } = [];
        public List<GroupUsersResponse?> Instructors { get; set; } = [];
    }

    public class GroupDepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
    }

    public class GroupUsersResponse
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public DateOnly? LastCheckin { get; set; }
        public bool Deactivated { get; set; }
    }
}
