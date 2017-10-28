using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace SortPics
{
    class Program
    {
        /// <summary>
        /// Find all images in the specified directory.
        /// </summary>
        /// <param name="searchPath">Directory that contains images</param>
        /// <returns>Iterable images list</returns>
        public static List<Image> FindImages(string searchPath)
        {
            List<Image> ImagesList = new List<Image>();
            
            var files = Directory.GetFiles(searchPath, "*.jpg");

            foreach (string fileName in files)
            {
                Image image = new Image(fileName, File.GetLastWriteTime(fileName), File.GetCreationTime(fileName));
                ImagesList.Add(image);
            }

            return ImagesList;
        }
       
        static void Main(string[] args)
        {
            /*
             Todo:

            Handle arguments: year, month, day
            Allow filtering images according to arguments

             */

            // set up default filter settings
            int filterYear = 0;
            int filterMonth = 0;
            int filterDay = 0;

            // Parse arguments using CommandLine module
            var options = new Options();
            var optionsParseSuccess = CommandLine.Parser.Default.ParseArguments(args, options);
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
            
            var settings = new SortPics.Properties.Settings();

            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // if picturesPath is specified as an argument, use that, if not, use the one from settings
            string fullPathToPictures = $"{profilePath}\\{settings.picturesPath}";
            string destinationBaseDir = $"{profilePath}\\{settings.destinationPath}";

            if (options.ImagesSourcePath != null)
            {
                fullPathToPictures = options.ImagesSourcePath;
            }

            if (options.ImagesDestinationPath != null)
            {
                destinationBaseDir = options.ImagesDestinationPath;
            }
                
          
            // check that both source and destination paths exist

            if (!Directory.Exists(fullPathToPictures))
            {
                Common.Die($"Source directory '{fullPathToPictures}' does not exist!");
            }

            Console.WriteLine($"Searching for images in {fullPathToPictures}");
            var images = FindImages(fullPathToPictures);
            Console.WriteLine($"Filtering by year: {filterYear}");

            // filter images list according to specified arguments
            var imagesFiltered = images.OfType<Image>().Where(s => s.ModificationDate.Year == filterYear).ToList();
            if(imagesFiltered.Count == 0)
            {
                Console.WriteLine("No images found matching the filter criteria");
                System.Environment.Exit(0);
            }
            Console.WriteLine($" {imagesFiltered.Count} images found");
           
            foreach (var image in imagesFiltered)
            {
                MoveImage.Move(image, destinationBaseDir);
            }
            Console.WriteLine("If this looks ok, press any key to continue or Ctrl+C to abort");
            Console.ReadKey();
            foreach (var image in imagesFiltered)
            {
                MoveImage.Move(image, destinationBaseDir, false);
            }

        }
    }
}
