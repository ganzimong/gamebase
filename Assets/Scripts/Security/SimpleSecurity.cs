using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SimpleSecurity
{
    public static string GetSHA1HashString(string value)
    {
        string hash = string.Empty;

        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Value is null or empty");
            return hash;
        }
        
        byte[] bytes = Encoding.UTF8.GetBytes(value);

        if (bytes != null)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var hashBytes = sha1.ComputeHash(bytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        return hash;
    }

    public static string GetAESEncryptedString(string iv, string key, string value)
    {
        string encrypted = string.Empty;

        using (var aes = new AesManaged())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Padding = PaddingMode.PKCS7;

            var inputBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = aes.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            encrypted = Convert.ToBase64String(encryptedBytes).Replace('+', '-').Replace('/', '_').ToLower();
        }

        return encrypted;
    }

    public static string GetHMACMD5HashString(string key, string value)
    {
        string hash = string.Empty;

        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Key is null or empty");
            return hash;
        }

        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Value is null or empty");
            return hash;
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] valueBytes = Encoding.UTF8.GetBytes(value);

        using (var md5 = new HMACMD5())
        {
            var hashBytes = md5.ComputeHash(valueBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        return hash;
    }
}
