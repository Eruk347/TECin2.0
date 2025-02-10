using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public class LoggerRepository(TECinContext context)
    {
        private readonly TECinContext _context = context;

        public static async void WriteLog(string message)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateOnly today = new(now.Year, now.Month, now.Day);

                using StreamWriter file = new("c:\\TECin\\Log\\API\\" + today + "Log.txt", append: true);
                await file.WriteLineAsync("" + now + ": " + message);
                await file.WriteLineAsync("");
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public async Task<bool> WriteLog(Log log)
        {
            try
            {
                _context.Log.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                WriteLog("logwriter: " + e);
                throw;
            }
            return true;
        }

        public static Log GetAllLogs()
        {
            return null;
        }
    }
}
