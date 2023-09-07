
using System.Security.Cryptography;
using System.Text;

namespace TelegramBotApi

{
    public static class Decrypter
    {
        public static string password = "";

        private static readonly string SecretKey = Environment.GetEnvironmentVariable("MY_SECRET_KEY");

        public static (string passwordSalt, string passwordEncrypted) CreatePasswordHash(string password)
        {
            // generate random salt 
            byte[] buffer = new byte[16];
            RNGCryptoServiceProvider secureRandom = new RNGCryptoServiceProvider();
            secureRandom.GetBytes(buffer);

            // create encrypted password
            string passwordSalt = Convert.ToBase64String(buffer);
            string passwordEncrypted = Encrypt(password, passwordSalt);

            // done
            return (passwordSalt, passwordEncrypted);
        }

        public static string DecryptPassword(string passwordEncrypted, string passwordSalt)
        {
            return Decrypt(passwordEncrypted, passwordSalt);
        }

        private static string Encrypt(string plainText, string passwordSalt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Derive key and IV from the password and salt
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(SecretKey, Encoding.UTF8.GetBytes(passwordSalt));

                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        private static string Decrypt(string cipherText, string passwordSalt)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                // Derive key and IV from the password and salt
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(SecretKey, Encoding.UTF8.GetBytes(passwordSalt));

                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


        public static string DecryptValue(string value)
        {
            if (value != ""&&value!=null)
            {
                string[] values = value.Split('~');
                password = Decrypt(values[0], values[1]);
                return password;
            }
            return password;
        }

    }

    

       
   
}
