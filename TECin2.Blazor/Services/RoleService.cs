using TECin2.ClassLibrary;
using TECin2.ClassLibrary.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IRoleService
    {
        Task<Role?> CreateRole(Role newRole);
        Task<Role?> DeleteRole(int roleId);
        Task<List<Role?>> GetAllRoles();
        Task<Role?> GetRoleById(int roleId);
        Task<Role?> UpdateRole(Role updateRole, int roleId);
    }
    public class RoleService : IRoleService
    {
        private const string _URL = Global.URL + "role/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
        public async Task<Role?> CreateRole(Role newRole)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);

                var response = client.PostAsJsonAsync(_URL, newRole).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RoleResponse>();

                    return MapRoleResponseToRole(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("CreateRole", e);
                return null;
            }
            return null;
        }

        public async Task<Role?> DeleteRole(int roleId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + roleId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RoleResponse>();

                    return MapRoleResponseToRole(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("DeleteRole", e);
                return null;
            }
            return null;
        }

        public async Task<List<Role?>> GetAllRoles()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<RoleResponse?>>();

                    if (result == null)
                        return [];

                    return [.. result.Select(set => MapRoleResponseToRole(set))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAllRoles", e);
                return [];
            }
            return [];
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + roleId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RoleResponse>();

                    return MapRoleResponseToRole(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetRoleById", e);
                return null;
            }
            return null;
        }

        public async Task<Role?> UpdateRole(Role updateRole, int roleId)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.PutAsJsonAsync(_URL + roleId, updateRole);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RoleResponse>();

                    return MapRoleResponseToRole(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("UpdateRole", e);
                return null;
            }
            return null;
        }

        private Role? MapRoleResponseToRole(RoleResponse? roleResponse)
        {
            if (roleResponse == null)
                return null;
            try
            {
                Role answer = new()
                {
                    Id = roleResponse.Id,
                    Description = roleResponse.Description,
                    Name = roleResponse.Name,
                    Deactivated = roleResponse.Deactivated,
                    Rank = roleResponse.Rank
                };
                return answer;
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupResponseToGroup", e);
                return null;
            }
        }
    }
}
