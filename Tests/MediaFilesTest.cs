﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SortPicsLib.Common;
using SortPicsLib.Images;

namespace Tests
{
    [TestFixture]
    public class MediaFilesTest
    {
        // set up fields
        public string TestFile1;

        public string TestFile2;
        public string TestFile3;
        public string TestFileNotMediaFile;

        public MediaFile TestImage1;

        private string imagesDirectory;
        private string destinationBaseDirPhotos;
        private string destinationBaseDirVideos;
        private RuntimeSettings runtimeSettings;

        private List<MediaFile> images;

        [OneTimeSetUp]
        public void SetUpImages()
        {
            imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testfiles");
            destinationBaseDirVideos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "destinationVideos");
            destinationBaseDirPhotos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "destinationPhotos");

            // in order for tests to work, we need to ensure that file modification dates are set accordingly
            // as they will break if doing a clean checkout
            TestFile1 = Path.Combine(imagesDirectory, "20170224_115931000_iOS.png");
            TestFile2 = Path.Combine(imagesDirectory, "20170126_012712498_iOS.jpg");
            TestFile3 = Path.Combine(imagesDirectory, "computer-2893112_640.png");
            TestFileNotMediaFile = Path.Combine(imagesDirectory, "not_a_media_file.txt");
            File.SetLastWriteTime(TestFile1, new DateTime(2017, 1, 10)); // January 10th 2017
            File.SetLastWriteTime(TestFile2, new DateTime(2017, 2, 11)); // February 11th 2017
            File.SetLastWriteTime(TestFile3, new DateTime(2017, 2, 12)); // February 12th 2017
            // populate List<MediaFile> for later consumption by tests  
            images = Images.FindImages(imagesDirectory);
            // todo: add all images as TestImages here and remove from other methods
            TestImage1 = images.FirstOrDefault(s => s.FileName == "20170224_115931000_iOS.png");

            // create new directories for testing media file moving
            Directory.CreateDirectory(destinationBaseDirPhotos);
            Directory.CreateDirectory(destinationBaseDirVideos);

