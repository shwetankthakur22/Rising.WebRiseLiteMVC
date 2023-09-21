using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Rising.OracleDBHelper
{
    public static class EncryptDecrypt
    {
        public static string Encrypt(string str)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(str);

                string key = "123456rising123456";

                MD5CryptoServiceProvider ish5 = new MD5CryptoServiceProvider();
                keyArray = ish5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                ish5.Clear();

                TripleDESCryptoServiceProvider ddd = new TripleDESCryptoServiceProvider();
                ddd.Key = keyArray;
                ddd.Mode = CipherMode.ECB;
                ddd.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = ddd.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                ddd.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch 
            {
                return "";
            }
           
        }

        public static string Decrypt(string str)
        {
            try
            {


                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(str);

                string key = "123456rising123456";

                MD5CryptoServiceProvider ish5 = new MD5CryptoServiceProvider();
                keyArray = ish5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                ish5.Clear();

                TripleDESCryptoServiceProvider ddd = new TripleDESCryptoServiceProvider();
                ddd.Key = keyArray;
                ddd.Mode = CipherMode.ECB;
                ddd.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = ddd.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                ddd.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return "";
            }
        }
    }
}
