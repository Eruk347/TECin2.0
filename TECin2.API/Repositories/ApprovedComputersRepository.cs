using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.ClassLibrary.Entities;
namespace TECin2.API.Repositories
{
    public interface IApprovedComputersRepository
    {
        Task<ApprovedComputer?> DeleteApprovedComputer(int approvedComputerId);
        Task<ApprovedComputer?> InsertNewApprovedComputer(ApprovedComputer approvedComputer);
        Task<List<ApprovedComputer>> SelectAllApprovedComputers();
        Task<ApprovedComputer?> SelectApprovedComputerById(int approvedComputerId);
        Task<ApprovedComputer?> UpdateApprovedComputer(int approvedComputerId, ApprovedComputer approvedComputer);

    }
    public class ApprovedComputersRepository(TECinContext context) : IApprovedComputersRepository
    {
        private readonly TECinContext _context = context;

        public async Task<ApprovedComputer?> DeleteApprovedComputer(int approvedComputerId)
        {
            try
            {
                ApprovedComputer? deletedComputer = await _context.ApprovedComputers
                    .Include(a => a.ComputerLocation)
                    .FirstOrDefaultAsync(approvedComputer => approvedComputer.Id == approvedComputerId);
                if (deletedComputer != null)
                {
                    _context.ApprovedComputers.Remove(deletedComputer);
                    await _context.SaveChangesAsync();
                }
                return deletedComputer;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteApprovedComputer", e);
                return null;
            }
        }

        public async Task<ApprovedComputer?> InsertNewApprovedComputer(ApprovedComputer approvedComputer)
        {
            try
            {
                _context.ApprovedComputers.Add(approvedComputer);
                await _context.SaveChangesAsync();
                return _context.ApprovedComputers
                    .Include(a => a.ComputerLocation)
                    .Include(a => a.Department)
                    .FirstOrDefault(computer => computer.Id == approvedComputer.Id);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewApprovedComputer", e);
                return null;
            }

        }

        public async Task<List<ApprovedComputer>> SelectAllApprovedComputers()
        {
            try
            {
                return await _context.ApprovedComputers
                    .Include(a => a.ComputerLocation)
                    .Include(a => a.Department)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllApprovedComputer", e);
                return [];
            }
        }

        public async Task<ApprovedComputer?> SelectApprovedComputerById(int approvedComputerId)
        {
            try
            {
                return await _context.ApprovedComputers
                    .Include(a => a.ComputerLocation)
                    .Include(a => a.Department)
                    .FirstOrDefaultAsync(approvedComputer => approvedComputer.Id == approvedComputerId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectApprovedCopmuter", e);
                return null;
            }
        }

        public async Task<ApprovedComputer?> UpdateApprovedComputer(int approvedComputerId, ApprovedComputer approvedComputer)
        {
            try
            {
                ApprovedComputer? updatedComputer = await _context.ApprovedComputers
                    .Include(a => a.ComputerLocation)
                    .Include(a => a.Department)
                    .FirstOrDefaultAsync(computer => computer.Id == approvedComputerId);
                if (updatedComputer != null)
                {
                    updatedComputer.MACaddress = approvedComputer.MACaddress;
                    updatedComputer.ComputerLocationId = approvedComputer.ComputerLocationId;
                    await _context.SaveChangesAsync();
                }
                return updatedComputer;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateApprovedComputer", e);
                return null;
            }
        }

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
    }
}
