using System.IdentityModel.Tokens.Jwt;

namespace TECin2.API.Services
{
    public static class Global
    {
        public enum CheckInStatus
        {
            Hello,
            Goodbye,
            Early,
            Late,            
            Error
        }

        public enum LeavingEarly
        {
            No,
            Yes,
            Flex,
        }

        public static readonly List<string> Tokens = [];

        public static List<string> GetTokens()
        {
            return Tokens;
        }

        public static void AddTokens(string _token)
        {
            Tokens.Add(_token);
        }

        public static bool RemoveToken(string _token)
        {
            return Tokens.Remove(_token);
        }

        public static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        public static bool CheckTokenValid(string token)
        {
            var tokenPart = token.Split(',')[0];
            var tokenTicks = GetTokenExpirationTime(tokenPart);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks);

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

        public static TimeOnly ConvertStringToTimeOnly(string time)//input string(071523) - output TimeOnly(07:15:23)
        {
            try
            {
                if (time.Contains(':'))
                {
                    string[] timeSplit = time.Split(':');

                    int hours = Convert.ToInt32(timeSplit[0]);
                    int minutes = Convert.ToInt32(timeSplit[1]);

                    TimeOnly answer = new(hours, minutes);

                    return answer;
                }
                else if (time.Length == 6)
                {
                    int hours = Convert.ToInt32(time[..2]);
                    int minutes = Convert.ToInt32(time.Substring(2, 2));
                    int seconds = Convert.ToInt32(time.Substring(4, 2));

                    TimeOnly answer = new(hours, minutes, seconds);

                    return answer;
                }
                return new TimeOnly();
            }
            catch (Exception)
            {
                return new TimeOnly();

            }
        }

        public static TimeSpan ConvertStringToTimeSpan(string time)//input string(071523) - output TimeSpan(07:15:23)
        {
            try
            {
                if (time.Contains(':'))
                {
                    string[] timeSplit = time.Split(':');

                    int hours = Convert.ToInt32(timeSplit[0]);
                    int minutes = Convert.ToInt32(timeSplit[1]);

                    TimeSpan answer = new(hours, minutes, 0);

                    return answer;
                }
                else if (time.Length == 6)
                {
                    int hours = Convert.ToInt32(time[..2]);
                    int minutes = Convert.ToInt32(time.Substring(2, 2));
                    int seconds = Convert.ToInt32(time.Substring(4, 2));

                    TimeSpan answer = new(hours, minutes, seconds);

                    return answer;
                }
                return new TimeSpan();
            }
            catch (Exception)
            {
                return new TimeSpan();

            }
        }

        public static string ConvertTimeOfDateTimeToStringTime(DateTime _dateTime)// 11:07:55 -> 110755
        {
            string timeBuffer = "";

            if (_dateTime.Hour < 10)
            {
                timeBuffer += "0" + _dateTime.Hour;
            }
            else
                timeBuffer += _dateTime.Hour;

            if (_dateTime.Minute < 10)
            {
                timeBuffer += "0" + _dateTime.Minute;
            }
            else
                timeBuffer += _dateTime.Minute;

            if (_dateTime.Second < 10)
            {
                timeBuffer += "0" + _dateTime.Second;
            }
            else
                timeBuffer += _dateTime.Second;
            return timeBuffer;
        }

        public static string ConvertDateOfDateTimeToStringDate(DateTime _dateTime)// 19-02-2023 -> 20230219
        {
            string dateBuffer = "" + _dateTime.Year;

            if (_dateTime.Month < 10)
            {
                dateBuffer += "0" + _dateTime.Month;
            }
            else
                dateBuffer += _dateTime.Month;

            if (_dateTime.Day < 10)
            {
                dateBuffer += "0" + _dateTime.Day;
            }
            else
                dateBuffer += _dateTime.Day;

            return dateBuffer;
        }
    }
}
