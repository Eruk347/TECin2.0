using TECin2.ClassLibrary.Entities;
using TECin2.ClassLibrary.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IDailyOverviewService
    {
        Task<List<CheckInResponseLong>> GetCheckInsForToday(int groupId, DateOnly today);
    }

    public class DailyOverviewService : IDailyOverviewService
    {

        private const string _URL = Global.URL + "CheckIn/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
        public async Task<List<CheckInResponseLong>> GetCheckInsForToday(int groupId, DateOnly today)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                string information = groupId.ToString() + "," + today.ToString("yyyyMMdd");
                var response = await client.GetAsync(_URL + information);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return [];
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<CheckInResponseLong>>() ?? [];
                    return result;
                }
                return [];
            }
            catch (Exception e)
            {
                WriteToLog("Create", e);
                return null;
            }
        }
    }
}
