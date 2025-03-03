namespace TECin2.API.DTOs
{
    public class SchoolResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required string Principal { get; set; }
        public List<SchoolDepartmentResponse> Departments { get; set; } = [];
    }

    public class SchoolDepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string DepartmentHead { get; set; }
    }
}
