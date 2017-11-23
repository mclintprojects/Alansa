using System;
using System.Security.Cryptography;
using System.Text;

namespace Alansa.Droid.Utils
{
    public class Cryptor
    {
        private readonly byte[] key;
        private readonly byte[] iv;

        /// <summary>
        /// Initializes the Cryptor
        /// </summary>
        /// <param name="keyString">A string 16 chars in length to use as the key for encryption and decryption</param>
        /// <param name="ivString">A string 16 chars in length to use as the initialization vector for encryption and decryption</param>
        public Cryptor(string keyString, string ivString)
        {
            key = Encoding.UTF8.GetBytes(keyString);
            iv = Encoding.UTF8.GetBytes(ivString);
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <returns>The encrypted form of the original string</returns>
        public string Encrypt(string text)
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
        public string Decrypt(string text)
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