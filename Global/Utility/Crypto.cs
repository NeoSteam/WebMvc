using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Global
{
    public static class Crypto
    {
        private static SymmetricAlgorithm mCSP;
        private static string txtKey = "Linksteicom=";
        private static string txtIV  = "Phaedruns80=";

        private static void btnKeyGen()
        {
            mCSP = SetEnc();
            byte[] byt2 = Convert.FromBase64String(txtKey);
            mCSP.Key = byt2;
        }

        private static void btnIVGen()
        {
            byte[] byt2 = Convert.FromBase64String(txtIV);
            mCSP.IV = byt2;
        }

        public static string EncryptString(string Value)
        {
            btnKeyGen();
            btnIVGen();

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string Value)
        {
            btnKeyGen();
            btnIVGen();

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            try
            {
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
                byt = Convert.FromBase64String(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
            
        }

        private static SymmetricAlgorithm SetEnc()
        {
            return new DESCryptoServiceProvider();
        }
    }
}
