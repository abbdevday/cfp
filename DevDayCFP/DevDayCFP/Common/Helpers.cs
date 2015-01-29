using System;
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

            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword + "devSaltyBeacon"); // TODO: Get this out of code
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }
    }
}