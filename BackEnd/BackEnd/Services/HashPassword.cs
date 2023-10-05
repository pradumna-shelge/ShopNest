using System;
using System.Security.Cryptography;

namespace BackEnd.Services
{
    public class HashPasswordClass
    {
        private const int SaltSize = 16; // 16 bytes for salt
        private const int HashSize = 32; // 32 bytes for hash
        private const int IterationCount = 10000;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Derive the hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, IterationCount, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(HashSize);

                // Combine salt and hash into a single byte array
                byte[] combinedBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, combinedBytes, 0, SaltSize);
                Array.Copy(hashBytes, 0, combinedBytes, SaltSize, HashSize);

                // Convert to Base64 and return as a string
                string hashedPassword = Convert.ToBase64String(combinedBytes);
                return hashedPassword;
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            byte[] storedHashBytes = Convert.FromBase64String(storedHashedPassword);

            // Extract the salt from the stored hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(storedHashBytes, 0, salt, 0, SaltSize);

            // Compute the hash of the entered password using the same salt and parameters
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, IterationCount, HashAlgorithmName.SHA256))
            {
                byte[] computedHashBytes = pbkdf2.GetBytes(HashSize);

                // Compare the computed hash with the stored hash
                for (int i = 0; i < HashSize; i++)
                {
                    if (computedHashBytes[i] != storedHashBytes[i + SaltSize])
                    {
                        return false; // Passwords don't match
                    }
                }

                return true; // Passwords match
            }
        }
    }




}
