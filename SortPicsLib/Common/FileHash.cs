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
        /// <summary>
        /// Check if both files are the same based on their MD5 hashes.
        /// </summary>
        /// <param name="fileName1"></param>
        /// <param name="fileName2"></param>
        /// <returns></returns>
        public static bool FilesAreTheSame(string fileName1, string fileName2)
        {
            var hash1 = GetMd5Hash(fileName1);
            var hash2 = GetMd5Hash(fileName2);
            return hash1 == hash2;
        }
    }
}