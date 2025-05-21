namespace TECin2.API.DTOs
{
    public class WorkHoursInDayResponse
    {
        public required int Id { get; set; }
        public required TimeSpan Monday { get; set; }
        public required TimeSpan Tuesday { get; set; }
        public required TimeSpan Wednesday { get; set; }
        public required TimeSpan Thursday { get; set; }
        public required TimeSpan Friday { get; set; }
    }
}