            runtimeSettings = new RuntimeSettings(imagesDirectory, destinationBaseDirPhotos,
                destinationBaseDirVideos);
            runtimeSettings.FilterYear = 2017;
            runtimeSettings.FilterMonth = 2;
        }

        [OneTimeTearDown]
        public void TearDownImages()
        {
            Directory.Delete(destinationBaseDirPhotos, true);
            Directory.Delete(destinationBaseDirVideos, true);
            Directory.Delete(imagesDirectory, true);
        }

        [Test]
        public void ImagesCountIsCorrect()
        {
            Assert.AreEqual(images.Count, 5);
        }

        [Test]
        public void TestDetectIphone()
        {
            var testImagePng = images.Where(s => s.FileName == "20170126_012712498_iOS.jpg").FirstOrDefault();
            Assert.IsTrue(testImagePng.IsImage);
            Assert.AreEqual(testImagePng.MimeType, "image/jpeg");
            Assert.AreEqual(testImagePng.CameraMake, "Apple");
            Assert.AreEqual(testImagePng.CameraModel, "iPhone 6s");
        }

        [Test]
        public void TestFilesAreNotTheSame()
        {
            Assert.IsFalse(FileHash.FilesAreTheSame(TestFile1, TestFile2));
        }

        [Test]
        public void TestFilesAreTheSame()
        {
            Assert.IsTrue(FileHash.FilesAreTheSame(TestFile1, TestFile1));
        }

        [Test]
        public void TestFilteringByYear()
        {
            var imagesFiltered = Images.FilterImagesByDate(images, 2017, 0, 0);
            Assert.AreEqual(5, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonth()
        {
            var imagesFiltered = Images.FilterImagesByDate(images, 2017, 2, 0);
            Assert.AreEqual(2, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonthShouldBeOne()
        {
            var imagesFiltered = Images.FilterImagesByDate(images, 2017, 1, 0);
            Assert.AreEqual(1, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonthShouldBeZero()
        {
            var imagesFiltered = Images.FilterImagesByDate(images, 2017, 5, 0);
            Assert.AreEqual(0, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonthShouldBeZero2()
        {
            var imagesFiltered = Images.FilterImagesByDate(images, 2015, 3, 0);
            Assert.AreEqual(0, imagesFiltered.Count);
        }

        [Test]
        public void TestFindImagesFiltered()
        {
            var images = Images.FindImagesFiltered(runtimeSettings);
            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images[0].FileName == "20170126_012712498_iOS.jpg");
        }

        [Test]
        public void TestFindImagesFilteredNoResults()
        {
            runtimeSettings = new RuntimeSettings(imagesDirectory, destinationBaseDirPhotos,
                destinationBaseDirVideos);
            runtimeSettings.FilterYear = 2015;
            runtimeSettings.FilterMonth = 1;
            var images = Images.FindImagesFiltered(runtimeSettings);
            Assert.AreEqual(0, images.Count);
        }

        [Test]
        public void TestGeoTagging()
        {
            var testImagePng = images.Where(s => s.FileName == "20170126_012712498_iOS.jpg").FirstOrDefault();
            Assert.AreEqual(testImagePng.GpsLatitude, "37° 49' 41.88\"");
            Assert.AreEqual(testImagePng.GpsLongitude, "-122° 29' 51.49\"");
        }

        [Test]
        public void TestJpgFile()
        {
            var testImagePng = images.Where(s => s.FileName == "nobody-2798850_640.jpg").FirstOrDefault();
            Assert.IsTrue(testImagePng.IsImage);
            Assert.AreEqual(testImagePng.MimeType, "image/jpeg");
        }

        [Test]
        public void TestMd5Hash()
        {
            var md5Hash = FileHash.GetMd5Hash(TestFile1);
            Assert.AreEqual("28-F5-99-56-F0-F6-FA-98-E2-53-A7-56-55-AC-A1-54", md5Hash);
        }

        /// <summary>
        ///     Test instantiating MediaFile with a valid image file.
        /// </summary>
        [Test]
        public void TestMediaFileInstance()
        {
            var mediaFile = new MediaFile(TestFile1);
            Assert.IsTrue(mediaFile.IsImage);
            Assert.IsFalse(mediaFile.IsVideo);
        }

        /// <summary>
        ///     Test instantiating MediaFile with a non-media file.
        ///     In this case test it with a text file.
        /// </summary>
        [Test]
        public void TestMediaFileInstanceNotMediaFile()
        {
            var exception = Assert.Throws<UnsupportedFileTypeException>(() => new MediaFile(TestFileNotMediaFile));
            Assert.That(exception.Message, Is.EqualTo("Unsupported file type."));
        }

        [Test]
        public void TestMediaFileStringRepresentation()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            Assert.AreEqual(testImagePng.ToString(), "Media file: computer-2893112_640.png");
        }

        [Test]
        public void TestMoveImage()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            var destinationBaseDirPhotosWithYear = Path.Combine(destinationBaseDirPhotos, "2017");
            var destinationBaseDirPhotosWithYearAndMonth = Path.Combine(destinationBaseDirPhotosWithYear, "02");

            var destinationFileAfterMove =
                Path.Combine(destinationBaseDirPhotosWithYearAndMonth, testImagePng.FileName);
            Directory.CreateDirectory(
                destinationBaseDirPhotosWithYearAndMonth); // todo: remove this once Move() logic is improved
            // check that file does not exist before the move
            Assert.IsFalse(File.Exists(destinationFileAfterMove));
            // move the file
            Images.Move(testImagePng, destinationBaseDirPhotos, destinationBaseDirVideos, false);
            // verify that it exists in destination dir now
            Assert.IsTrue(File.Exists(destinationFileAfterMove));
            Directory.Delete(destinationBaseDirPhotosWithYear, true); // cleanup
        }


        [Test]
        public void TestMoveImageDryRun()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            var destinationBaseDirPhotosWithYear = Path.Combine(destinationBaseDirPhotos, "2017");
            var destinationBaseDirPhotosWithYearAndMonth = Path.Combine(destinationBaseDirPhotosWithYear, "02");

            var destinationFileAfterMove =
                Path.Combine(destinationBaseDirPhotosWithYearAndMonth, testImagePng.FileName);
            Directory.CreateDirectory(
                destinationBaseDirPhotosWithYearAndMonth); // todo: remove this once Move() logic is improved
            // check that file does not exist before the move
            Assert.IsFalse(File.Exists(destinationFileAfterMove));
            // move the file
            Images.Move(testImagePng, destinationBaseDirPhotos, destinationBaseDirVideos);
            // verify that in fact nothing changed, as this was a dry run
            Assert.IsFalse(File.Exists(destinationFileAfterMove));
        }

        [Test]
        public void TestMoveImageFileExists()
        {
            var destination = new Destination(TestImage1, destinationBaseDirPhotos, destinationBaseDirVideos);
            Images.CreateDirectoryIfNotExists(destination.Directory);
            File.Copy(TestImage1.FilePath, destination.Path);
            Assert.Throws<FilesAreTheSameException>(() =>
                Images.Move(TestImage1, destinationBaseDirPhotos, destinationBaseDirVideos, false));
        }

        [Test]
        public void TestMoveImageFileExistsButContentsDiffer()
        {
            //
        }

        [Test]
        public void TestPngFile()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            Assert.IsTrue(testImagePng.IsImage);
            Assert.AreEqual(testImagePng.MimeType, "image/png");
        }
    }
}