using FacturationApi.Spi;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Web.Tools
{
    public class Encryptor : IEncryptor
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public Encryptor(string key, string iv)
        {
            _key = Encoding.Unicode.GetBytes(key);
            _iv = Encoding.Unicode.GetBytes(iv);
        }

        public T Decrypt<T>(string cryptedstring)
        {
            var decryptor = Aes.Create().CreateDecryptor(_key, _iv);
            var bytes = Convert.FromBase64String(cryptedstring.Replace('_', '/'));
            var decryptedByte = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            var json = Encoding.Unicode.GetString(decryptedByte);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public string Encrypt<T>(T obj)
        {
            var encryptor = Aes.Create().CreateEncryptor(_key, _iv);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var bytes = Encoding.Unicode.GetBytes(json);
            var cryptedByte = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            var cryptedString = Convert.ToBase64String(cryptedByte).Replace('/', '_');
            return cryptedString;
        }
    }
}
