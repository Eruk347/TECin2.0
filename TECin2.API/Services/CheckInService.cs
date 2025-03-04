using Microsoft.EntityFrameworkCore;
using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ICheckInService
    {
        Task<CheckInResponse?> CheckIn(CheckInRequest checkInRequest);
        Task<List<CheckInResponseLong?>> GetAllCheckInStatusesFromGroup(int groupId, DateOnly date);
        Task<List<CheckInStatus>> GetAllCheckInStatusesForUser(string userId);
    }
    public class CheckInService(ICheckInRepository checkInRepository, ISecurityRepository securityRepository, IUserRepository userRepository, IGroupRepository groupRepository) : ICheckInService
    {
        private readonly ICheckInRepository _checkInRepository = checkInRepository;
        private readonly ISecurityRepository _securityRepository = securityRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IGroupRepository _groupRepository = groupRepository;

        public async Task<CheckInResponse?> CheckIn(CheckInRequest checkInRequest)
        {
            try
            {
                string encryptedCpr = Hash.HashPassword(checkInRequest.CPR_number, "sugar");//!!!!! HUSK SALT !!!!!!
                SecurityNumb? securityNumb = await _securityRepository.SelectSecurityNumbByCPR(encryptedCpr);

                if (securityNumb == null)
                {
                    return new CheckInResponse { FirstName = "", Message = "Lærling findes ikke", Color = FindColor(Global.CheckInStatus.Error) };
                }

                User? user = await _userRepository.SelectUserById(securityNumb.Id);
                if (user == null || user.Groups == null)
                {
                    WriteToLog("CheckIn", new Exception("CPR findes, men ingen bruger tilknyttet."));
                    return new CheckInResponse { FirstName = "", Message = "Lærling findes ikke", Color = FindColor(Global.CheckInStatus.Error) };
                }

                TimeOnly checkinTimeFromRequest = TimeOnly.FromDateTime(checkInRequest.CheckinTime);
                CheckInStatus? firstCheckIn = await _checkInRepository.SelectCheckInForUserOnDate(securityNumb.Id, DateOnly.FromDateTime(DateTime.Now));
                if (firstCheckIn == null)//der findes ikke et checkin, så det er første checkin for dagen
                {
                    CheckInStatus newCheckInStatus = MapCheckInRequestToCheckInStatus(checkInRequest, user.Id);
                    CheckInStatus? insertedCheckInStatus = await _checkInRepository.InsertCheckInStatus(newCheckInStatus);
                    if (insertedCheckInStatus == null)
                    {
                        WriteToLog("CheckIn", new Exception("Der skete en fejl ved indsættelse af checkin"));
                        return new CheckInResponse { FirstName = "", Message = "Der skete en fejl", Color = FindColor(Global.CheckInStatus.Error) };
                    }
                    bool isStudentLate = IsStudentLate(checkinTimeFromRequest, user.Groups.ToList()[0]);
                    if (isStudentLate && user.Groups.ToList()[0].IsLateMessage != null)
                    {
                        return new CheckInResponse { FirstName = user.FirstName, Message = user.Groups.ToList()[0].IsLateMessage, Color = FindColor(Global.CheckInStatus.Late) };
                    }
                    return new CheckInResponse { FirstName = user.FirstName, Message = "Hej " + user.FirstName, Color = FindColor(Global.CheckInStatus.Hello) };
                }

                if (checkinTimeFromRequest < firstCheckIn.ArrivalTime.AddMinutes(30))
                {
                    return new CheckInResponse { FirstName = user.FirstName, Message = "Du er allerede tjekket ind." + user.FirstName, Color = FindColor(Global.CheckInStatus.Hello) };
                }

                CheckInStatus checkInStatus = MapCheckInRequestToCheckInStatus(checkInRequest, user.Id);
                CheckInStatus? updatedCheckInStatus = await _checkInRepository.UpdateCheckInStatus(checkInStatus, firstCheckIn.Id);

                if (updatedCheckInStatus == null)
                {
                    WriteToLog("CheckIn", new Exception("Der skete en fejl ved opdatering af checkin"));
                    return new CheckInResponse { FirstName = "", Message = "Der skete en fejl", Color = FindColor(Global.CheckInStatus.Error) };
                }

                Global.LeavingEarly isStudentLeavingEarly = IsStudentLeavingEarly(firstCheckIn.ArrivalTime, checkinTimeFromRequest, user.Groups.ToList()[0]);
                if (isStudentLeavingEarly == Global.LeavingEarly.No)
                {
                    return new CheckInResponse { FirstName = user.FirstName, Message = "Farvel " + user.FirstName, Color = FindColor(Global.CheckInStatus.Goodbye) };
                }
                else if (isStudentLeavingEarly == Global.LeavingEarly.Yes)
                {
                    string message = ReturnMessage(firstCheckIn.ArrivalTime, checkinTimeFromRequest, user.Groups.ToList()[0], user.FirstName);
                    return new CheckInResponse { FirstName = user.FirstName, Message = message, Color = FindColor(Global.CheckInStatus.Early) };
                }

                return null;
            }
            catch (Exception e)
            {
                WriteToLog("CheckIn", e);
                return null;
            }
        }

        public async Task<List<CheckInResponseLong?>> GetAllCheckInStatusesFromGroup(int groupId, DateOnly date)
        {
            Group? group = await _groupRepository.SelectGroupById(groupId);
            if (group == null || group.Users == null)
            {
                return [];
            }

            List<User>? users = [.. group.Users];
            List<CheckInResponseLong?> checkInResponses = [];
            foreach (User user in users)
            {
                CheckInStatus? checkInStatus = await _checkInRepository.SelectCheckInForUserOnDate(user.Id, date);//?? new CheckInStatus() { ArrivalDate = date, ArrivalTime = new(), User_Id = user.Id };
                if (checkInStatus != null)
                {
                    checkInResponses.Add(new CheckInResponseLong
                    {
                        Arrival = checkInStatus.ArrivalTime,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserId = user.Id,
                        Email = user.Email,
                        Phonenumber = user.Phonenumber,
                        LastCheckin = user.LastCheckin,
                        Departure = checkInStatus.Departure,
                    });
                }
            }
            return checkInResponses;
        }

        public async Task<List<CheckInStatus>> GetAllCheckInStatusesForUser(string userId)
        {
            List<CheckInStatus>? checkInStatuses = await _checkInRepository.SelectCheckInForUser(userId);
            if (checkInStatuses != null)
            {
                return checkInStatuses;
            }
            return [];
        }

        private static byte[] FindColor(Global.CheckInStatus message)
        {
            byte[] turkis = [0xff, 0x28, 0xcd, 0xaf];
            byte[] koralRoed = [0xFF, 0xfa, 0x50, 0x50];
            byte[] sennepsGul = [0xFF, 0xcd, 0xa0, 0x1e];

            if (message == Global.CheckInStatus.Early || message == Global.CheckInStatus.Late)
                return sennepsGul;

            if (message == Global.CheckInStatus.Hello || message == Global.CheckInStatus.Goodbye)
                return turkis;

            return koralRoed;
        }

        private static bool IsStudentLate(TimeOnly checkinTime, Group group)
        {
            if (group.FlexibleAmount != null)
            {
                TimeSpan flexSpan = new(group.FlexibleAmount.Value.Hour, group.FlexibleAmount.Value.Minute, 0);
                if (checkinTime > group.ArrivalTime.Add(flexSpan))
                {
                    return true;
                }
                return false;
            }

            if (group.IsLateBuffer != null)
            {
                TimeSpan isLateBuffer = new(group.IsLateBuffer.Value.Hour, group.IsLateBuffer.Value.Minute, 0);
                if (checkinTime > group.ArrivalTime.Add(isLateBuffer))
                {
                    return true;
                }
                return false;
            }
            if (checkinTime > group.ArrivalTime)
            {
                return true;
            }
            return false;
        }

        private static Global.LeavingEarly IsStudentLeavingEarly(TimeOnly arrivalTime, TimeOnly checkOutTime, Group group)
        {
            if (arrivalTime < group.ArrivalTime)
            {
                arrivalTime = group.ArrivalTime;
            }
            if (group.FlexibleArrivalEnabled)
            {
                if (group.FlexibleAmount == null)
                {
                    group.FlexibleAmount = new TimeOnly(0, 0, 0);
                }

                TimeSpan flexSpan = new(group.FlexibleAmount.Value.Hour, group.FlexibleAmount.Value.Minute, 0);
                if (group.FlexibleArrivalEnabled && arrivalTime > group.ArrivalTime.Add(flexSpan))
                {
                    arrivalTime = group.ArrivalTime.Add(flexSpan);
                }
            }
            group.WorkHoursInDay ??= new()
            {
                Monday = new(7, 30, 0),
                Tuesday = new(7, 30, 0),
                Wednesday = new(7, 30, 0),
                Thursday = new(7, 30, 0),
                Friday = new(7, 0, 0),
            };

            switch ((int)DateTime.Now.DayOfWeek)
            {
                case 0:
                    return Global.LeavingEarly.No;
                case 1:
                    if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Monday.ToTimeSpan()))
                        return Global.LeavingEarly.Yes;
                    break;
                case 2:
                    if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Tuesday.ToTimeSpan()))
                        return Global.LeavingEarly.Yes;
                    break;
                case 3:
                    if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Wednesday.ToTimeSpan()))
                        return Global.LeavingEarly.Yes;
                    break;
                case 4:
                    if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Thursday.ToTimeSpan()))
                        return Global.LeavingEarly.Yes;
                    break;
                case 5:
                    if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Friday.ToTimeSpan()))
                        return Global.LeavingEarly.Yes;
                    break;
                case 6: return Global.LeavingEarly.No;
            }
            return Global.LeavingEarly.No;
        }

        private static string ReturnMessage(TimeOnly arrivalTime, TimeOnly checkOutTime, Group group, string firstName)
        {
            if ((int)DateTime.Now.DayOfWeek == 0 || (int)DateTime.Now.DayOfWeek == 6)
                return firstName + ", det er weekend!";

            group.WorkHoursInDay ??= new()
            {
                Monday = new(7, 30, 0),
                Tuesday = new(7, 30, 0),
                Wednesday = new(7, 30, 0),
                Thursday = new(7, 30, 0),
                Friday = new(7, 0, 0),
            };
            if (group.FlexibleArrivalEnabled)
            {
                switch ((int)DateTime.Now.DayOfWeek)
                {
                    case 1:
                        if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Monday.ToTimeSpan()))
                            return firstName + ",\ndu tjekkede ind " + arrivalTime.ToString() + ",\ndu har først fri kl. " + arrivalTime.Add(group.WorkHoursInDay.Monday.ToTimeSpan());
                        break;
                    case 2:
                        if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Tuesday.ToTimeSpan()))
                            return firstName + ",\ndu tjekkede ind " + arrivalTime.ToString() + ",\ndu har først fri kl. " + arrivalTime.Add(group.WorkHoursInDay.Tuesday.ToTimeSpan());
                        break;
                    case 3:
                        if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Wednesday.ToTimeSpan()))
                            return firstName + ",\ndu tjekkede ind " + arrivalTime.ToString() + ",\ndu har først fri kl. " + arrivalTime.Add(group.WorkHoursInDay.Wednesday.ToTimeSpan());
                        break;
                    case 4:
                        if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Thursday.ToTimeSpan()))
                            return firstName + ",\ndu tjekkede ind " + arrivalTime.ToString() + ",\ndu har først fri kl. " + arrivalTime.Add(group.WorkHoursInDay.Thursday.ToTimeSpan());
                        break;
                    case 5:
                        if (checkOutTime < arrivalTime.Add(group.WorkHoursInDay.Friday.ToTimeSpan()))
                            return firstName + ",\ndu tjekkede ind " + arrivalTime.ToString() + ",\ndu har først fri kl. " + arrivalTime.Add(group.WorkHoursInDay.Friday.ToTimeSpan());
                        break;

                }
            }
            switch ((int)DateTime.Now.DayOfWeek)
            {
                case 1:
                    if (checkOutTime < group.ArrivalTime.Add(group.WorkHoursInDay.Monday.ToTimeSpan()))
                        return "Du har først fri kl. " + group.ArrivalTime.Add(group.WorkHoursInDay.Monday.ToTimeSpan());
                    break;
                case 2:
                    if (checkOutTime < group.ArrivalTime.Add(group.WorkHoursInDay.Tuesday.ToTimeSpan()))
                        return "Du har først fri kl. " + group.ArrivalTime.Add(group.WorkHoursInDay.Tuesday.ToTimeSpan());
                    break;
                case 3:
                    if (checkOutTime < group.ArrivalTime.Add(group.WorkHoursInDay.Wednesday.ToTimeSpan()))
                        return "Du har først fri kl. " + group.ArrivalTime.Add(group.WorkHoursInDay.Wednesday.ToTimeSpan());
                    break;
                case 4:
                    if (checkOutTime < group.ArrivalTime.Add(group.WorkHoursInDay.Thursday.ToTimeSpan()))
                        return "Du har først fri kl. " + group.ArrivalTime.Add(group.WorkHoursInDay.Thursday.ToTimeSpan());
                    break;
                case 5:
                    if (checkOutTime < group.ArrivalTime.Add(group.WorkHoursInDay.Friday.ToTimeSpan()))
                        return "Du har først fri kl. " + group.ArrivalTime.Add(group.WorkHoursInDay.Friday.ToTimeSpan());
                    break;
            }
            return "Farvel " + firstName;
        }

        private static CheckInStatus MapCheckInRequestToCheckInStatus(CheckInRequest checkInRequest, string userID)
        {
            return new CheckInStatus
            {
                ArrivalTime = TimeOnly.FromDateTime(checkInRequest.CheckinTime),
                User_Id = userID,
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
            };
        }

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
    }
}
