using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface IRoleRepository
    {

        Task<Role?> DeleteRole(int roleId);
        Task<Role?> InsertNewRole(Role role);
        Task<List<Role>> SelectAlleRoles();
        Task<Role?> SelectRoleById(int roleId);
        Task<Role?> SelectRoleByName(string roleName);
        Task<Role?> UpdateRole(int roleId, Role role);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly TECinContext _context;
        public RoleRepository(TECinContext context)
        {
            _context = context;
        }

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Role?> DeleteRole(int roleId)
        {
            try
            {
                Role? deletedeRole = await _context.Role.
                    FirstOrDefaultAsync(role => role.Id == roleId);
                if (deletedeRole != null)
                {
                    _context.Role.Remove(deletedeRole);
                    await _context.SaveChangesAsync();
                }
                return deletedeRole;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteRole", e);
                return null;
            }
        }

        public async Task<Role?> InsertNewRole(Role role)
        {
            try
            {
                _context.Role.Add(role);
                await _context.SaveChangesAsync();
                return role;
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewRole", e);
                return null;
            }
        }

        public async Task<List<Role>> SelectAlleRoles()
        {
            try
            {
                return await _context.Role
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAlleRoles", e);
                return [];
            }
        }

        public async Task<Role?> SelectRoleById(int roleId)
        {
            try
            {
                return await _context.Role
                    .FirstOrDefaultAsync(role => role.Id == roleId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectRoleById", e);
                return null;
            }
        }

        public async Task<Role?> SelectRoleByName(string roleName)
        {
            try
            {
                return await _context.Role
                    .FirstOrDefaultAsync(role => role.Name == roleName);
            }
            catch (Exception e)
            {
                WriteToLog("SelectRoleById", e);
                return null;
            }
        }

        public async Task<Role?> UpdateRole(int roleId, Role role)
        {
            try
            {
                Role? updatedRole = await _context.Role.FirstOrDefaultAsync(role => role.Id == roleId);
                if (updatedRole != null)
                {
                    updatedRole.Name = role.Name;
                    updatedRole.Description = role.Description;
                    updatedRole.Deactivated = role.Deactivated;
                    updatedRole.Rank = role.Rank;
                    await _context.SaveChangesAsync();
                }
                return updatedRole;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateRole", e);
                return null;
            }
        }
    }
}
