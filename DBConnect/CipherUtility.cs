using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using pgmpm.Database.Properties;

namespace pgmpm.Database
{
    /// <summary>
    /// Cipher Utility Based on  http://www.superstarcoders.com/blogs/posts/symmetric-encryption-in-c-sharp.aspx
    /// Modified for use in MPM
    /// </summary>
    /// <author>Bernd Nottbeck</author>
    public class CipherUtility
    {
        /// <summary>
        /// Creates a random string using System.Guid.NewGuid()
        /// </summary>
        /// <returns>Returns a random string.</returns>
        /// <author>Bernd Nottbeck</author>
        public static String CreateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        #region Encrypt
        /// <summary>
        /// Encryption Utility generates a new AESKey if non is set. Then calls on Encrypt<T>(string value, string password, string salt) to encrypt a String using AesManaged Class.
        /// </summary>
        /// <param name="value">Text to encrypt</param>
        /// <param name="salt">Salt</param>
        /// <returns>An encrypted string.</returns>
        /// <author>Bernd Nottbeck</author>
        public static string Encrypt(string value, string salt)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new Exception("Encryption error: Empty encryption string");
            if (string.IsNullOrWhiteSpace(salt)) throw new Exception("Encryption error: Empty salt");
            if (string.IsNullOrEmpty(DBSettings.Default.AESkey))
            {
                String myGuid = CreateSalt();
                DBSettings.Default.AESkey = myGuid;
                DBSettings.Default.Save();

            }
            return Encrypt<AesManaged>(value, DBSettings.Default.AESkey, salt);
        }

        /// <summary>
        /// Encrypts a given String using a SymetricAlgorithm and generates a secure encryption key using Rfc2898DeriveBytes.
        /// </summary>
        /// <typeparam name="T">SymetricAlgorithm</typeparam>
        /// <param name="value">Text to encrypt</param>
        /// <param name="password">Password to generate Key</param>
        /// <param name="salt">Salt</param>
        /// <returns>An encrypted string.</returns>
        public static string Encrypt<T>(string value, string password, string salt)
             where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Decrypts a string using AesManaged and using Properties.Settings.Default.AESkey.
        /// </summary>
        /// <param name="text">Encrypted text</param>
        /// <param name="salt">IV Salt</param>
        /// <returns>Decrypted string</returns>
        /// <author>Bernd Nottbeck</author>
        public static string Decrypt(string text, string salt)
        {
            if (String.IsNullOrEmpty(DBSettings.Default.AESkey)) throw new Exception("Password Error: Please re-enter Connection Passwords.");
            return Decrypt<AesManaged>(text, DBSettings.Default.AESkey, salt);
        }

        /// <summary>
        /// Decrypts a string.
        /// </summary>
        /// <typeparam name="T">SymmetricAlgorithm</typeparam>
        /// <param name="text">Encrypted text</param>
        /// <param name="password">Password to generate Key</param>
        /// <param name="salt">Salt</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt<T>(string text, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            try
            {
                DeriveBytes rgb;

                rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));


                SymmetricAlgorithm algorithm = new T();

                byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

                ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);


                using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
                {
                    using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("The password cannot be decrypted.");
            }
        }
        #endregion

        #region Testhelper
        /// <summary>
        /// Resets the Properties.DBSettings.Default.AESkey.
        /// This Method is a test helper and should not be used in any other context.
        /// </summary>
        /// <returns></returns>
        public static String TemporaryAESKeyDelete()
        {
            String temp = DBSettings.Default.AESkey;
            DBSettings.Default.AESkey = "";
            return temp;
        }

        /// <summary>
        /// Resets the Properties.DBSettings.Default.AESkey to a specified value.
        /// This Method is a test helper and should not be used in any other context.
        /// </summary>
        /// <param name="aesKey">AesKey Value</param>
        /// <returns>true if the AesKey was reset.</returns>
        public static Boolean TemporaryAESKeyRestore(String aesKey)
        {
            DBSettings.Default.AESkey = aesKey;
            DBSettings.Default.Save();
            if (aesKey == DBSettings.Default.AESkey)
            { return true; }
             return false; 
        }
        #endregion
    }
}
