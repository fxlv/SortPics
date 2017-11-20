﻿using System;
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

            // call Runtime Settings here
            var settings = new Settings();

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
            var files = Images.FindImages(runtimeSettings.FullPathToMedia);

            var imagesFiltered = Images.FilterImages(files, filterYear, filterMonth, filterDay);
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
    }
}