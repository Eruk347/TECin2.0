namespace TECin2.API.DTOs
{
    public class WorkHoursInDayRequest
    {
        public required TimeOnly Monday { get; set; }
        public required TimeOnly Tuesday { get; set; }
        public required TimeOnly Wednesday { get; set; }
        public required TimeOnly Thursday { get; set; }
        public required TimeOnly Friday { get; set; }
    }
}
