namespace TECin2.ClassLibrary.DTOs
{
    public class ApprovedComputerRequest
    {
        public required string MACaddress { get; set; }
        public int ComputerLocationId { get; set; }
        public int DepartmentId { get; set; }
    }
}
