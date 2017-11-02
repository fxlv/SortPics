using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SortPics.Common;

namespace SortPics.Images
{
    internal class Images
    {
        /// <summary>
        ///     Find all images in the specified directory.
        /// </summary>
        /// <param name="searchPath">Directory that contains images</param>
        /// <returns>Iterable images list</returns>
        public static List<Image> FindImages(string searchPath)
        {
            var ImagesList = new List<Image>();

            var files = Directory.GetFiles(searchPath, "*.jpg"); // todo: support more image file formats

            foreach (var fileName in files)
            {
                var image = new Image(fileName, File.GetLastWriteTime(fileName), File.GetCreationTime(fileName)); //todo: getting the date could be done in Image() constructor, not here
                ImagesList.Add(image);
            }

            return ImagesList;
        }

        /// <summary>
        ///     Move one image to the right destination path.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destinationBaseDir"></param>
        /// <param name="dryRun"></param>
        public static void Move(Image image, string destinationBaseDir, bool dryRun = true)
        {
            var imageYear = image.ModificationDate.Year;


            var imageDestinationDirectory = $"{destinationBaseDir}\\{imageYear}";
            var imageDestinationPath = $"{imageDestinationDirectory}\\{image.FileName}";
            //todo: consider using Path.Combine instead of concatenating strings

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
        /// Iteratively filter images based on year, month, day.
        /// </summary>
        /// <param name="images"></param>
        /// <param name="filterYear"></param>
        /// <param name="filterMonth"></param>
        /// <param name="filterDay"></param>
        /// <returns>List of filtered images</returns>
        public static List<Image> FilterImages(List<Image> images, int filterYear, int filterMonth, int filterDay)
        {
            List<Image> imagesFiltered;
            if (filterYear > 0)
            {
                Console.WriteLine($"Filtering by year: {filterYear}");
                imagesFiltered = images.OfType<Image>().Where(s => s.ModificationDate.Year == filterYear).ToList();

                // if a month has been prvided as well, narrow it down to that month
                if (filterMonth > 0)
                {
                    Console.WriteLine($"Filtering by month: {filterMonth}");
                    imagesFiltered = images.OfType<Image>().Where(s => s.ModificationDate.Month == filterMonth).ToList();

                    // if a day has been specified as well, narrow it down to that day
                    if (filterDay > 0)
                    {
                        Console.WriteLine($"Filtering by day: {filterDay}");
                        imagesFiltered = images.OfType<Image>().Where(s => s.ModificationDate.Day == filterDay).ToList();
                    }
                }
            }
            else
            {
                imagesFiltered = images;
            }
            return imagesFiltered;
        }
    }
}