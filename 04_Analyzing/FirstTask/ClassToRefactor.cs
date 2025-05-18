using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask
{
    public class ClassToRefactor
    {
        private const int Iterate = 10000;
        private const int HashLength = 20;
        private const int SaltLength = 16;
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, Iterate, HashAlgorithmName.SHA256);

            var hash = pbkdf2.GetBytes(HashLength);
            var hashBytes = new byte[SaltLength + HashLength];

            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
