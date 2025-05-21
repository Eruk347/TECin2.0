using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IComputerLocationService
    {
        Task<ComputerLocationResponse?> CreateComputerLocation(ComputerLocationRequest newComputerLocation, string accesstoken);
        Task<ComputerLocationResponse?> DeleteComputerLocation(int computerLocationId, string accesstoken);
        Task<List<ComputerLocationResponse?>> GetAllComputerLocations();
        Task<ComputerLocationResponse?> GetComputerLocationById(int computerLocationId);
        Task<ComputerLocationResponse?> UpdateComputerLocation(int computerLocationId, ComputerLocationRequest updateComputerLocation, string accesstoken);
    }

    public class ComputerLocationService(IComputerLocationRepository computerLocationRepository):IComputerLocationService
    {
        private readonly IComputerLocationRepository _computerLocationRepository = computerLocationRepository;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<ComputerLocationResponse?> CreateComputerLocation(ComputerLocationRequest newComputerLocation, string accesstoken)
        {
            ComputerLocation? computerLocation = MapComputerLocationRequestToComputerLocation(newComputerLocation);
            if (computerLocation != null)
            {
                ComputerLocation? insertedComputerLocation = await _computerLocationRepository.InsertNewComputerLocation(computerLocation);
                if (insertedComputerLocation != null)
                {
                    //await LoggerRepository.WriteLog("Create", accesstoken, insertedComputerLocation);
                    return MapComputerLocationToComputerLocationResponse(insertedComputerLocation);
                }
            }
            return null;
        }

        public async Task<ComputerLocationResponse?> DeleteComputerLocation(int computerLocationId, string accesstoken)
        {
            ComputerLocation? deletedComputerLocation = await _computerLocationRepository.DeleteComputerLocation(computerLocationId);
            if (deletedComputerLocation != null)
            {
                //await LoggerRepository.WriteLog("Delete", accesstoken, deletedComputerLocation);
                return MapComputerLocationToComputerLocationResponse(deletedComputerLocation);
            }
            return null;
        }

        public async Task<List<ComputerLocationResponse?>> GetAllComputerLocations()
        {
            List<ComputerLocation> computerLocations = await _computerLocationRepository.SelectAllComputerLocations();
            return computerLocations.Select(computerLocation => MapComputerLocationToComputerLocationResponse(computerLocation)).ToList() ?? [];
        }

        public async Task<ComputerLocationResponse?> GetComputerLocationById(int computerLocationId)
        {
            ComputerLocation? computerLocation = await _computerLocationRepository.SelectComputerLocationById(computerLocationId);
            if (computerLocation != null)
            {
                return MapComputerLocationToComputerLocationResponse(computerLocation);
            }
            return null;
        }

        public async Task<ComputerLocationResponse?> UpdateComputerLocation(int computerLocationId, ComputerLocationRequest updateComputerLocation, string accesstoken)
        {
            ComputerLocation? computerLocation = MapComputerLocationRequestToComputerLocation(updateComputerLocation);
            if (computerLocation != null)
            {
                ComputerLocation? updatedComputerLocation = await _computerLocationRepository.UpdateComputerLocation(computerLocationId, computerLocation);
                if (updatedComputerLocation != null)
                {
                    //await LoggerRepository.WriteLog("Update", accesstoken, updatedComputerLocation);
                    return MapComputerLocationToComputerLocationResponse(updatedComputerLocation);
                }
            }
            return null;
        }

        private ComputerLocation? MapComputerLocationRequestToComputerLocation(ComputerLocationRequest computerLocationRequest)
        {
            try
            {

                return new ComputerLocation
                {
                    Location = computerLocationRequest.Location,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapComputerLocationRequestToComputerLocation", e);
                return null;
            }
        }

        private ComputerLocationResponse? MapComputerLocationToComputerLocationResponse(ComputerLocation computerLocation)
        {
            try
            {
                return new ComputerLocationResponse
                {
                    Id = computerLocation.Id,
                    Location = computerLocation.Location,
                    ApprovedComputers = computerLocation.ApprovedComputers?.Select(approvedComputer => new LocationApprovedComputerResponse
                    {
                        Id = approvedComputer.Id,
                        MACaddress = approvedComputer.MACaddress,
                    }).ToList() ?? []
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapComputerLocationToComputerLocationResponse", e);
                return null;
            }
        }
    }
}
