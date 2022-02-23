using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ButlerLee.API.Security
{
    public static class Cryptography
    {
        #region Settings

        public static string PrefixKey = "-AP-";

        #endregion

        public static string Encrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("The text must have valid value.", nameof(text));

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            SHA512CryptoServiceProvider hash = new SHA512CryptoServiceProvider();
            byte[] aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream resultStream = new MemoryStream())
                {
                    using (CryptoStream aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (MemoryStream plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    byte[] result = resultStream.ToArray();
                    byte[] combined = new byte[aes.IV.Length + result.Length];
                    Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string Decrypt(string encryptedText, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentException("The encrypted text must have valid value.", nameof(encryptedText));

            byte[] combined = Convert.FromBase64String(encryptedText);
            byte[] buffer = new byte[combined.Length];
            SHA512CryptoServiceProvider hash = new SHA512CryptoServiceProvider();
            byte[] aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (Aes aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                byte[] iv = new byte[aes.IV.Length];
                byte[] ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (MemoryStream resultStream = new MemoryStream())
                {
                    using (CryptoStream aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                    using (MemoryStream plainStream = new MemoryStream(ciphertext))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
        }
    }
}
