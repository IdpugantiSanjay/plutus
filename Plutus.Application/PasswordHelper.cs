using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Plutus.Application
{
    public static class PasswordHelper
    {
        public static string GeneratePassword(string password, string username)
        {
            byte[] saltedPassword = StrToBytes(password).Concat(StrToBytes(username)).ToArray();
            return Encoding.UTF8.GetString(new SHA256Managed().ComputeHash(saltedPassword));

            static byte[] StrToBytes(string str) => Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Checks if entered password is same as stored password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <param name="storedPassword"></param>
        /// <returns></returns>
        public static bool ConfirmPassword(string password, string username, string storedPassword) => GeneratePassword(password, username) == storedPassword;
    }
}