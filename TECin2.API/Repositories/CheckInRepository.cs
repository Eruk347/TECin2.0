using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface ICheckInRepository
    {
        Task<List<CheckInStatus>?> DeleteAllCheckInStatusForUser(string userId);
        Task<CheckInStatus?> InsertCheckInStatus(CheckInStatus request);
        Task<List<CheckInStatus>?> SelectCheckInForUser(string userId);
        Task<List<CheckInStatus>?> SelectCheckInForDate(DateOnly today);
        Task<CheckInStatus?> UpdateCheckInStatus(CheckInStatus checkInStatus, int checkInStatusId);
    }

    public class CheckInRepository(TECinContext context) : ICheckInRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<List<CheckInStatus>?> DeleteAllCheckInStatusForUser(string userId)
        {
            try
            {
                List<CheckInStatus> checkInStatuses = await _context.CheckInStatus
                .Where(un => un.User_Id == userId)
                .ToListAsync();

                foreach (CheckInStatus uN in checkInStatuses)
                {
                    _context.CheckInStatus.Remove(uN);
                    await _context.SaveChangesAsync();
                }
                return checkInStatuses;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteAllCheckInStatusForUser", e);
                return null;
            }
        }

        public async Task<CheckInStatus?> InsertCheckInStatus(CheckInStatus checkIn)
        {
            try
            {
                _context.CheckInStatus.Add(checkIn);
                await _context.SaveChangesAsync();
                return await _context.CheckInStatus
                .Include(s => s.User)
                .FirstOrDefaultAsync(c => c.User_Id == checkIn.User_Id);
            }
            catch (Exception e)
            {
                WriteToLog("CheckIn", e);
                return null;
            }
        }

        public async Task<List<CheckInStatus>?> SelectCheckInForDate(DateOnly today)
        {
            try
            {
                List<CheckInStatus> checkIns = await _context.CheckInStatus
                    .Where(UN => UN.ArrivalDate == today)
                    .ToListAsync();
                return checkIns;
            }
            catch (Exception e)
            {
                WriteToLog("SelectCheckInForDate", e);
                return null;
            }
        }

        public async Task<List<CheckInStatus>?> SelectCheckInForUser(string userId)
        {
            try
            {
                return await _context.CheckInStatus
                    .Where(ci => ci.User_Id == userId)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectCheckInForUser", e);
                return null;
            }
        }

        public async Task<CheckInStatus?> UpdateCheckInStatus(CheckInStatus checkInStatus, int checkInStatusId)
        {
            try
            {
                CheckInStatus? updatedCheckInStatus = await _context.CheckInStatus.FirstOrDefaultAsync(checkInStatus => checkInStatus.Id == checkInStatusId);
                if (updatedCheckInStatus != null)
                {
                    updatedCheckInStatus.ArrivalDate = checkInStatus.ArrivalDate;
                    updatedCheckInStatus.ArrivalTime = checkInStatus.ArrivalTime;
                    updatedCheckInStatus.Departure = checkInStatus.Departure;
                    await _context.SaveChangesAsync();
                }
                return updatedCheckInStatus;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateCheckInStatus", e);
                return null;
            }

        }
    }
}
