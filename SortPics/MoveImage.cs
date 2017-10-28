using System;
using System.IO;
using SortPics.Properties;

namespace SortPics
{
    internal class MoveImage
    {
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
                Common.Die($"Destination directory '{imageDestinationDirectory}' does not exist!");

            // source and destination file paths prepared
            // make sure that destination file does not yet exist
            if (File.Exists(imageDestinationPath))
            {
                Console.WriteLine("Interesting, destination file already exists. A bug?");
                Common.Die();
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