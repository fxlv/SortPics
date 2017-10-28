using System;
using System.IO;

namespace SortPics
{
    /// <summary>
    /// Image object
    /// </summary>
    class Image
    {

        public string FilePath { set; get; }
        public DateTime ModificationDate { set; get; }
        public DateTime CreationDate { set; get; }
        public string FileName { set; get; }

        public Image(string path, DateTime modificationDate, DateTime creationDate)
        {
            FilePath = path;
            FileName = Path.GetFileName(FilePath);
            ModificationDate = modificationDate;
            CreationDate = creationDate;
        }

        public override string ToString()
        {
            var stringRepresentation = $"Image: {FileName}";
            return stringRepresentation;
        }
    }
}
