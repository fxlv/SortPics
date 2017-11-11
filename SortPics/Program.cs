using System;
using System.IO;
using CommandLine;
using SortPics.Properties;
using SortPicsLib.Common;
using SortPicsLib.Images;

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
                Common.Die("Invalid options specified, please see above for supported options.");
            }

            var settings = new Settings();
            var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // if picturesPath is specified as an argument, use that, if not, use the one from settings
            var fullPathToMedia = Path.Combine(profilePath, settings.sourcePath);
            var destinationBaseDirPhotos = Path.Combine(profilePath, settings.photosDestinationPath);
            var destinationBaseDirVideos = Path.Combine(profilePath, settings.videosDestinationPath);

            // user can override source and destination directories via Options
            if (options.ImagesSourcePath != null)
                fullPathToMedia = options.ImagesSourcePath;

            if (options.ImagesDestinationPath != null)
            {
                destinationBaseDirPhotos = options.ImagesDestinationPath;
                destinationBaseDirVideos = options.ImagesDestinationPath;
            }


            // check that both source and destination paths exist
            if (!Directory.Exists(fullPathToMedia))
                Common.Die($"Source directory '{fullPathToMedia}' does not exist!");

            if (!Directory.Exists(destinationBaseDirPhotos))
                Common.Die($"Destination directory '{destinationBaseDirPhotos}' does not exist!");
            //todo: refactor source/destination dir checking into separate method
            if (!Directory.Exists(destinationBaseDirVideos))
                Common.Die($"Destination directory '{destinationBaseDirVideos}' does not exist!");

            // search for images and videos

            Console.WriteLine($"Searching in {fullPathToMedia}");
            var files = Images.FindImages(fullPathToMedia);

            var imagesFiltered = Images.FilterImages(files, filterYear, filterMonth, filterDay);
            if (imagesFiltered.Count == 0)
            {
                Console.WriteLine("No images found matching the filter criteria");
                Environment.Exit(0);
            }
            Console.WriteLine($" {imagesFiltered.Count} images found");

            // present to the user the picture list and ask for confirmation to move
            foreach (var image in imagesFiltered)
                Images.Move(image, destinationBaseDirPhotos, destinationBaseDirVideos);

            var response = UserInput.ConfirmContinue("Do you want to continue and move these images?");
            if (response)
            {
                foreach (var image in imagesFiltered)
                    Images.Move(image, destinationBaseDirPhotos, destinationBaseDirVideos, false);
            }
            else
            {
                Console.WriteLine("Move aborted.");
                Environment.Exit(0);
            }
        }
    }
}