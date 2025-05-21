using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class ApprovedComputerResponse
    {
        public int Id { get; set; }
        public required string MACaddress { get; set; }
        public required ApprovedComputerLocation Location { get; set; }
    }

    public class ApprovedComputerLocation
    {
        public int Id { get; set; }
        public required string Location { get; set; }
    }
}
