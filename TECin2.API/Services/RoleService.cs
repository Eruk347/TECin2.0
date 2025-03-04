using Azure.Core;
using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IRoleService
    {
        Task<RoleResponse?> Create(RoleRequest role, string accesstoken);
        Task<RoleResponse?> Delete(int roleId, string accesstoken);
        Task<RoleResponse?> GetById(int roleId);
        Task<List<RoleResponse?>> GetAll();
        Task<RoleResponse?> Update(int roleId, RoleRequest role, string accesstoken);
    }
    public class RoleService(IRoleRepository roleRepository, ILoggerService loggerService) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<RoleResponse?> Create(RoleRequest role, string accesstoken)
        {
            Role? newRole = MapRoleRequestToRole(role);

            if (newRole != null)
            {
                Role? inserted = await _roleRepository.InsertNewRole(newRole);
                if (inserted != null)
                {
                    _loggerService.WriteLog("Create", accesstoken, inserted);
                    return MapRoleToRoleResponse(inserted);
                }
            }
            return null;
        }

        public async Task<RoleResponse?> Delete(int roleId, string accesstoken)
        {
            Role? deletedRole = await _roleRepository.DeleteRole(roleId);

            if (deletedRole != null)
            {
                _loggerService.WriteLog("Delete", accesstoken, deletedRole);
                return MapRoleToRoleResponse(deletedRole);
            }
            return null;
        }

        public async Task<RoleResponse?> GetById(int roleId)
        {
            Role? role = await _roleRepository.SelectRoleById(roleId);

            if (role != null)
            {
                return MapRoleToRoleResponse(role);
            }
            return null;
        }

        public async Task<List<RoleResponse?>> GetAll()
        {
            List<Role> roles = await _roleRepository.SelectAlleRoles();

            return roles.Select(role => MapRoleToRoleResponse(role)).ToList() ?? [];
        }

        public async Task<RoleResponse?> Update(int roleId, RoleRequest updateRole, string accesstoken)
        {
            Role? originalRole = await _roleRepository.SelectRoleById(roleId);
            Role? role = MapRoleRequestToRole(updateRole);

            if (role != null)
            {
                Role? updatedRole = await _roleRepository.UpdateRole(roleId, role);
                if (updatedRole != null && originalRole != null)
                {
                    _loggerService.WriteLog(accesstoken, originalRole, updatedRole);
                    return MapRoleToRoleResponse(updatedRole);
                }
            }
            return null;
        }

        private Role? MapRoleRequestToRole(RoleRequest role)
        {
            try
            {
                return new()
                {
                    Description = role.Description,
                    Deactivated = role.Deactivated,
                    Name = role.Name,
                    Rank = role.Rank
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapRoleRequestToRole", e);
                return null;
            }
        }

        private RoleResponse? MapRoleToRoleResponse(Role role)
        {
            try
            {
                return new()
                {
                    Description = role.Description,
                    Deactivated = role.Deactivated,
                    Id = role.Id,
                    Name = role.Name,
                    Rank = role.Rank
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapRoleRequestToRole", e);
                return null;
            }
        }
    }
}
