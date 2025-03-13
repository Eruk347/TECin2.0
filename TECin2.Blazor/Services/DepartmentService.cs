using TECin2.Blazor.Models;
using TECin2.Blazor.Models.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IDepartmentService//skal have rettet så metoderne brugger mapperen til at lave response om til entitet
    {
        Task<Department?> CreateDepartment(DepartmentRequest newDepartment);
        Task<Department?> DeleteDepartment(int departmentId);
        Task<List<Department?>> GetAllDepartments();
        Task<Department?> GetDepartmentById(int departmentId);
        Task<Department?> UpdateDepartment(DepartmentRequest updateDepartment, int departmentId);
    }
    public class DepartmentService : IDepartmentService
    {
        private const string _URL = Global.URL + "department/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Department?> CreateDepartment(DepartmentRequest newDepartment)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);

                var response = await client.PostAsJsonAsync(_URL, newDepartment);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DepartmentResponse>();
                    return MapDepartmentResponseToDepartment(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Create", e);
                return null;
            }
            return null;
        }

        public async Task<Department?> DeleteDepartment(int departmentId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + departmentId);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DepartmentResponse>();
                    return MapDepartmentResponseToDepartment(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Delete", e);
                return null;
            }
            return null;
        }

        public async Task<List<Department?>> GetAllDepartments()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<DepartmentResponse?>>();

                    if (result == null)
                        return [];

                    return [.. result.Select(dep => MapDepartmentResponseToDepartment(dep))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAll", e);
                return [];
            }
            return [];
        }

        public async Task<Department?> GetDepartmentById(int departmentId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + departmentId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DepartmentResponse>();
                    return MapDepartmentResponseToDepartment(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetById", e);
                return null;
            }
            return null;
        }

        public async Task<Department?> UpdateDepartment(DepartmentRequest updateDepartment, int departmentId)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.PutAsJsonAsync(_URL + departmentId, updateDepartment);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DepartmentResponse>();
                    return MapDepartmentResponseToDepartment(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Update", e);
                return null;
            }
            return null;
        }

        private Department? MapDepartmentResponseToDepartment(DepartmentResponse? departmentResponse)
        {
            if (departmentResponse == null)
                return null;
            try
            {
                Department answer = new()
                {
                    Name = departmentResponse.Name,
                    Deactivated = departmentResponse.Deactivated,
                    DepartmentHead = departmentResponse.DepartmentHead,
                    Id = departmentResponse.Id,
                    School = new()
                    {
                        Id = departmentResponse.School.Id,
                        Name = departmentResponse.School.Name,
                        Deactivated = departmentResponse.School.Deactivated,
                        Principal = departmentResponse.School.Principal
                    }
                };
                return answer;
            }
            catch (Exception e)
            {
                WriteToLog("Update", e);
                return null;
            }
        }


    }
}
