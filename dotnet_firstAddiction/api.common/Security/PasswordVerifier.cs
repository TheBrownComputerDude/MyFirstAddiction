using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace api.common.Security
{
    public class PasswordVerifier : IPasswordVerifier
    {
        public PasswordResponse HashPassword(string password)
        {
            var salt = this.GenerateSalt();
            return new PasswordResponse()
                {
                    Hash = this.GenerateHash(password, salt),
                    Salt = Convert.ToBase64String(salt)
                };
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string GenerateHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

        }
    }
}