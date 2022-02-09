using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SITConnect.Model
{
    public static class Encryption
    {
        public static string EncryptRecord(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
        public static string DecryptRecord(string password)
        {
            string msg = "";
             var   res = Convert.FromBase64String(password);
            var result = Encoding.UTF8.GetString(res);
          
            return result;
        }
        public static string EncryptData(string raw)
        {
            try
            {
                string encString = "";
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    encString= Encoding.Default.GetString(encrypted);
                    // Print encrypted string    
                    // Decrypt the bytes to a string.    
                    // Print decrypted string. It should be same as raw data

                }

                return encString;
            }
            catch (Exception exp)
            {
                return "";
            }
            Console.ReadKey();
        }
        static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        public   static string DecryptData(string raw)
        {

            try
            {
                string decrypted = "";
                //// Create Aes that generates a new key and initialization vector (IV).    
                //// Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    //    // Encrypt string
                    //    byte[] encrypted = new byte[(raw.Length + 1) / 3];
                    //    for (int i = 0; i < encrypted.Length; i++)
                    //    {
                    //        encrypted[i] = (byte)(
                    //           "0123456789ABCDEF".IndexOf(raw[i * 3]) * 16 +
                    //           "0123456789ABCDEF".IndexOf(raw[i * 3 + 1])
                    //        );
                    //    }
                    // var encrypted = BitConverter.GetBytes(raw);
                    // Print encrypted string    
                    // Decrypt the bytes to a string.    
                    decrypted = Decrypt(Encoding.Default.GetBytes(raw), aes.Key, aes.IV);
                 
                    // Print decrypted string. It should be same as raw data    
                }
                return decrypted;
            }
            catch (Exception exp)
            {
                return "";
            }
            Console.ReadKey();
        }
    public    static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
   public     static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}