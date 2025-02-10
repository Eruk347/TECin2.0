namespace TECin2.API.DTOs
{
    public class LogInRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
