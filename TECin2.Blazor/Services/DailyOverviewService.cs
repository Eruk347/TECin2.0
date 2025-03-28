using TECin2.Blazor.Models;
using TECin2.Blazor.Models.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IDailyOverviewService
    {
        Task<List<CheckInResponseLong>?> GetCheckInsForToday(int groupId, DateOnly today);
    }

    public class DailyOverviewService : IDailyOverviewService
    {

        private const string _URL = Global.URL + "CheckIn/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
        public async Task<List<CheckInResponseLong>?> GetCheckInsForToday(int groupId, DateOnly today)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                string information = groupId.ToString() + "," + today.ToString("yyyyMMDD");
                var response = await client.GetAsync(_URL + information);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<CheckInResponseLong>?>();
                    return result;
                }
            }
            catch (Exception e)
            {
                WriteToLog("Create", e);
                return null;
            }
            return [];
        }
    }
}
