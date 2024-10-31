using CalendarProject.EntityFramework;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CalendarProject
{
    internal class SessionContext
    {
        public static User CurrentUser { get; set; }
        public static Settings CurrentSettings { get; set; }

        public static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static bool ValidatePassword(string password)
        {
            return !string.IsNullOrEmpty(password);
        }

        public static bool ValidateEmail(string email)
        {
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9\.]+@[a-zA-Z0-9]+\.[a-zA-Z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return !string.IsNullOrWhiteSpace(email) && emailRegex.IsMatch(email);
        }
    }
}
