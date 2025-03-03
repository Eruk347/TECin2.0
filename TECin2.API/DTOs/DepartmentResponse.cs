using TECin2.API.Services;

namespace TECin2.API.DTOs
{
    public class DepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required string DepartmentHead { get; set; }
        public required DepartmentSchoolResponse School { get; set; }
        public List<DepartmentGroupResponse> Groups { get; set; } = [];
    }
    public class DepartmentGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
    }

    public class DepartmentSchoolResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public string? Principal { get; set; }
    }
}
