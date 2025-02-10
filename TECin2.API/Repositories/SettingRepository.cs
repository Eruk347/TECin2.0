using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface ISettingRepository
    {

        Task<Setting?> DeleteSetting(int settingId);
        Task<Setting?> InsertNewSetting(Setting setting);
        Task<List<Setting>?> SelectAllSettings();
        Task<Setting?> SelectSettingById(int settingId);
        Task<List<Setting>?> SelectSettingsByUserId(string userId);
        Task<Setting?> UpdateSetting(int settinId, Setting setting);
    }
    public class SettingRepository(TECinContext context) : ISettingRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Setting?> DeleteSetting(int settingId)
        {
            try
            {
                Setting? deletedSetting = await _context.Setting.
                    FirstOrDefaultAsync(setting => setting.Id == settingId);
                if (deletedSetting != null)
                {
                    _context.Setting.Remove(deletedSetting);
                    await _context.SaveChangesAsync();
                }
                return deletedSetting;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteSetting", e);
                return null;
            }
        }

        public async Task<Setting?> InsertNewSetting(Setting setting)
        {
            try
            {
                _context.Setting.Add(setting);
                await _context.SaveChangesAsync();
                return setting;
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewSetting", e);
                return null;
            }
        }

        public async Task<List<Setting>?> SelectAllSettings()
        {
            try
            {
                return await _context.Setting.Include(u => u.Users).ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllSettings", e);
                return null;
            }
        }

        public async Task<Setting?> SelectSettingById(int settingId)
        {
            try
            {
                return await _context.Setting
                    .FirstAsync(setting => setting.Id == settingId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSettingById", e);
                return null;
            }
        }

        public async Task<List<Setting>?> SelectSettingsByUserId(string userId)//skal nok fjernes
        {
            try
            {
                return await _context.Setting.ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectSettingsByUserId", e);
                return null;
            }
        }

        public async Task<Setting?> UpdateSetting(int settinId, Setting setting)
        {
            try
            {
                Setting? updatedSetting = await _context.Setting.FirstOrDefaultAsync(setting => setting.Id == settinId);
                if (updatedSetting != null)
                {
                    updatedSetting.Name = setting.Name;
                    updatedSetting.Description = setting.Description;
                    updatedSetting.Deactivated = setting.Deactivated;
                    updatedSetting.Users = setting.Users;
                    await _context.SaveChangesAsync();
                }
                return updatedSetting;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateSetting", e);
                return null;
            }
        }
    }
}
