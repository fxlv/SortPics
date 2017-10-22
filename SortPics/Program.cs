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

    class Options
    {
        [Option('y', "year", Required = true, HelpText = "Filter year")]
        public int FilterYear { get; set; }

        [Option('m', "month", Required = false, DefaultValue = null, HelpText = "Filter month")]
        public int FilterMonth { get; set; }

        [Option('d', "day", Required = false, DefaultValue = null, HelpText = "Filter day")]
        public int FilterDay { get; set; }

        [Option('s', "source", Required = false, DefaultValue = null,  HelpText = "Images source path")]
        public string ImagesSourcePath { get; set; }

        /// <summary>
        /// Construct Usage message.
        /// </summary>
        /// <returns></returns>
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("SortPics", "0.01"),
                Copyright = new CopyrightInfo("kaspars@fx.lv", 2017),
                //AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPostOptionsLine("Usage: .\\SortPics -y 2017\n\n");
            help.AddOptions(this);
            return help;
        }
        /// <summary>
        /// Print usage message and exit.
        /// </summary>
        /// <param name="msg"></param>
        public void PrintUsage(string msg = null)
        {
            if (msg != null)
            {
                Console.WriteLine(msg);
                Console.WriteLine();
            }
            Console.Write(GetUsage());

            System.Environment.Exit(0);


        }
    }
    /// <summary>
    /// Image object
    /// </summary>
    class Image
    {

        public string FilePath { set; get; }
        public DateTime ModificationDate { set; get; }
        public DateTime CreationDate { set; get; }
        public string FileName { set; get;  }

        public Image(string path, DateTime modificationDate, DateTime creationDate) {
            FilePath = path;
            FileName = Path.GetFileName(FilePath);
            ModificationDate = modificationDate;
            CreationDate = creationDate;
        } 

        public override string ToString()
        {
            string stringRepresentation = $"Image: {FileName}";
            return stringRepresentation;
        }
    }
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
       
        
        /// <summary>
        /// Exit and optionally print an error message.
        /// </summary>
        /// <param name="msg"></param>
        static void Die(string msg = null)
        {
            if (msg != null)
            {
                Console.WriteLine($"ERROR: {msg}");
            }
            System.Environment.Exit(1);
        }
        /// <summary>
        /// Move one image to the right destination path.
        /// </summary>
        /// <param name="image"></param>
        static void MoveImage(Image image, Boolean dryRun = true)
        {
            var imageYear = image.ModificationDate.Year; 
            var settings = new SortPics.Properties.Settings();
            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string imageDestinationDirectory = $"{profilePath}\\{settings.destinationPath}\\{imageYear}";
            string imageDestinationPath = $"{imageDestinationDirectory}\\{image.FileName}";

            
            if (!Directory.Exists(imageDestinationDirectory))
            {
                Die($"Destination directory '{imageDestinationDirectory}' does not exist!");
            }

            // source and destination file paths prepared
            // make sure that destination file does not yet exist
            if(File.Exists(imageDestinationPath))
            {
                Console.WriteLine("Interesting, destination file already exists. A bug?");
                Die();
            }
            if (dryRun == false)
            {
                Console.WriteLine($"Moving: {image.FilePath} ==> {imageDestinationPath}");
                File.Move(image.FilePath, imageDestinationPath);
            } else
            {
                Console.WriteLine($"(dry run) Moving: {image.FilePath} ==> {imageDestinationPath}");
            }
        }

        static void Main(string[] args)
        {


            /*
             Todo:

            Handle arguments: year, month, day

            Print Usage message when needed.

            Allow filtering images according to arguments

            Move images from one directory to another

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
                Die("Invalid options specified, please see above for supported options.");
            }
            
            var settings = new SortPics.Properties.Settings();

            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string fullPathToPictures = $"{profilePath}\\{settings.picturesPath}";
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
                MoveImage(image);
            }
            Console.WriteLine("If this looks ok, press any key to continue or Ctrl+C to abort");
            Console.ReadKey();
            foreach (var image in imagesFiltered)
            {
                MoveImage(image, false);
            }

        }
    }
}
