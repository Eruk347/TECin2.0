using Microsoft.EntityFrameworkCore;
using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ICheckInService
    {
        Task<CheckInResponse?> CheckIn(CheckInRequest checkInRequest);
        Task<List<CheckInResponseLong>> GetAllCheckInsFromGroup(int groupId, DateOnly date);
        Task<List<CheckInStatus>> GetCheckInstatusesForUser(string userId);
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
                string encryptedCpr = Hash.HashPassword(checkInRequest.CPR_number, "salt");
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

                TimeOnly checkinTime = TimeOnly.FromDateTime(checkInRequest.CheckinTime);
                CheckInStatus? checkIn = await _checkInRepository.SelectCheckInForUserOnDate(securityNumb.Id, DateOnly.FromDateTime(DateTime.Now));
                if (CheckIn == null)//der findes ikke et checkin, så det er første checkin for dagen
                {
                    bool isStudentLate = IsStudentLate(checkinTime, user.Groups.ToList()[0]);
                    if (isStudentLate)
                    {
                        return new CheckInResponse { FirstName = user.FirstName, Message = user.Groups.ToList()[0].IsLateMessage, Color = FindColor(Global.CheckInStatus.Late) };
                    }
                    return new CheckInResponse { FirstName = user.FirstName, Message = "Hej " + user.FirstName, Color = FindColor(Global.CheckInStatus.Hello) };
                }

                if (checkIn != null)
                {
                    if (checkinTime < checkIn.ArrivalTime.AddMinutes(30))
                    {
                        return new CheckInResponse { FirstName = user.FirstName, Message = "Du er allerede tjekket ind." + user.FirstName, Color = FindColor(Global.CheckInStatus.Hello) };
                    }

                    Global.LeavingEarly isStudentLeavingEarly = IsStudentLeavingEarly(checkinTime, user.Groups.ToList()[0]);
                    if (isStudentLeavingEarly == Global.LeavingEarly.No)
                    {
                        return new CheckInResponse { FirstName = user.FirstName, Message = "Farvel " + user.FirstName, Color = FindColor(Global.CheckInStatus.Goodbye) };
                    }
                    else if (isStudentLeavingEarly == Global.LeavingEarly.Yes)//der skal fikses noget her........
                    {
                        WorkHoursInDay? workHoursInDay = user.Groups.ToList()[0].WorkHoursInDay;
                        if (workHoursInDay != null)
                        {
                            DateTime today = DateTime.Now;
                            
                                return new CheckInResponse { FirstName = user.FirstName, Message = "Du har først fri kl. " + user.FirstName, Color = FindColor(Global.CheckInStatus.Goodbye) };
                        }
                    }
                    else if (isStudentLeavingEarly == Global.LeavingEarly.Flex)
                    {

                    }
                    //er der flextid??
                    //er der krav om check ud??
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("CheckIn", e);
                return null;
            }
        }

        public Task<List<CheckInResponseLong>> GetAllCheckInsFromGroup(int groupId, DateOnly date)
        {
            //hvilken medlemmer har vi?
            throw new NotImplementedException();
        }

        public Task<List<CheckInStatus>> GetCheckInstatusesForUser(string userId)
        {
            throw new NotImplementedException();
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

        private static Global.LeavingEarly IsStudentLeavingEarly(TimeOnly checkOutTime, Group group)
        {

        }

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
    }
}
