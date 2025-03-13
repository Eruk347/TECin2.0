using System.Security.Cryptography;
using System.Text;

namespace TECin2.Blazor.Services
{
    public class Hash
    {
        public const int SaltIndex = 1;
        public const int HashIndex = 0;

        private static void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in Hash.cs in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public static string? GetSalt(string fullPassword)
        {
            try
            {
                var split = fullPassword.Split(':');

                if (split.Length != 2)
                {
                    return null;
                }
                return split[SaltIndex];
            }
            catch (Exception e)
            {
                WriteToLog("Validate", e);
                return null;
            }
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] hashValue;
            UTF8Encoding objUtf8 = new();
            hashValue = SHA256.HashData(objUtf8.GetBytes(password + salt));

            return string.Format("{0}:{1}", Convert.ToBase64String(hashValue), salt);
        }

        public static bool Validate(string password, string passwordHash)
        {
            try
            {
                var split = passwordHash.Split(':');

                if (split.Length != 2)
                {
                    return false;
                }

                var hash = Convert.FromBase64String(split[HashIndex]);
                var salt = Convert.FromBase64String(split[SaltIndex]);

                UTF8Encoding objUtf8 = new();
                var hashTest = SHA256.HashData(objUtf8.GetBytes(password + salt));

                return Equals(hash, hashTest);
            }
            catch (Exception e)
            {
                WriteToLog("Validate", e);
                return false;
            }
        }

        private static bool Equals(byte[] a, byte[] b)
        {
            //Længden tjekkes om den forskellig ved at udføre en bitwise xor
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                //Der udføres en bitwise or på diff og resultatet af vores expression.
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}
