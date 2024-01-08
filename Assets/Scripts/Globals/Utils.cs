using System;
using System.Security.Cryptography;
using System.Text;

namespace LHC.Globals {

    public static class Utility
    {
        public static string CreateHash(string name)
        {
            MD5 hash = MD5.Create();
            byte[] hBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(name));
            return new Guid(hBytes).ToString();
        }
    }
}
