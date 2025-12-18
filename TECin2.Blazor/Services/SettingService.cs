using TECin2.ClassLibrary.Entities;
using TECin2.ClassLibrary.DTOs;

namespace TECin2.Blazor.Services
{
    public interface ISettingService
    {
        Task<Setting?> CreateSetting(Setting newSetting);
        Task<Setting?> DeleteSetting(int settingId);
        Task<List<Setting?>> GetAllSettings();
        Task<Setting?> GetSettingById(int settingId);
        Task<List<Setting?>> GetSettingsByUserId(string userId);
        Task<Setting?> UpdateSetting(Setting updateSetting, int settingId);
    }
    public class SettingService : ISettingService
    {
        private const string _URL = Global.URL + "setting/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
        public async Task<Setting?> CreateSetting(Setting newSetting)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);

                var response = await client.PostAsJsonAsync(_URL, newSetting);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SettingResponse>();

                    return MapSettignResponseToSetting(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("CreateSetting", e);
                return null;
            }
            return null;
        }

        public async Task<Setting?> DeleteSetting(int settingId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + settingId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SettingResponse>();

                    return MapSettignResponseToSetting(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("DeleteSetting", e);
                return null;
            }
            return null;
        }

        public async Task<List<Setting?>> GetAllSettings()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SettingResponse?>>();

                    if (result == null)
                        return [];

                    return [.. result.Select(set => MapSettignResponseToSetting(set))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAllSettings", e);
                return [];
            }
            return [];
        }

        public async Task<Setting?> GetSettingById(int settingId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + settingId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SettingResponse>();

                    return MapSettignResponseToSetting(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetSettingById", e);
                return null;
            }
            return null;
        }

        public async Task<List<Setting?>> GetSettingsByUserId(string userId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + userId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SettingResponse?>>();

                    if (result == null)
                        return [];

                    return [.. result.Select(set => MapSettignResponseToSetting(set))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetSettingsByUserId", e);
                return [];
            }
            return [];
        }

        public async Task<Setting?> UpdateSetting(Setting updateSetting, int settingId)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.PutAsJsonAsync(_URL + settingId, updateSetting);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SettingResponse>();

                    return MapSettignResponseToSetting(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Update", e);
                return null;
            }
            return null;
        }

        private Setting? MapSettignResponseToSetting(SettingResponse? settingResponse)//skal måske kige på users der kommer med....vil jeg gerne se hvem der ahr hvilke settings?
        {
            if (settingResponse == null)
                return null;
            try
            {
                Setting answer = new()
                {
                    Id = settingResponse.Id,
                    Name = settingResponse.Name,
                    Deactivated = settingResponse.Deactivated,
                    Description = settingResponse.Description,
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
