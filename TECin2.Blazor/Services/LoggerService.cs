namespace TECin2.Blazor.Services
{
    public interface ILoggerService
    {
        void WriteLog(string message);
    }
    public class LoggerService
    {
        public static async void WriteLog(string message)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateOnly today = new DateOnly(now.Year, now.Month, now.Day);

                using StreamWriter file = new("c:\\TECin\\Log\\web\\" + today + ".txt", append: true);
                await file.WriteLineAsync("" + DateTime.Now + ": " + message + "\n");
                await file.WriteLineAsync("");
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}
