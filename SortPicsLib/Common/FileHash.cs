using System;
using System.IO;
using System.Security.Cryptography;

namespace SortPicsLib.Common
{
    public class FileHash
    {
        /// <summary>
        /// Returns MD5 hash of a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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