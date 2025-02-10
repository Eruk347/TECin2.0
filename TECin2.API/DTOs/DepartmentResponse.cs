namespace TECin2.API.DTOs
{
    public class DepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required string DepartmentHead { get; set; }
        public List<DepartmentGroupResponse> Groups { get; set; } = [];
    }
    public class DepartmentGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ArrivalTime { get; set; }
        public required string DepartureTime { get; set; }
        //public int NumberOfStudents { get; set; }
    }
}
