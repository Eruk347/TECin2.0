using System.IdentityModel.Tokens.Jwt;

namespace TECin2.Blazor.Services
{
    public class Global
    {
        private static string? Token;
        private static int RoleRank;//need this in the _layout.cshtml
        private static int GroupId;//need this in the _layout.cshtml
        //public const string URL = "http://localhost:8082/api/";// PROD og QA
        //public const string URL = "http://212.60.107.194:8082/api/"; //Test mod prod data
        public const string URL = "https://localhost:7223/api/"; //Local test

        private static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        ///<summary>
        ///Checks if a token is still valid
        /// </summary>
        public static bool CheckTokenValid(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks);

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

        public static string? GetToken()
        {
            return Token;
        }

        public static void SetToken(string? token)
        {
            Token = token;
        }

        public static int GetRoleRank()
        {
            return RoleRank;
        }

        public static void SetRoleRank(int roleRank)
        {
            RoleRank = roleRank;
        }

        public static int GetGroupId()
        {
            return GroupId;
        }

        public static void SetGroupId(int groupId)
        {
            GroupId = groupId;
        }
    }
}
