using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ISettingService
    {

        Task<SettingResponse?> CreateSetting(SettingRequest newSetting);
        Task<SettingResponse?> DeleteSetting(int settingID);
        Task<List<SettingResponse?>> GetAllSetting();
        Task<SettingResponse?> GetSettingById(int settingId);
        Task<List<SettingResponse?>> GetSettingsByUserId(string userId);
        Task<SettingResponse?> UpdateSetting(int settingId, SettingRequest updateSetting);
    }
    public class SettingService(ISettingRepository settingRepository) : ISettingService
    {
        private readonly ISettingRepository _settingRepository = settingRepository;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<SettingResponse?> CreateSetting(SettingRequest newSetting)
        {
            Setting? setting = MapSettingRequestToSetting(newSetting);

            if (setting != null)
            {
                Setting? insertedSetting = await _settingRepository.InsertNewSetting(setting);

                if (insertedSetting != null)
                {
                    return MapSettingToSettingResponse(insertedSetting);
                }
            }
            return null;
        }

        public async Task<SettingResponse?> DeleteSetting(int settingID)
        {
            Setting? deletedSetting = await _settingRepository.DeleteSetting(settingID);
            if (deletedSetting != null)
            {
                return MapSettingToSettingResponse(deletedSetting);
            }
            return null;
        }

        public async Task<List<SettingResponse?>> GetAllSetting()
        {
            List<Setting> settings = await _settingRepository.SelectAllSettings();

            return settings.Select(setting => MapSettingToSettingResponse(setting)).ToList() ?? [];
        }

        public async Task<SettingResponse?> GetSettingById(int settingId)
        {
            Setting? setting = await _settingRepository.SelectSettingById(settingId);
            if (setting != null)
            {
                return MapSettingToSettingResponse(setting);
            }
            return null;
        }

        public async Task<List<SettingResponse?>> GetSettingsByUserId(string userId)
        {
            List<Setting> settings = await _settingRepository.SelectSettingsByUserId(userId);

            return settings.Select(setting => MapSettingToSettingResponse(setting)).ToList() ?? [];
        }

        public async Task<SettingResponse?> UpdateSetting(int settingId, SettingRequest updateSetting)
        {
            Setting? originalSetting = await _settingRepository.SelectSettingById(settingId);
            Setting? setting = MapSettingRequestToSetting(updateSetting);

            if (setting != null)
            {
                Setting? updatedSetting = await _settingRepository.UpdateSetting(settingId, setting);
                if (updatedSetting != null && originalSetting != null)
                {
                    return MapSettingToSettingResponse(updatedSetting);
                }
            }
            return null;
        }

        private Setting? MapSettingRequestToSetting(SettingRequest settingRequest)
        {
            try
            {
                return new Setting
                {
                    Name = settingRequest.Name,
                    Deactivated = settingRequest.Deactivated,
                    Description = settingRequest.Description,
                    Users = settingRequest.Users
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapSettingRequestToSetting", e);
                return null;
            }
        }

        private SettingResponse? MapSettingToSettingResponse(Setting setting)
        {
            try
            {
                SettingResponse response = new()
                {
                    Id = setting.Id,
                    Name = setting.Name,
                    Deactivated = setting.Deactivated,
                    Description = setting.Description ?? "",
                };
                if (setting.Users != null)
                {
                    response.Users = [.. setting.Users.Select(user => new SettingUserResponse
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phonenumber = user.Phonenumber,
                        RoleId = user.RoleId,
                        Deactivated = user.Deactivated
                    })];
                }

                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapSettingToSettingResponse", e);
                return null;
            }
        }
    }
}