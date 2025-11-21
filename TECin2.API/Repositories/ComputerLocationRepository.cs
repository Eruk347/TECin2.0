using Microsoft.EntityFrameworkCore;
using TECin2.ClassLibrary.DTOs;
using TECin2.ClassLibrary.Entities;

namespace TECin2.API.Repositories
{
    public interface IComputerLocationRepository
    {
        Task<ComputerLocation?> DeleteComputerLocation(int computerLocationId);
        Task<ComputerLocation?> InsertNewComputerLocation(ComputerLocation computerLocation);
        Task<List<ComputerLocation>> SelectAllComputerLocations();
        Task<ComputerLocation?> SelectComputerLocationById(int computerLocationId);
        Task<ComputerLocation?> UpdateComputerLocation(int computerLocationId, ComputerLocation computerLocation);
    }
    public class ComputerLocationRepository(TECinContext context) : IComputerLocationRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<ComputerLocation?> DeleteComputerLocation(int computerLocationId)
        {
            try
            {
                ComputerLocation? deletedComputerLocation = await _context.ComputerLocation
                    .Include(a => a.ApprovedComputers)
                    .FirstOrDefaultAsync(computerLocation => computerLocation.Id == computerLocationId);
                if (deletedComputerLocation != null)
                {
                    _context.ComputerLocation.Remove(deletedComputerLocation);
                    await _context.SaveChangesAsync();
                }
                return deletedComputerLocation;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteComputerLocation", e);
                return null;
            }
        }

        public async Task<ComputerLocation?> InsertNewComputerLocation(ComputerLocation computerLocation)
        {
            try
            {
                _context.ComputerLocation.Add(computerLocation);
                await _context.SaveChangesAsync();
                return _context.ComputerLocation
                    .Include(a => a.ApprovedComputers)
                    .FirstOrDefault(computerLocation => computerLocation.Location == computerLocation.Location);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewComputerLocation", e);
                return null;
            }
        }

        public async Task<List<ComputerLocation>> SelectAllComputerLocations()
        {
            try
            {
                return await _context.ComputerLocation
                    .Include(a => a.ApprovedComputers)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllComputerLocations", e);
                return [];
            }
        }

        public async Task<ComputerLocation?> SelectComputerLocationById(int computerLocationId)
        {
            try
            {
                return await _context.ComputerLocation
                    .Include(a => a.ApprovedComputers)
                    .FirstOrDefaultAsync(computerLocation => computerLocation.Id == computerLocationId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectComputerLocationById", e);
                return null;
            }
        }

        public async Task<ComputerLocation?> UpdateComputerLocation(int computerLocationId, ComputerLocation computerLocation)
        {
            try
            {
                ComputerLocation? updatedComputerLocation = await _context.ComputerLocation
                    .Include(a => a.ApprovedComputers)
                    .FirstOrDefaultAsync(computerLocation => computerLocation.Id == computerLocationId);
                if (updatedComputerLocation != null)
                {
                    updatedComputerLocation.Location = computerLocation.Location;
                    await _context.SaveChangesAsync();
                }
                return updatedComputerLocation;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateComputerLocation", e);
                return null;
            }
        }
    }
}
