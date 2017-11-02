using System;
using System.IO;
using CommandLine;
using SortPics.Common;
using SortPics.Properties;

namespace SortPics
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // set up default filter settings
            var filterYear = 0;
            var filterMonth = 0; 
            var filterDay = 0;

            // Parse arguments using CommandLine module
            var options = new Options();
            var optionsParseSuccess = Parser.Default.ParseArguments(args, options);
            if (optionsParseSuccess)
            {
                filterYear = options.FilterYear;
                filterMonth = options.FilterMonth;
                filterDay = options.FilterDay;
            }
            else
            {
                Common.Common.Die("Invalid options specified, please see above for supported options.");
            }

            var settings = new Settings();
            var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // if picturesPath is specified as an argument, use that, if not, use the one from settings
            var fullPathToPictures = Path.Combine(profilePath, settings.sourcePath);
            var destinationBaseDir = Path.Combine(profilePath, settings.photosDestinationPath);

            // user can override source and destination directories via Options
            if (options.ImagesSourcePath != null)
                fullPathToPictures = options.ImagesSourcePath;

            if (options.ImagesDestinationPath != null)
                destinationBaseDir = options.ImagesDestinationPath;


            // check that both source and destination paths exist
            if (!Directory.Exists(fullPathToPictures))
                Common.Common.Die($"Source directory '{fullPathToPictures}' does not exist!");

            if (!Directory.Exists(destinationBaseDir))
                Common.Common.Die($"Destination directory '{destinationBaseDir}' does not exist!");

            // search for images

            Console.WriteLine($"Searching for images in {fullPathToPictures}");
            var images = Images.Images.FindImages(fullPathToPictures);

            var imagesFiltered = Images.Images.FilterImages(images, filterYear, filterMonth, filterDay);
            if (imagesFiltered.Count == 0)
            {
                Console.WriteLine("No images found matching the filter criteria");
                Environment.Exit(0);
            }
            Console.WriteLine($" {imagesFiltered.Count} images found");

            // present to the user the picture list and ask for confirmation to move
            foreach (var image in imagesFiltered)
                Images.Images.Move(image, destinationBaseDir);

            var response = UserInput.ConfirmContinue("Do you want to continue and move these images?");
            if (response)
            {
                foreach (var image in imagesFiltered)
                    Images.Images.Move(image, destinationBaseDir, false);
            }
            else
            {
                Console.WriteLine("Move aborted.");
                Environment.Exit(0);
            }
        }
    }
}