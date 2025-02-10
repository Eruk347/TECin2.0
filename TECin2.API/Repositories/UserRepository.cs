using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> DeleteUser(string userId);
        Task<User?> InsertNewUser(User user);
        Task<List<User>?> SelectAllUsers();
        Task<List<User>?> SelectAllStudents();
        Task<List<User>?> SelectAllStaff();
        Task<User?> SelectUserById(string userId);
        Task<User?> SelectUserByUsername(string userName);
        Task<User?> UpdateUser(string userId, User user);
    }
    public class UserRepository(TECinContext context) : IUserRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<User?> DeleteUser(string userId)
        {
            try
            {
                User? deletedUser = await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if (deletedUser != null)
                {
                    _context.User.Remove(deletedUser);
                    await _context.SaveChangesAsync();
                }
                return deletedUser;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteUser", e);
                return null;
            }
        }

        public async Task<User?> InsertNewUser(User user)
        {
            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewUser", e);
                return null;
            }
        }

        public async Task<List<User>?> SelectAllUsers()
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllUsers", e);
                return null;
            }
        }

        public async Task<List<User>?> SelectAllStudents()
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Where(user => user.IsStudent == true)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllStudents", e);
                return null;
            }
        }

        public async Task<List<User>?> SelectAllStaff()
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .Where(user => user.IsStudent == false)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllStaff", e);
                return null;
            }
        }

        public async Task<User?> SelectUserById(string userId)
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(user => user.Id == userId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectUserById", e);
                return null;
            }
        }

        public async Task<User?> SelectUserByUsername(string userName)
        {
            try
            {
                return await _context.User
                .Include(g => g.Groups)
                .Include(r => r.Role)
                .Include(s => s.Settings)
                .FirstOrDefaultAsync(user => user.Username == userName);
            }
            catch (Exception e)
            {
                WriteToLog("SelectUserByUsername", e);
                return null;
            }
        }

        public async Task<User?> UpdateUser(string userId, User user)
        {
            try
            {
                User? updatedUser = await _context.User
                    .Include(g => g.Groups)
                    .Include(r => r.Role)
                    .Include(s => s.Settings)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if (updatedUser != null)
                {
                    updatedUser.FirstName = user.FirstName;
                    updatedUser.LastName = user.LastName;
                    updatedUser.Email = user.Email;
                    updatedUser.Phonenumber = user.Phonenumber;
                    updatedUser.Username = user.Username;
                    updatedUser.Deactivated = user.Deactivated;
                    updatedUser.RoleId = user.RoleId;
                    updatedUser.Settings = user.Settings;
                    updatedUser.LastCheckin = user.LastCheckin;
                    updatedUser.Groups = user.Groups;
                    await _context.SaveChangesAsync();
                }
                User? returnUser = await _context.User
                    .Include(g => g.Groups)
                    .Include(r => r.Role)
                    .Include(s => s.Settings)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                return returnUser;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateUser", e);
                return null;
            }
        }
    }
}
