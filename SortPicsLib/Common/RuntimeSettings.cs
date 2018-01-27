using System;
using System.Collections.Generic;
using System.IO;

namespace SortPicsLib.Common
{
    /// <summary>
    ///     Handle run time settings.
    /// </summary>
    public class RuntimeSettings
    {
        public string DestinationBaseDirPhotos;
        public string DestinationBaseDirVideos;
        public int FilterDay;
        public int FilterMonth;

        public int FilterYear;
        public string FullPathToMedia;

        /// <summary>
        ///     Take in source and destination paths for media as well as CLI options
        ///     and marshal them into usable run time settings.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="photosDestinationPath"></param>
        /// <param name="videosDestinationPath"></param>
        /// <param name="options"></param>
        public RuntimeSettings(string sourcePath, string photosDestinationPath, string videosDestinationPath)
        {
            var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // if picturesPath is specified as an argument, use that, if not, use the one from settings
            FullPathToMedia = Path.Combine(profilePath, sourcePath);
            DestinationBaseDirPhotos = Path.Combine(profilePath, photosDestinationPath);
            DestinationBaseDirVideos = Path.Combine(profilePath, videosDestinationPath);
        }

        /// <summary>
        ///     Do active substitutions and checks for the settings.
        /// </summary>
        /// <param name="options"></param>
        public void Activate(Options options)
        {
            SubstituteUserOptions(options);
            SubstituteFilters(options);
            CheckThatPathsExist();
        }

        /// <summary>
        ///     check that both source and destination paths exist
        /// </summary>
        private void CheckThatPathsExist()
        {
            var paths = new List<string>();
            paths.Add(FullPathToMedia);
            paths.Add(DestinationBaseDirPhotos);
            paths.Add(DestinationBaseDirVideos);

            foreach (var path in paths)
                if (!Directory.Exists(path))
                    throw new DirectoryNotFoundException($"Directory {path} not found.");
        }

        private void SubstituteFilters(Options options)
        {
            FilterYear = options.FilterYear;
            FilterMonth = options.FilterMonth;
            FilterDay = options.FilterDay;
        }

        private void SubstituteUserOptions(Options options)
        {
            // user can override source and destination directories via Options
            if (options.ImagesSourcePath != null)
                FullPathToMedia = options.ImagesSourcePath;

            if (options.ImagesDestinationPath != null)
            {
                DestinationBaseDirPhotos = options.ImagesDestinationPath;
                DestinationBaseDirVideos = options.ImagesDestinationPath;
            }
        }
    }
}