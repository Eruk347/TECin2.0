using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IWorkHoursInDayService
    {
        Task<WorkHoursInDayResponse?> CreateWorkHoursInDay(WorkHoursInDayRequest workHoursInDay);
        Task<WorkHoursInDayResponse?> DeleteWorkHoursInDay(int workHoursInDayId);
        Task<WorkHoursInDayResponse?> GetWorkHoursInDay(int workHoursInDayId);
        Task<WorkHoursInDayResponse?> UpdateWorkHoursInDay(int workHoursInDayId, WorkHoursInDayRequest workHoursInDay);
    }
    public class WorkHoursInDayService(IWorkHoursInDayRepository workHoursInDayRepository, ILoggerService loggerService) : IWorkHoursInDayService
    {
        private readonly IWorkHoursInDayRepository _workHoursInDayRepository = workHoursInDayRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<WorkHoursInDayResponse?> CreateWorkHoursInDay(WorkHoursInDayRequest newWorkHoursInDay)
        {
            WorkHoursInDay? workHours = MapWorkHoursInDayRequestToWorkHoursInDay(newWorkHoursInDay);

            if (workHours != null)
            {
                WorkHoursInDay? inserted = await _workHoursInDayRepository.InsertNewWorkHoursInDay(workHours);

                if (inserted != null)
                {
                    //_loggerService.WriteLog("Create",access)
                    return MapWorkHoursInDayToWorkHoursInDayResponse(inserted);
                }
            }
            return null;
        }

        public async Task<WorkHoursInDayResponse?> DeleteWorkHoursInDay(int workHoursInDayId)
        {
            WorkHoursInDay? deletedWorkHoursInDay = await _workHoursInDayRepository.DeleteWorkHoursInDay(workHoursInDayId);

            if (deletedWorkHoursInDay != null)
            {
                return MapWorkHoursInDayToWorkHoursInDayResponse(deletedWorkHoursInDay);
            }
            return null;
        }

        public async Task<WorkHoursInDayResponse?> GetWorkHoursInDay(int workHoursInDayId)
        {
            try
            {
                WorkHoursInDay? workHoursInDay = await _workHoursInDayRepository.SelectWorkHoursInDayById(workHoursInDayId);
                if (workHoursInDay != null)
                {
                    return MapWorkHoursInDayToWorkHoursInDayResponse(workHoursInDay);
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("GetWorkHoursInDay", e);
                return null;
            }
        }

        public async Task<WorkHoursInDayResponse?> UpdateWorkHoursInDay(int workHoursInDayId, WorkHoursInDayRequest workHoursInDayRequest)
        {
            try
            {
                WorkHoursInDay? workHoursInDay = MapWorkHoursInDayRequestToWorkHoursInDay(workHoursInDayRequest);
                if (workHoursInDay != null)
                {
                    WorkHoursInDay? updatedWorkHoursInDay = await _workHoursInDayRepository.UpdateWorkHoursInDay(workHoursInDayId, workHoursInDay);
                    if (updatedWorkHoursInDay != null)
                    {
                        return MapWorkHoursInDayToWorkHoursInDayResponse(updatedWorkHoursInDay);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateWorkHoursInDay", e);
                return null;
            }
        }

        private WorkHoursInDay? MapWorkHoursInDayRequestToWorkHoursInDay(WorkHoursInDayRequest request)
        {
            try
            {
                return new()
                {
                    Monday = request.Monday,
                    Tuesday = request.Tuesday,
                    Wednesday = request.Wednesday,
                    Thursday = request.Thursday,
                    Friday = request.Friday,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupRequestToGroup", e);
                return null;
            }
        }

        private WorkHoursInDayResponse? MapWorkHoursInDayToWorkHoursInDayResponse(WorkHoursInDay workHoursInDay)
        {
            try
            {
                return new()
                {
                    Id = 1,
                    Monday = workHoursInDay.Monday,
                    Tuesday = workHoursInDay.Tuesday,
                    Wednesday = workHoursInDay.Wednesday,
                    Thursday = workHoursInDay.Thursday,
                    Friday = workHoursInDay.Friday,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupRequestToGroup", e);
                return null;
            }
        }
    }
}
