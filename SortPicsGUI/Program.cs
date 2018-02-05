using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommandLine;
using SortPics.Properties;
using SortPicsLib.Common;
using SortPicsLib.Images;

namespace SortPicsGUI
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new SortPics());
        }

        public static List<MediaFile> FindPics()
        {
            string[] args = {"something", "cool"};
            var options = new Options();
            var optionsParseSuccess = Parser.Default.ParseArguments(args, options);
            var settings = new Settings();

            var runtimeSettings = new RuntimeSettings(settings.sourcePath, settings.photosDestinationPath,
                settings.videosDestinationPath);

            runtimeSettings.Activate(options);
            var imagesFiltered = Images.FindImagesFiltered(runtimeSettings);
            return imagesFiltered;
        }


        public static void MovePics()
        {
            string[] args = {"something", "cool"};
            var options = new Options();
            var optionsParseSuccess = Parser.Default.ParseArguments(args, options);
            var settings = new Settings();

            var runtimeSettings = new RuntimeSettings(settings.sourcePath, settings.photosDestinationPath,
                settings.videosDestinationPath);

            runtimeSettings.Activate(options);
            var imagesFiltered = Images.FindImagesFiltered(runtimeSettings);
            foreach (var image in imagesFiltered)
                Images.Move(image, runtimeSettings.DestinationBaseDirPhotos,
                    runtimeSettings.DestinationBaseDirVideos, false);
        }
    }
}