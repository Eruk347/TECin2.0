namespace TECin2.Blazor.Models
{
    public class Week(int _weekNumber)
    {
        public int WeekNumber = _weekNumber;
        public List<CheckInStatus> CheckIns = [];
        public string TimeWorked = "";
    }
}
