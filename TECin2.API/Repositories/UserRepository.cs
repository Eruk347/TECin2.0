using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.ClassLibrary.Entities;

namespace TECin2.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> DeleteUser(string userId);
        Task<User?> InsertNewUser(User user);
        Task<List<User>> SelectAllUsers();
        Task<List<User>> SelectAllStudents();
        Task<List<User>> SelectAllStaff();
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

        // Helper: replace incoming User.Groups/Settings with tracked entities from the context
        private async Task AttachExistingGroupsAndSettingsAsync(User user)
        {
            if (user == null) return;

            if (user.Groups != null && user.Groups.Any())
            {
                var groupIds = user.Groups.Select(g => g.Id).ToList();
                var trackedGroups = await _context.Group.Where(g => groupIds.Contains(g.Id)).ToListAsync();
                user.Groups = trackedGroups;
            }

            if (user.Settings != null && user.Settings.Any())
            {
                var settingIds = user.Settings.Select(s => s.Id).ToList();
                var trackedSettings = await _context.Setting.Where(s => settingIds.Contains(s.Id)).ToListAsync();
                user.Settings = trackedSettings;
            }
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
                // Attach each Group in user.Groups to the context if not already tracked
                if (user.Groups != null)
                {
                    var groupIds = user.Groups.Select(g => g.Id).ToList();
                    var trackedGroups = await _context.Group.Where(g => groupIds.Contains(g.Id)).ToListAsync();
                    user.Groups = trackedGroups;
                }

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

        public async Task<List<User>> SelectAllUsers()
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
                return [];
            }
        }

        public async Task<List<User>> SelectAllStudents()
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
                return [];
            }
        }

        public async Task<List<User>> SelectAllStaff()
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(r => r.Role)
                    .Include(s => s.Settings)
                    .Where(user => user.IsStudent == false)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllStaff", e);
                return [];
            }
        }

        public async Task<User?> SelectUserById(string userId)
        {
            try
            {
                return await _context.User
                    .Include(s => s.Groups)
                    .Include(s => s.Settings)
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
                .Include(r => r.Role)
                .Include(s => s.Settings)
                .Include(g => g.Groups)
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
                    // Clear existing navigation collections safely
                    if (updatedUser.Settings != null)
                        updatedUser.Settings.Clear();
                    else
                        updatedUser.Settings = new List<Setting>();

                    if (updatedUser.Groups != null)
                        updatedUser.Groups.Clear();
                    else
                        updatedUser.Groups = new List<Group>();

                    await _context.SaveChangesAsync();
                }

                // Replace incoming references with tracked entities to avoid duplicate tracking exceptions
                await AttachExistingGroupsAndSettingsAsync(user);
                updatedUser = await _context.User
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
                    updatedUser.PrimaryGroupId = user.PrimaryGroupId;
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
