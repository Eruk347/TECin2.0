using TECin2.API.Database;
using TECin2.ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace TECin2.API.Repositories
{
    public interface ISecurityRepository
    {

        Task<SecurityNumb?> SelectSecurityNumbById(string securityNumbId);
        Task<SecurityNumb?> DeleteSecurityNumb(string securityNumbId);
        Task<SecurityNumb?> InsertNewSecurityNumb(SecurityNumb securityNumb);
        Task<SecurityNumb?> SelectSecurityNumbByCPR(string _securityNumb);
    }

    public class SecurityRepository(TECinContext context) : ISecurityRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<SecurityNumb?> DeleteSecurityNumb(string securityNumbId)
        {
            try
            {
                SecurityNumb? deletedSecurityNumb = await _context.SecurityNumb.
                    FirstOrDefaultAsync(securityNumb => securityNumb.Id == securityNumbId);
                if (deletedSecurityNumb != null)
                {
                    _context.SecurityNumb.Remove(deletedSecurityNumb);
                    await _context.SaveChangesAsync();
                }
                return deletedSecurityNumb;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteSecurityNumb", e);
                return null;
            }
        }

        public async Task<SecurityNumb?> InsertNewSecurityNumb(SecurityNumb securityNumb)
        {
            try
            {
                _context.SecurityNumb.Add(securityNumb);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewSecurityNumb", e);
                return null;
            }
            return securityNumb;
        }

        public async Task<SecurityNumb?> SelectSecurityNumbById(string securityNumbId)
        {
            try
            {
                return await _context.SecurityNumb.FirstOrDefaultAsync(securityNumb => securityNumb.Id == securityNumbId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSecurityNumbById", e);
                return null;
            }
        }

        public async Task<SecurityNumb?> SelectSecurityNumbByCPR(string _securityNumb)
        {
            try
            {
                return await _context.SecurityNumb.FirstOrDefaultAsync(securityNumb => securityNumb.Cipher == _securityNumb);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSecurityNumbByCPR", e);
                return null;
            }
        }
    }
}
