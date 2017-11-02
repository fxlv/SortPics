using System;
using System.IO;

namespace SortPics.Images
{
    /// <summary>
    ///     Media file
    /// </summary>
    internal class MediaFile
    {
        public MediaFile(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(FilePath);
            ModificationDate=  File.GetLastWriteTime(FilePath);
            CreationDate = File.GetCreationTime(FilePath);
        }

        public string FilePath { set; get; }
        public DateTime ModificationDate { set; get; }
        public DateTime CreationDate { set; get; }
        public string FileName { set; get; }

        public override string ToString()
        {
            var stringRepresentation = $"Media file: {FileName}";
            return stringRepresentation;
        }
    }
}