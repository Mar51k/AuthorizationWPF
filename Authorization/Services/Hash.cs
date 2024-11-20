using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services
{
    internal class Hash
    {
        public static string HashPassword(string password)
        {
            using (SHA256 Hash = SHA256.Create())
            {
                byte[] sourceBytePassword = Encoding.UTF8.GetBytes(password);
                byte[] hash_ = Hash.ComputeHash(sourceBytePassword);
                return BitConverter.ToString(hash_).Replace("-", String.Empty);
            }
        }
    }
}
