using TECin2.Blazor.Models;

namespace TECin2.Blazor.Models
{
    public class Group
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public bool Deactivated { get; set; }

        public required int DepartmentId { get; set; }

        public Department? Department { get; set; }

        public required TimeOnly ArrivalTime { get; set; }

        public bool CheckoutRequired { get; set; }

        public int? WorkHoursInDayId { get; set; }

        public WorkHoursInDay? WorkHoursInDay { get; set; }

        public bool FlexibleArrivalEnabled { get; set; }

        public TimeOnly? FlexibleAmount { get; set; }

        public bool IsLateMessageEnabled { get; set; }

        public TimeOnly? IsLateBuffer { get; set; }

        public required string IsLateMessage { get; set; }

        public List<Student>? Students { get; set; }

        public List<Instructor>? Instructors { get; set; }
    }
}
