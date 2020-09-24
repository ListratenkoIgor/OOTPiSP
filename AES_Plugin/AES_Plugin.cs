using System.Text;
using System.IO;
using System.Security.Cryptography;
using PluginInterface;

    namespace AES_Plugin
    {
        [Plugin(PluginType.Cryptography)]
        public class MyPlugin : ICrypography
        {
            public string Name
            {
                get { return "AES Cipher Plugin"; }
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
                get { return ""; }
            }

            const string initVector = "@1B2c3D4e5F6g7H8";
            const string passPhrase = "p235pr1se";
            const string saltValue = "a@1bcdrue";
            const string hashAlgorithm = "SHA1";
            const int passwordIterations = 2;
            const int keySize = 256;
            private static string randomKeyText = "챼\u07bbﰾṦ챻챰籶❠⨘뜱湋驴礉";


            static byte[] GetBytes(string randomKeyText)
            {
                byte[] bytes = new byte[randomKeyText.Length * sizeof(char)];
                System.Buffer.BlockCopy(randomKeyText.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }

            public byte[] Encrypt(byte[] plainTextBytes)
            {
                byte[] chipherTextBytes = null;
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);

                byte[] keyBytes = GetBytes(randomKeyText);//
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(
                                                        memoryStream,
                                                        encryptor,
                                                        CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();

                chipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();
                return chipherTextBytes;
            }

            public byte[] Decrypt(byte[] chipherTextBytes)
            {
                byte[] plainTextBytes = null;
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                byte[] keyBytes = GetBytes(randomKeyText);
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(chipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read);
                plainTextBytes = new byte[chipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(
                                                        plainTextBytes,
                                                        0,
                                                        plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return plainTextBytes;
            }
        }
    }

