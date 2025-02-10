namespace TECin2.API.DTOs
{
    public class UserNotationResponse
    {
        public int Id { get; set; }
        public required string User_Id { get; set; }
        public required string ArrivalDate { get; set; }
        public required string ArrivalTime { get; set; }
        public string? Departure { get; set; }
    }
}
