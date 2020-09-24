using System.Text;
using PluginInterface;
using System.IO;
using System.Security.Cryptography;

namespace RC2_Plugin
{
    [Plugin(PluginType.Cryptography)]
    public class MyPlugin : ICrypography
    {
        public string Name
        {
            get { return "RC_2 Cipher Plugin"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Author
        {
            get { return "Listratenko"; }
        }

        public string ChyperName
        {
            get { return "RC_2"; }
        }

        static string RC2Key = "111111111111111";
        static string RC2IV = "00000000";
        public byte[] Encrypt(byte[] plainTextBytes)
        {
            byte[] Key = Encoding.UTF8.GetBytes(RC2Key);
            byte[] IV = Encoding.UTF8.GetBytes(RC2IV);
            MemoryStream mStream = new MemoryStream();
            RC2 RC2alg = RC2.Create();
            CryptoStream cStream = new CryptoStream(mStream, RC2alg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cStream.FlushFinalBlock();
            byte[] cyperTextBytes = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return cyperTextBytes;
        }

        public byte[] Decrypt(byte[] Data)
        {
            byte[] Key = Encoding.UTF8.GetBytes(RC2Key);
            byte[] IV = Encoding.UTF8.GetBytes(RC2IV);
            MemoryStream mStream = new MemoryStream(Data);
            RC2 RC2alg = RC2.Create();
            CryptoStream cStream = new CryptoStream(mStream, RC2alg.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
            byte[] ret = new byte[Data.Length];
            int decryptedByteCount = cStream.Read(ret, 0, ret.Length);
            cStream.Close();
            mStream.Close();
            return ret;
        }
    }
}
