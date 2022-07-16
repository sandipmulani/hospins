using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace hospins.Repository.Infrastructure
{
    public static class SecurityLibrary
    {
        public static string GetEncryptedString(string strInput)
        {
            SHA1CryptoServiceProvider Sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] data = Sha1Hasher.ComputeHash(Encoding.Default.GetBytes(strInput));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private const int INDEX_START = 0;

        public static string Decrypt(string encryptedText, string key)
        {
            var decryptedText = string.Empty;
            encryptedText=encryptedText.Replace(" ","+");
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                var decryptor = CreateCryptoByType(CryptoType.Decrypt, key);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, INDEX_START, plainTextBytes.Length);
                decryptedText = Encoding.UTF8.GetString(plainTextBytes, INDEX_START, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            return decryptedText;
        }

        private static ICryptoTransform CreateCryptoByType(CryptoType cryptoType, string key)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            var rijndaelAlgorithm = new RijndaelManaged() { Mode = CipherMode.ECB };
            if (cryptoType.Equals(CryptoType.Encrypt))
            {
                rijndaelAlgorithm.Padding = PaddingMode.Zeros;
                var encryptor = rijndaelAlgorithm.CreateEncryptor(keyBytes, null);
                return encryptor;
            }
            else
            {
                rijndaelAlgorithm.Padding = PaddingMode.None;
                var decryptor = rijndaelAlgorithm.CreateDecryptor(keyBytes, null);
                return decryptor;
            }
        }

        private enum CryptoType
        {
            Encrypt = 1,
            Decrypt = 2,
        }
    }
}
