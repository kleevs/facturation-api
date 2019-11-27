﻿using System.Security.Cryptography;
using System.Text;
using Db;

namespace RepositoryExample
{
    public class Hasher : IHasher
    {
        private SHA256 _SHA256;
        public Hasher()
        {
            _SHA256 = SHA256.Create();
        }

        public string Compute(string text)
        {
            return System.Convert.ToBase64String(_SHA256.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
