namespace TECin2.ClassLibrary.DTOs
{
    public class ApprovedComputerResponse
    {
        public int Id { get; set; }
        public required string MACaddress { get; set; }
        public required ApprovedComputerLocation Location { get; set; }
        public required ApprovedComputerDepartmentResponse Department { get; set; }
    }

    public class ApprovedComputerLocation
    {
        public int Id { get; set; }
        public required string Location { get; set; }
    }

    public class ApprovedComputerDepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? DepartmentHead { get; set; }
        public int SchoolId { get; set; }
    }
}
