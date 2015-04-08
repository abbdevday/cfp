using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DevDayCFP.Common
{
    public static class Helpers
    {
        public static string EncodePassword(string originalPassword)
        {
            if (originalPassword == null)
                return String.Empty;

            string salt = ConfigurationManager.AppSettings[Consts.SettingKeys.PasswordSalt];
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword + salt);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }

        public static string CalculateMd5(string hashSource)
        {
            if (hashSource == null)
                return String.Empty;

            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(hashSource);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();
        }
    }
}