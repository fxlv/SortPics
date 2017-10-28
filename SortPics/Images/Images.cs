using System;
using System.Collections.Generic;
using System.IO;
using SortPics.Properties;

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

            var files = Directory.GetFiles(searchPath, "*.jpg");

            foreach (var fileName in files)
            {
                var image = new Image(fileName, File.GetLastWriteTime(fileName), File.GetCreationTime(fileName));
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
            var settings = new Settings();
            var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var imageDestinationDirectory = $"{destinationBaseDir}\\{imageYear}";
            var imageDestinationPath = $"{imageDestinationDirectory}\\{image.FileName}";


            if (!Directory.Exists(imageDestinationDirectory))
                Common.Common.Die($"Destination directory '{imageDestinationDirectory}' does not exist!");

            // source and destination file paths prepared
            // make sure that destination file does not yet exist
            if (File.Exists(imageDestinationPath))
            {
                Console.WriteLine("Interesting, destination file already exists. A bug?");
                Common.Common.Die();
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
    }
}