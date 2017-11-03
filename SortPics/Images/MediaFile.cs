using System;
using System.IO;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

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
            ModificationDate = File.GetLastWriteTime(FilePath);
            CreationDate = File.GetCreationTime(FilePath);
            MimeType = Images.GetMimeType(FilePath);
            GetExifData();
            if (Images.IsImage(FilePath))
                IsImage = true;
            if (Images.IsVideo(FilePath))
                IsVideo = true;
        }

        public string FilePath { set; get; }
        public DateTime ModificationDate { set; get; }
        public DateTime CreationDate { set; get; }
        public string FileName { set; get; }
        public string MimeType { set; get;  }
        public string CameraModel { set; get;  }
        public string CameraMake { set; get; }
        public string GpsLatitude { set; get; }
        public string GpsLongitude { set; get; }
        public bool IsVideo { set; get; }
        public bool IsImage { set; get; }


        /// <summary>
        /// Get EXIF data from the image file
        /// </summary>
        public void GetExifData()
        {
            var metaData = ImageMetadataReader.ReadMetadata(FilePath);
            var exifDirectory = metaData.OfType<ExifIfd0Directory>().FirstOrDefault();
            if(exifDirectory != null )
            {
                CameraModel =  exifDirectory.GetDescription(ExifDirectoryBase.TagModel);
                CameraMake = exifDirectory.GetDescription(ExifDirectoryBase.TagMake);
            }
            // todo: refactor GPS stuff to separate method
            var gpsDirectory = metaData.OfType<GpsDirectory>().FirstOrDefault();
            if (gpsDirectory != null)
            {
                GpsLatitude = gpsDirectory.Tags.ToArray().Where(s => s.Name == "GPS Latitude").FirstOrDefault().Description;
                GpsLongitude = gpsDirectory.Tags.ToArray().Where(s => s.Name == "GPS Longitude").FirstOrDefault().Description;
            }
        }

        public override string ToString()
        {
            var stringRepresentation = $"Media file: {FileName}";
            return stringRepresentation;
        }
    }
}