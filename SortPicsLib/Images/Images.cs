using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SortPicsLib.Common;

namespace SortPicsLib.Images
{
    public class Destination
    {
        public string Directory;
        public string Path;

        public Destination(MediaFile image, string destinationBaseDirPhotos, string destinationBaseDirVideos)
        { 

            if (image.IsVideo)
            {
                Directory = System.IO.Path.Combine(destinationBaseDirVideos, image.MediaFileYear, image.MediaFileMonth);
                Path = System.IO.Path.Combine(Directory, image.FileName);
            }
            else if (image.IsImage)
            {
                Directory = System.IO.Path.Combine(destinationBaseDirPhotos, image.MediaFileYear, image.MediaFileMonth);
                Path = System.IO.Path.Combine(Directory, image.FileName);
            }
        }
    }

    public class Images
    {
        /// <summary>
        ///     Return MIME type as string.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMimeType(string fileName)
        {
            var mimeType = MimeMapping.GetMimeMapping(fileName);
            return mimeType;
        }

        /// <summary>
        ///     Check if the file is an image based on its MIME type
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImage(string fileName)
        {
            var supportedMimeTypes = new List<string>();
            supportedMimeTypes.Add("image/jpeg");
            supportedMimeTypes.Add("image/png");
            supportedMimeTypes.Add("image/gif");

            var mime = GetMimeType(fileName);
            if (supportedMimeTypes.Contains(mime))
                return true;
            return false;
        }

        /// <summary>
        ///     Check if file is a movie file based on MIME type and the extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsVideo(string fileName)
        {
            /*
             For videos, it seems that many MP4's have MIME type "application/octet-stream"
             Which isn't really that helpful, so idea is to check the extension and then 
             additionally check against supported MIME types list.
             */
            // todo: use meta data to validate whether this is a movie file or not
            var supportedMimeTypes = new List<string>();
            supportedMimeTypes.Add("video/quicktime");
            supportedMimeTypes.Add("application/octet-stream");

            var supportedVideoExtensions = new List<string>();
            supportedVideoExtensions.Add(".mp4");
            supportedVideoExtensions.Add(".mov");

            var mime = GetMimeType(fileName);
            var extension = Path.GetExtension(fileName);
            // if both MIME type and extension are recognized, it's a movie!
            if (supportedMimeTypes.Contains(mime) && supportedVideoExtensions.Contains(extension))
                return true;
            return false;
        }

        public static List<MediaFile> FindImagesFiltered(RuntimeSettings runtimeSettings)
        {
            var files = FindImages(runtimeSettings.FullPathToMedia);
            var imagesFiltered = FilterImagesByDate(files, runtimeSettings.FilterYear, runtimeSettings.FilterMonth,
                runtimeSettings.FilterDay);
            return imagesFiltered;
        }

        /// <summary>
        ///     Find all images in the specified directory.
        /// </summary>
        /// <param name="searchPath">Directory that contains images</param>
        /// <returns>Iterable images list</returns>
        public static List<MediaFile> FindImages(string searchPath)
        {
            var ImagesList = new List<MediaFile>();

            var files = Directory.GetFiles(searchPath);

            foreach (var fileName in files)
                if (IsImage(fileName) || IsVideo(fileName))
                {
                    var image = new MediaFile(fileName);
                    ImagesList.Add(image);
                }
                else
                {
                    Console.WriteLine($"Ignoring {fileName}");
                }
            return ImagesList;
        }

        /// <summary>
        ///     Create specified directory if it does not exist yet,
        ///     ask user for confirmation.
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        
        /// <summary>
        ///     Move one image to the right destination path.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destinationBaseDirPhotos"></param>
        /// <param name="destinationBaseDirVideos"></param>
        /// <param name="dryRun"></param>
        public static void Move(MediaFile image, string destinationBaseDirPhotos, string destinationBaseDirVideos,
            bool dryRun = true)
        {
            // todo: refactor so that there would be no need to pass two destination arguments for each: photos and videos
            
            var destination = new Destination(image, destinationBaseDirPhotos, destinationBaseDirVideos);

            // todo: decouple directory checks from Move()
            // todo: don't interact with user from within a library
            if (!dryRun)
                CreateDirectoryIfNotExists(destination.Directory);

            // source and destination file paths prepared
            // make sure that destination file does not yet exist
            // if it does exist, check if both source and destination files are the same
            if (File.Exists(destination.Path))
                if (FileHash.FilesAreTheSame(destination.Path, image.FilePath))
                    throw new FilesAreTheSameException();
                else
                    throw new FilesAreTheSameButContentsDifferException();
            if (dryRun == false)
            {
                Console.WriteLine($"Moving: {image.FilePath} ==> {destination.Path}");
                File.Move(image.FilePath, destination.Path);
            }
            else
            {
                Console.WriteLine($"(dry run) Moving: {image.FilePath} ==> {destination.Path}");
            }
        }

        /// <summary>
        ///     Iteratively filter images based on year, month, day.
        /// </summary>
        /// <param name="images"></param>
        /// <param name="filterYear"></param>
        /// <param name="filterMonth"></param>
        /// <param name="filterDay"></param>
        /// <returns>List of filtered images</returns>
        public static List<MediaFile> FilterImagesByDate(List<MediaFile> images, int filterYear, int filterMonth,
            int filterDay)
        {
            // return all images by default
            var imagesFiltered = images;
            // do actual filtering
            if (filterYear > 0)
            {
                Console.WriteLine($"Filtering by year: {filterYear}");
                imagesFiltered = imagesFiltered.OfType<MediaFile>().Where(s => s.ModificationDate.Year == filterYear)
                    .ToList();

                // if a month has been prvided as well, narrow it down to that month
                if (filterMonth > 0)
                {
                    Console.WriteLine($"Filtering by month: {filterMonth}");
                    imagesFiltered = imagesFiltered.OfType<MediaFile>()
                        .Where(s => s.ModificationDate.Month == filterMonth)
                        .ToList();

                    // if a day has been specified as well, narrow it down to that day
                    if (filterDay > 0)
                    {
                        Console.WriteLine($"Filtering by day: {filterDay}");
                        imagesFiltered = imagesFiltered.OfType<MediaFile>()
                            .Where(s => s.ModificationDate.Day == filterDay)
                            .ToList();
                    }
                }
            }
            return imagesFiltered;
        }
    }
}