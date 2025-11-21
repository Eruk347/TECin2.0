using Microsoft.AspNetCore.Components;
using TECin2.ClassLibrary;
using TECin2.ClassLibrary.DTOs;

namespace TECin2.Blazor.Services
{
    public static class TableColor
    {
        private static readonly string yellow = "#CDA01E";
        private static readonly string red = "#fa5050";
        private static string ReturnColor(bool seeAll)
        {
            if (seeAll)
                return yellow;
            return red;
        }

        public static string Color(CheckInResponseLong checkInResponse, LogInResponse _currentUserData, DateOnly date)
        {
            return Color(checkInResponse.Arrival, checkInResponse.Departure, checkInResponse.GroupId, _currentUserData, date);
        }

        public static string Color(CheckInStatus checkInStatus, LogInResponse _currentUserData, DateOnly date, Group group)
        {
            return Color(checkInStatus.ArrivalTime, checkInStatus.Departure, group.Id, _currentUserData, date);
        }

        private static string Color(TimeOnly studentArrival, TimeOnly? studentDeparture, int _groupId, LogInResponse _currentUserData, DateOnly date)
        {
            if (studentArrival == new TimeOnly())
            {
                return red;
            }

            LogInGroupResponse? group = null;

            if (_currentUserData == null)
                return red;

            bool seeAll = true;
            foreach (var item in _currentUserData.Settings)
            {
                if (item.Name == "SeeAll")
                    seeAll = true;
            }

            foreach (var item in _currentUserData.Groups)
            {
                if (item.Id == _groupId)
                {
                    group = item;
                    break;
                }
            }

            if (group == null)
                return "#FFFFFF";

            if (group.WorkHoursInDay == null)
                return "#FFFFFF";

            //group flex
            if (group.FlexibleArrivalEnabled)
            {
                if (studentArrival > group.ArrivalTime.Add(group.FlexibleAmount ?? new()))
                {
                    return ReturnColor(seeAll);
                }
            }
            //group arrivaltime
            else if (studentArrival > group.ArrivalTime)
            {
                return ReturnColor(seeAll);
            }

            //group departure
            if (studentDeparture != new TimeOnly())
            {
                TimeOnly arrivalBuffer = studentArrival;
                if (studentArrival < group.ArrivalTime)
                    arrivalBuffer = group.ArrivalTime;

                TimeSpan time;

                switch ((int)date.DayOfWeek)
                {
                    case 1:
                        time = group.WorkHoursInDay.Monday;
                        break;
                    case 2:
                        time = group.WorkHoursInDay.Tuesday;
                        break;
                    case 3:
                        time = group.WorkHoursInDay.Wednesday;
                        break;
                    case 4:
                        time = group.WorkHoursInDay.Thursday;
                        break;
                    case 5:
                        time = group.WorkHoursInDay.Friday;
                        break;
                    default:
                        return red;
                }

                TimeSpan? timeWorked = studentDeparture - arrivalBuffer;

                if (!timeWorked.HasValue || timeWorked.Value < time)
                    return ReturnColor(seeAll);
            }
            return "#FFFFFF";
        }
    }
}
