using System;
using System.IO;

namespace SortPics.Images
{
    /// <summary>
    ///     Image object
    /// </summary>
    internal class Image
    {
        public Image(string path)
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
            var stringRepresentation = $"Image: {FileName}";
            return stringRepresentation;
        }
    }
}