using System;
using System.IO;
using System.Security.Cryptography;

namespace SortPicsLib.Common
{
    public class FileHash
    {
        public static string GetMd5Hash(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }
    }
}