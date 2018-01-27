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
            // Parse arguments using CommandLine module
            var options = new Options();
            var optionsParseSuccess = Parser.Default.ParseArguments(args, options);
            if (!optionsParseSuccess)
                Common.Die("Invalid options specified, please see above for supported options.");

            // read settings
            var settings = new Settings();
            // handle "FirstRun" case where the user is running the app for the first time or the settings have been wiped
            FirstRun(settings);
            // create run time settings
            var runtimeSettings = new RuntimeSettings(settings.sourcePath, settings.photosDestinationPath,
                settings.videosDestinationPath);
            try
            {
                runtimeSettings.Activate(options);
            }
            catch (DirectoryNotFoundException e)
            {
                Common.Die(e.Message);
            }

            // search for images and videos
            Console.WriteLine($"Searching in {runtimeSettings.FullPathToMedia}");
            var imagesFiltered = Images.FindImagesFiltered(runtimeSettings);
            if (imagesFiltered.Count == 0)
            {
                Console.WriteLine("No images found matching the filter criteria");
                Environment.Exit(0);
            }
            Console.WriteLine($" {imagesFiltered.Count} images found");

            // present to the user the picture list and ask for confirmation to move
            foreach (var image in imagesFiltered)
                Images.Move(image, runtimeSettings.DestinationBaseDirPhotos, runtimeSettings.DestinationBaseDirVideos);

            var response = UserInput.ConfirmContinue("Do you want to continue and move these images?");
            if (response)
            {
                foreach (var image in imagesFiltered)
                    Images.Move(image, runtimeSettings.DestinationBaseDirPhotos, runtimeSettings.DestinationBaseDirVideos, false);
            }
            else
            {
                Console.WriteLine("Move aborted.");
                Environment.Exit(0);
            }
        }

        private static void FirstRun(Settings settings)
        {
            string line = "---------------------------------------";
            if (!settings.settingsSaved)
            {
                Console.WriteLine();
                Console.WriteLine(line);
                Console.WriteLine("First run? Let's set up youf Pictures folders");
                Console.WriteLine("SortPics needs to know the source directory and destination directory");
                Console.WriteLine(line);
                Console.WriteLine("Currently (by default) defined paths look like so:");
                Console.WriteLine($"Source path: '{settings.sourcePath}'");
                Console.WriteLine($"Destination path for pictures: '{settings.photosDestinationPath}'");
                Console.WriteLine($"Destination path for videos: '{settings.videosDestinationPath}'");
                var response = UserInput.ConfirmContinue("Do you want to change these paths?");
                if (response)
                {
                    // todo: implememt interactive settings update
                    settings.settingsSaved = true;
                    settings.Save();
                    Console.WriteLine("Updating not implemented yet.");
                    Console.WriteLine(@"Please go to 'c:\Users\fx\AppData\Local\FX\' and edit the settings file manually.");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Allrighty then. Saving settings.");
                    settings.settingsSaved = true;
                    settings.Save();
                }
            }
        }
    }
}