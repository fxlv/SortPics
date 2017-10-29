using System;
using System.IO;
using System.Linq;
using CommandLine;
using SortPics.Common;
using SortPics.Images;
using SortPics.Properties;

namespace SortPics
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // set up default filter settings
            var filterYear = 0;
            var filterMonth = 0; //todo: handle filterMonth
            var filterDay = 0; //todo: handle filterDay

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
            var fullPathToPictures = $"{profilePath}\\{settings.picturesPath}";
            var destinationBaseDir = $"{profilePath}\\{settings.destinationPath}";

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
            Console.WriteLine($"Filtering by year: {filterYear}");

            // filter images list according to specified arguments
            var imagesFiltered = images.OfType<Image>().Where(s => s.ModificationDate.Year == filterYear).ToList();
            if (imagesFiltered.Count == 0)
            {
                Console.WriteLine("No images found matching the filter criteria");
                Environment.Exit(0);
            }
            Console.WriteLine($" {imagesFiltered.Count} images found");

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