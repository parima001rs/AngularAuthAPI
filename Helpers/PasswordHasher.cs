/*
using System.Security.Cryptography;

namespace UserAuth.Helpers
{
    public class PasswordHasher
    {
        private static RSACryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 1000;
        
        public static string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[SaltSize + HashSize]);
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(HashSize, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes,0,salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
        }
    }
}
*/

using System.Security.Cryptography;
using System.Text;

namespace UserAuth.Helpers
{
    public class PasswordHasher
    {
        //Encrypt
        public static string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();

            var passwordBytes = Encoding.Default.GetBytes(password);

            var hashedpassword = hash.ComputeHash(passwordBytes);

            var encryptedPass = Convert.ToHexString(hashedpassword);

            return encryptedPass;
        }

        //Decrypt
        public static bool VerifyPassword(string password, string encryptedPass)
        {
            var hashedInputPass = HashPassword(password);
            return hashedInputPass.Equals(encryptedPass, StringComparison.OrdinalIgnoreCase);

        }
        
    }
}