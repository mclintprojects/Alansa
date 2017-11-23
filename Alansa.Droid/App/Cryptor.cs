using System;
using System.Security.Cryptography;
using System.Text;

namespace GhalaniDroid.Utils
{
    public class Cryptor
    {
        private static byte[] key = Encoding.UTF8.GetBytes(AppResources.Key);
        private static byte[] iv = Encoding.UTF8.GetBytes(AppResources.IV);

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