using System;
using System.Security.Cryptography;
using System.Text;

namespace Alansa.Droid.Utils
{
    public static class Cryptor
    {
        private static readonly byte[] key = Encoding.UTF8.GetBytes("qQTb5FxeYKpdADTn");
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("mf5kxtDH2XqbE2dA");

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <returns>The encrypted form of the original string</returns>
        public static string Encrypt(string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            AesCryptoServiceProvider encryptor = new AesCryptoServiceProvider()
            {
                Key = key,
                IV = iv,
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform transformer = encryptor.CreateEncryptor(key, iv);
            byte[] encryptedString = transformer.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return Convert.ToBase64String(encryptedString);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="text">The encrypted string to decrypt.</param>
        /// <returns>The decrypted form of the encrypted string</returns>
        public static string Decrypt(string text)
        {
            var textBytes = Convert.FromBase64String(text);
            AesCryptoServiceProvider decryptor = new AesCryptoServiceProvider()
            {
                Key = key,
                IV = iv,
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform transformer = decryptor.CreateDecryptor(key, iv);
            byte[] encryptedString = transformer.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return Encoding.UTF8.GetString(encryptedString);
        }
    }
}