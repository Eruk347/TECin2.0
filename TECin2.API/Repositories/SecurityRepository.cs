using TECin2.API.Database;
using TECin2.API.Database.Entities;
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

    public class SecurityRepository : ISecurityRepository
    {
        private readonly TECinContext2 _context2;

        public SecurityRepository(TECinContext2 context2)
        {
            _context2 = context2;
        }

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<SecurityNumb?> DeleteSecurityNumb(string securityNumbId)
        {
            try
            {
                SecurityNumb? deletedSecurityNumb = await _context2.SecurityNumb.
                    FirstOrDefaultAsync(securityNumb => securityNumb.Id == securityNumbId);
                if (deletedSecurityNumb != null)
                {
                    _context2.SecurityNumb.Remove(deletedSecurityNumb);
                    await _context2.SaveChangesAsync();
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
                _context2.SecurityNumb.Add(securityNumb);
                await _context2.SaveChangesAsync();
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
                return await _context2.SecurityNumb.FirstOrDefaultAsync(securityNumb => securityNumb.Id == securityNumbId);
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
                return await _context2.SecurityNumb.FirstOrDefaultAsync(securityNumb => securityNumb.Cipher == _securityNumb);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSecurityNumbByCPR", e);
                return null;
            }
        }
    }
}
