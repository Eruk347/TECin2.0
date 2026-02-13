using TECin2.ClassLibrary.DTOs;
namespace TECin2.Blazor.Services
{
    public interface IStudentCheckInService
    {
        //Task<CheckInResponseWEB> CheckIn(string _cpr);
        //Task<List<CheckInResponse>> GetAllStudentCheckIns(int groupId, DateTime selectedDate);
    }
    public class StudentCheckInService : IStudentCheckInService
    {
        //private const string _URL = Global.URL + "checkin/";

        //private void WriteToLog(string task, Exception e)
        //{
        //    LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        //}

        //public async Task<CheckInResponseWEB> CheckIn(string _cpr)
        //{
        //    try
        //    {
        //        string[] split = _cpr.Split('-');
        //        if (split.Length > 1)
        //        {
        //            _cpr = "";
        //            for (int i = 0; i < split.Count(); i++)
        //            {
        //                _cpr += split[i];
        //            }
        //        }

        //        CheckInRequest checkInRequest = new CheckInRequest { CheckinTime = DateTime.Now, CPR_number = Hash.HashPassword(_cpr, _cpr) };
        //        using var client = new HttpClient();
        //        client.BaseAddress = new Uri(_URL);
        //        var response = client.PostAsJsonAsync(_URL, checkInRequest).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = response.Content.ReadFromJsonAsync<CheckInResponseWEB>();
        //            result.Wait();
        //            return result.Result;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("CheckIn", e);
        //        return null;
        //    }
        //    return null;
        //}

        //public async Task<List<CheckInResponse>> GetAllStudentCheckIns(int groupId, DateTime selectedDate)
        //{
        //    try
        //    {
        //        using var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.Token);

        //        string infoDate = Global.ConvertToSaveDateTime(selectedDate);

        //        string information = groupId.ToString() + "," + infoDate;
        //        var responseTask = client.GetAsync(_URL + information);
        //        responseTask.Wait();
        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<CheckInResponse[]>();
        //            readTask.Wait();
        //            var students = readTask.Result;
        //            if (students != null)
        //            {
        //                return students.ToList();
        //            }
        //            return new List<CheckInResponse>();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("GetAll", e);
        //        return null;
        //    }
        //    return null;
        //}
    }
}
