using Microsoft.AspNetCore.Components;
using TECin2.Blazor.Models.DTOs;
using TECin2.Blazor.Services;

namespace TECin2.Blazor.Components.Pages.DailyOverview
{
    public static class TableColor
    {
        private static string ReturnColor(bool seeAll)
        {
            string red = "#FF0000";
            string yellow = "#CDA01E";
            if (seeAll)
                return yellow;
            return red;
        }
        public static string Color(CheckInResponseLong checkInResponse, HttpContext httpContext)//husk SeeAll setting
        {
            string red = "#FF0000";
            if (checkInResponse.Arrival == new TimeOnly())
            {
                return red;
            }

            if (httpContext != null)
            {
                LogInGroupResponse? group = null;
                LogInResponse? _currentUserData = httpContext.Session.GetObject<LogInResponse>("CurrentUser");
                if (_currentUserData == null)
                    return red;

                bool seeAll = false;
                foreach (var item in _currentUserData.Settings)
                {
                    if (item.Name == "SeeAll")
                        seeAll = true;
                }

                foreach (var item in _currentUserData.Groups)
                {
                    if (item.Id == checkInResponse.GroupId)
                    {
                        group = item;
                        break;
                    }
                }

                if (group == null)
                    return red;

                if (group.WorkHoursInDay == null)
                    return red;

                //group flex
                if (group.FlexibleArrivalEnabled)
                {
                    if (checkInResponse.Arrival > group.ArrivalTime.Add(group.FlexibleAmount ?? new()))
                    {
                        return ReturnColor(seeAll);
                    }
                }
                //group arrivaltime
                else if (checkInResponse.Arrival > group.ArrivalTime)
                {
                    return ReturnColor(seeAll);
                }

                //group departure
                if (checkInResponse.Departure != new TimeOnly())
                {
                    TimeOnly arrivalBuffer = checkInResponse.Arrival;
                    if (checkInResponse.Arrival < group.ArrivalTime)
                        arrivalBuffer = group.ArrivalTime;

                    TimeSpan time;

                    switch ((int)DateTime.Now.DayOfWeek)
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

                    TimeSpan? timeWorked = checkInResponse.Departure - arrivalBuffer;

                    if (timeWorked != null)
                    {
                        if (timeWorked < time)
                        {
                            return ReturnColor(seeAll);
                        }
                    }
                }
                return "#FFFFFF";
            }
            return red;
        }
    }
}
