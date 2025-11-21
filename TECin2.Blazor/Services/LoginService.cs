using TECin2.ClassLibrary;
using TECin2.ClassLibrary.DTOs;

namespace TECin2.Blazor.Services
{
    public interface ILoginService
    {
        Task<LogInResponse?> Login(LogInRequest login);
    }
    public class LoginService : ILoginService
    {
        private const string _URL = Global.URL + "Login/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<LogInResponse?> Login(LogInRequest login)//måske skal vi se om vi kan kende forskel på om der ikke er forbindelse eller user/passw er forkert
        {
            try
            {
                LoggerService _logger = new();
                login.Password = Hash.HashPassword(login.Password, login.Password);

                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);

                var response = await client.PostAsJsonAsync(_URL, login);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LogInResponse>();

                    return result;
                }
            }
            catch (Exception e)
            {
                WriteToLog("Login", e);
                return null;
            }
            return null;
        }
    }
}
