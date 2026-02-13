using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.ClassLibrary.Entities;

namespace TECin2.API.Repositories
{
    public interface IPasswordRepository
    {
        Task<Password?> DeletePassword(string userId);
        Task<Password?> InsertNewPassword(Password newPassword);
        Task<Password?> SelectPassword(string userId);
        Task<Password?> UpdatePassword(Password updatePassword);
    }

    public class PasswordRepository(TECinContext context) : IPasswordRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Password?> DeletePassword(string userId)
        {
            try
            {
                Password? deletedPassword = await _context.Password
                    .FirstOrDefaultAsync(password => password.Id == userId);
                if (deletedPassword != null)
                {
                    _context.Password.Remove(deletedPassword);
                    await _context.SaveChangesAsync();
                }
                return deletedPassword;
            }
            catch (Exception e)
            {
                WriteToLog("DeletePassword", e);
                return null;
            }
        }

        public async Task<Password?> InsertNewPassword(Password newPassword)
        {
            try
            {
                _context.Password.Add(newPassword);
                await _context.SaveChangesAsync();
                return newPassword;
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewPassword", e);
                return null;
            }
        }

        public async Task<Password?> SelectPassword(string userId)
        {
            try
            {
                return await _context.Password.FirstOrDefaultAsync(password => password.Id == userId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectPassword", e);
                return null;
            }
        }

        public async Task<Password?> UpdatePassword(Password updatePassword)
        {
            try
            {
                Password? updatedPassword = await _context.Password
                    .FirstOrDefaultAsync(id => id.Id == updatePassword.Id);
                if (updatedPassword != null)
                {
                    updatedPassword.Cipher = updatePassword.Cipher;
                    await _context.SaveChangesAsync();
                }
                return updatedPassword;
            }
            catch (Exception e)
            {
                WriteToLog("UpdatePassword", e);
                return null;
            }
        }
    }
}
