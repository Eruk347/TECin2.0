using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IApprovedComputersService
    {
        Task<ApprovedComputerResponse?> CreateApprovedComputer(ApprovedComputerRequest newApprovedComputer, string accesstoken);
        Task<ApprovedComputerResponse?> DeleteApprovedComputer(int approvedComputerId, string accesstoken);
        Task<List<ApprovedComputerResponse?>> GetAllApprovedComputers();
        Task<ApprovedComputerResponse?> GetApprovedComputerById(int approvedComputerId);
        Task<ApprovedComputerResponse?> UpdateApprovedComputer(int approvedComputerId, ApprovedComputerRequest newApprovedComputer, string accesstoken);

    }
    public class ApprovedComputersService(IApprovedComputersRepository approvedComputersRepository, ILoggerService loggerService) : IApprovedComputersService
    {
        private readonly IApprovedComputersRepository _approvedComputersRepository = approvedComputersRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<ApprovedComputerResponse?> CreateApprovedComputer(ApprovedComputerRequest newApprovedComputer, string accesstoken)
        {
            ApprovedComputer? computer = MapApprovedComputerRequestToApprovedComputer(newApprovedComputer);
            if (computer != null)
            {
                ApprovedComputer? insertedComputer = await _approvedComputersRepository.InsertNewApprovedComputer(computer);
                if (insertedComputer != null)
                {
                    //await _loggerService.WriteLog("Create", accesstoken, insertedComputer);
                    return MapApprovedComputerToApprovedComputerResponse(insertedComputer);
                }
            }
            return null;
        }

        public async Task<ApprovedComputerResponse?> DeleteApprovedComputer(int approvedComputerId, string accesstoken)
        {
            ApprovedComputer? computer = await _approvedComputersRepository.DeleteApprovedComputer(approvedComputerId);

            if (computer != null)
            {
                //await _loggerService.WriteLog("Delete", accesstoken, computer);
                return MapApprovedComputerToApprovedComputerResponse(computer);
            }
            return null;
        }

        public async Task<List<ApprovedComputerResponse?>> GetAllApprovedComputers()
        {
            List<ApprovedComputer> computers = await _approvedComputersRepository.SelectAllApprovedComputers();
            return computers.Select(computer => MapApprovedComputerToApprovedComputerResponse(computer)).ToList() ?? [];
        }

        public async Task<ApprovedComputerResponse?> GetApprovedComputerById(int approvedComputerId)
        {
            ApprovedComputer? computer = await _approvedComputersRepository.SelectApprovedComputerById(approvedComputerId);
            if (computer != null)
            {
                return MapApprovedComputerToApprovedComputerResponse(computer);
            }
            return null;
        }

        public async Task<ApprovedComputerResponse?> UpdateApprovedComputer(int approvedComputerId, ApprovedComputerRequest newApprovedComputer, string accesstoken)
        {
            ApprovedComputer? computer = MapApprovedComputerRequestToApprovedComputer(newApprovedComputer);
            ApprovedComputer? originalComputer = await _approvedComputersRepository.SelectApprovedComputerById(approvedComputerId);
            if (computer != null)
            {
                ApprovedComputer? updatedComputer = await _approvedComputersRepository.UpdateApprovedComputer(approvedComputerId, computer);
                if (updatedComputer != null && originalComputer != null)
                {
                    //await _loggerService.WriteLog("Update", accesstoken, updatedComputer);
                    return MapApprovedComputerToApprovedComputerResponse(updatedComputer);
                }
            }
            return null;
        }

        private ApprovedComputer? MapApprovedComputerRequestToApprovedComputer(ApprovedComputerRequest approvedComputerRequest)
        {
            try
            {
                ApprovedComputer approvedComputer = new()
                {
                    ComputerLocationId = approvedComputerRequest.ComputerLocationId,
                    MACaddress = approvedComputerRequest.MACaddress,
                    DepartmentId = approvedComputerRequest.DepartmentId
                };
                return approvedComputer;
            }
            catch (Exception e)
            {
                WriteToLog("MapApprovedComputerRequestToApprovedComputer", e);
                return null;
            }
        }

        private ApprovedComputerResponse? MapApprovedComputerToApprovedComputerResponse(ApprovedComputer approvedComputer)
        {
            try
            {
                approvedComputer.ComputerLocation ??= new()
                {
                    Location = "Error"
                };

                ApprovedComputerResponse response = new()
                {
                    Id = approvedComputer.Id,
                    MACaddress = approvedComputer.MACaddress,

                    Location = new()
                    {
                        Location = approvedComputer.ComputerLocation.Location ?? "Error",
                        Id = approvedComputer.ComputerLocationId
                    },
                    Department = new()
                    {
                        Id = approvedComputer.DepartmentId,
                        Name = approvedComputer.Department?.Name ?? "Error"
                    }
                };

                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapApprovedComputerToApprovedComputerResponse", e);
                return null;
            }
        }
    }
}
