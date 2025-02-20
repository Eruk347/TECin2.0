using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface IWorkHoursInDayRepository
    {
        Task<WorkHoursInDay?> DeleteWorkHoursInDay(int workHoursInDayId);
        Task<WorkHoursInDay?> InsertNewWorkHoursInDay(WorkHoursInDay workHoursInDay);
        Task<WorkHoursInDay?> SelectWorkHoursInDayById(int workHoursInDayId);
        Task<WorkHoursInDay?> UpdateWorkHoursInDay(int workHoursInDayId, WorkHoursInDay workHoursInDay);

    }
    public class WorkHoursInDayRepository(TECinContext context) : IWorkHoursInDayRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<WorkHoursInDay?> DeleteWorkHoursInDay(int workHoursInDayId)
        {
            try
            {
                WorkHoursInDay? deletedWorkHoursInDay = await _context.WorkHoursInDay
                    .FirstOrDefaultAsync(workHoursInDay => workHoursInDay.Id == workHoursInDayId);
                if (deletedWorkHoursInDay != null)
                {
                    _context.WorkHoursInDay.Remove(deletedWorkHoursInDay);
                    await _context.SaveChangesAsync();
                }
                return deletedWorkHoursInDay;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteWorkHoursInDay", e);
                return null;
            }
        }

        public async Task<WorkHoursInDay?> InsertNewWorkHoursInDay(WorkHoursInDay workHoursInDay)
        {
            try
            {
                _context.WorkHoursInDay.Add(workHoursInDay);
                await _context.SaveChangesAsync();
                return await _context.WorkHoursInDay
                    .FirstOrDefaultAsync(workHoursInDay => workHoursInDay.Id == workHoursInDay.Id);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewWorkHoursInDay", e);
                return null;
            }
        }

        public async Task<WorkHoursInDay?> SelectWorkHoursInDayById(int workHoursInDayId)
        {
            try
            {
                return await _context.WorkHoursInDay
                    .FirstOrDefaultAsync(workHoursInDay => workHoursInDay.Id == workHoursInDayId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectWorkHoursInDayById", e);
                return null;
            }
        }

        public async Task<WorkHoursInDay?> UpdateWorkHoursInDay(int workHoursInDayId, WorkHoursInDay workHoursInDay)
        {
            try
            {
                WorkHoursInDay? updatedWorkHoursInDay = await _context.WorkHoursInDay
                    .FirstOrDefaultAsync(workHoursInDay => workHoursInDay.Id == workHoursInDayId);
                if (updatedWorkHoursInDay != null)
                {
                    updatedWorkHoursInDay.Monday = workHoursInDay.Monday;
                    updatedWorkHoursInDay.Tuesday = workHoursInDay.Tuesday;
                    updatedWorkHoursInDay.Wednesday = workHoursInDay.Wednesday;
                    updatedWorkHoursInDay.Thursday = workHoursInDay.Thursday;
                    updatedWorkHoursInDay.Friday = workHoursInDay.Friday;
                    await _context.SaveChangesAsync();
                }
                return await _context.WorkHoursInDay
                    .FirstOrDefaultAsync(workHoursInDay => workHoursInDay.Id == workHoursInDayId);
            }
            catch (Exception e)
            {
                WriteToLog("UpdateWorkHoursInDay", e);
                return null;
            }
        }
    }
}
