using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SortPics.Common;

namespace SortPics.Images
{
    internal class Images
    {
        /// <summary>
        ///     Check if the file is an image based on its MIME type
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImage(string fileName)
        {
            var supportedMimeTypes = new List<string>();
            supportedMimeTypes.Add("image/jpeg");
            supportedMimeTypes.Add("image/png");
            supportedMimeTypes.Add("image/gif");

            var mime = MimeMapping.GetMimeMapping(fileName);
            if (supportedMimeTypes.Contains(mime))
                return true;
            Console.WriteLine($"Unknown file {fileName} with mime type: {mime}");
            return false;
        }

        public static bool IsVideo(string fileName)
        {
            var supportedMimeTypes = new List<string>();
            supportedMimeTypes.Add("video/quicktime");

            var mime = MimeMapping.GetMimeMapping(fileName);
            if (supportedMimeTypes.Contains(mime))
                return true;
            Console.WriteLine($"Unknown file {fileName} with mime type: {mime}");
            return false;
        }

        /// <summary>
        ///     Find all images in the specified directory.
        /// </summary>
        /// <param name="searchPath">Directory that contains images</param>
        /// <returns>Iterable images list</returns>
        public static List<MediaFile> FindImages(string searchPath)
        {
            var ImagesList = new List<MediaFile>();

            var files = Directory.GetFiles(searchPath);

            foreach (var fileName in files)
                if (IsImage(fileName))
                {
                    var image = new MediaFile(fileName);
                    ImagesList.Add(image);
                }
                else
                {
                    Console.WriteLine($"Ignoring {fileName}");
                }
            return ImagesList;
        }

        /// <summary>
        ///     Move one image to the right destination path.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destinationBaseDir"></param>
        /// <param name="dryRun"></param>
        public static void Move(MediaFile image, string destinationBaseDir, bool dryRun = true)
        {
            var imageYear = image.ModificationDate.Year.ToString();
            var imageMonth = image.ModificationDate.Month.ToString().PadLeft(2, '0');

            var imageDestinationDirectory = Path.Combine(destinationBaseDir, imageYear, imageMonth);
            var imageDestinationPath = Path.Combine(imageDestinationDirectory, image.FileName);

            if (!dryRun)
                if (!Directory.Exists(imageDestinationDirectory))
                {
                    Console.WriteLine($"Destination directory {imageDestinationDirectory} doest not exist!");
                    var response = UserInput.ConfirmContinue("Do you want to create the destination directory?");
                    if (response)
                    {
                        Console.WriteLine($"Creating {imageDestinationDirectory}");
                        Directory.CreateDirectory(imageDestinationDirectory);
                    }
                    else
                    {
                        Common.Common.Die($"Destination directory '{imageDestinationDirectory}' does not exist!");
                    }
                }

            // source and destination file paths prepared
            // make sure that destination file does not yet exist
            // if it does exist, check if both source and destination files are the same
            if (File.Exists(imageDestinationPath))
            {
                Console.WriteLine("Interesting, destination file already exists. Will check the hash.");
                var destinationHash = FileHash.GetMd5Hash(imageDestinationPath);
                var sourceHash = FileHash.GetMd5Hash(image.FilePath);
                if (destinationHash == sourceHash)
                    Console.WriteLine("Source and destination files are the same!");
                else
                    Console.WriteLine("Source and destination files names are the same, but contents are different!");
                Common.Common.Die($"Error while moving {image.FileName}");
            }
            if (dryRun == false)
            {
                Console.WriteLine($"Moving: {image.FilePath} ==> {imageDestinationPath}");
                File.Move(image.FilePath, imageDestinationPath);
            }
            else
            {
                Console.WriteLine($"(dry run) Moving: {image.FilePath} ==> {imageDestinationPath}");
            }
        }

        /// <summary>
        ///     Iteratively filter images based on year, month, day.
        /// </summary>
        /// <param name="images"></param>
        /// <param name="filterYear"></param>
        /// <param name="filterMonth"></param>
        /// <param name="filterDay"></param>
        /// <returns>List of filtered images</returns>
        public static List<MediaFile> FilterImages(List<MediaFile> images, int filterYear, int filterMonth, int filterDay)
        {
            // return all images by default
            var imagesFiltered = images;
            // do actual filtering
            if (filterYear > 0)
            {
                Console.WriteLine($"Filtering by year: {filterYear}");
                imagesFiltered = images.OfType<MediaFile>().Where(s => s.ModificationDate.Year == filterYear).ToList();

                // if a month has been prvided as well, narrow it down to that month
                if (filterMonth > 0)
                {
                    Console.WriteLine($"Filtering by month: {filterMonth}");
                    imagesFiltered = images.OfType<MediaFile>().Where(s => s.ModificationDate.Month == filterMonth)
                        .ToList();

                    // if a day has been specified as well, narrow it down to that day
                    if (filterDay > 0)
                    {
                        Console.WriteLine($"Filtering by day: {filterDay}");
                        imagesFiltered = images.OfType<MediaFile>().Where(s => s.ModificationDate.Day == filterDay)
                            .ToList();
                    }
                }
            }
            return imagesFiltered;
        }
    }
}