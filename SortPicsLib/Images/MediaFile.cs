﻿using System;
using System.IO;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace SortPicsLib.Images
{
    /// <summary>
    ///     Media file
    /// </summary>
    public class MediaFile
    {
        public MediaFile(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(FilePath);
            ModificationDate = File.GetLastWriteTime(FilePath);
            CreationDate = File.GetCreationTime(FilePath);
            MimeType = Images.GetMimeType(FilePath);
            if (Images.IsImage(FilePath))
                IsImage = true;
            else if (Images.IsVideo(FilePath))
                IsVideo = true;
            else
                throw new UnsupportedFileTypeException("Unsupported file type.");
            GetExifData();
            MediaFileYear = ModificationDate.Year.ToString();
            MediaFileMonth = ModificationDate.Month.ToString().PadLeft(2, '0');
        }

        public string MediaFileMonth { get; set; }
        public string MediaFileYear { get; set; }
        public string FilePath { set; get; }
        public DateTime ModificationDate { set; get; }
        public DateTime CreationDate { set; get; }
        public string FileName { set; get; }
        public string MimeType { set; get; }
        public string CameraModel { set; get; }
        public string CameraMake { set; get; }
        public string GpsLatitude { set; get; }
        public string GpsLongitude { set; get; }
        public bool IsVideo { set; get; }
        public bool IsImage { set; get; }


        /// <summary>
        ///     Get EXIF data from the image file
        /// </summary>
        public void GetExifData()
        {
            var metaData = ImageMetadataReader.ReadMetadata(FilePath);
            var exifDirectory = metaData.OfType<ExifIfd0Directory>().FirstOrDefault();
            if (exifDirectory != null)
            {
                CameraModel = exifDirectory.GetDescription(ExifDirectoryBase.TagModel);
                CameraMake = exifDirectory.GetDescription(ExifDirectoryBase.TagMake);
            }
            // todo: refactor GPS stuff to separate method
            var gpsDirectory = metaData.OfType<GpsDirectory>().FirstOrDefault();
            if (gpsDirectory != null)
            {
                GpsLatitude = gpsDirectory.Tags.ToArray().Where(s => s.Name == "GPS Latitude").FirstOrDefault()
                    .Description;
                GpsLongitude = gpsDirectory.Tags.ToArray().Where(s => s.Name == "GPS Longitude").FirstOrDefault()
                    .Description;
            }
        }

        public override string ToString()
        {
            var stringRepresentation = $"Media file: {FileName}";
            return stringRepresentation;
        }
    }
}