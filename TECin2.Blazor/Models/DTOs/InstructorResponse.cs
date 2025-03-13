namespace TECin2.Blazor.Models.DTOs
{
    public class InstructorResponse
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public required string Username { get; set; }
        public required List<InstructorGroupResponse> Groups { get; set; }
        public required InstructorRoleResponse Role { get; set; }
    }

    public class InstructorRoleResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Rank { get; set; }
    }

    public class InstructorGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int DepartmentId { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
        public required string IsLateMessage { get; set; }
    }


}

