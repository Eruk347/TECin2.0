namespace TECin2.API.DTOs
{
    public class ApprovedComputerRequest
    {
        public required string MACaddress { get; set; }
        public int ComputerLocationId { get; set; }
    }
}
