using System;
using System.IO;

namespace SortPics
{
    class MoveImage
    {
        /// <summary>
        /// Move one image to the right destination path.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destinationBaseDir"></param>
        /// <param name="dryRun"></param>
        public static void Move(Image image, string destinationBaseDir, Boolean dryRun = true)
        {
            var imageYear = image.ModificationDate.Year;
            var settings = new SortPics.Properties.Settings();
            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string imageDestinationDirectory = $"{destinationBaseDir}\\{imageYear}";
            string imageDestinationPath = $"{imageDestinationDirectory}\\{image.FileName}";



            if (!Directory.Exists(imageDestinationDirectory))
            {
                Common.Die($"Destination directory '{imageDestinationDirectory}' does not exist!");
            }

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
