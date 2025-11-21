namespace TECin2.ClassLibrary.DTOs
{
    public class ComputerLocationResponse
    {
        public int Id { get; set; }
        public required string Location { get; set; }
        public List<LocationApprovedComputerResponse>? ApprovedComputers { get; set; } = [];
    }

    public class LocationApprovedComputerResponse
    {
        public int Id { get; set; }
        public required string MACaddress { get; set; }
    }    
}
