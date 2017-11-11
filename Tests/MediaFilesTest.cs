using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
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

        [SetUp]
        public void SetUpImages()
        {
            imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testfiles");
            // in order for tests to work, we need to ensure that file modification dates are set accordingly
            // as they will break if doing a clean checkout
            TestFile1 = Path.Combine(imagesDirectory, "20170224_115931000_iOS.png");
            TestFile2 = Path.Combine(imagesDirectory, "20170126_012712498_iOS.jpg");
            TestFile3 = Path.Combine(imagesDirectory, "computer-2893112_640.png");
            File.SetLastWriteTime(TestFile1, new DateTime(2017, 1, 10)); // January 10th 2017
            File.SetLastWriteTime(TestFile2, new DateTime(2017, 2, 11)); // February 11th 2017
            File.SetLastWriteTime(TestFile3, new DateTime(2017, 2, 12)); // February 12th 2017
            // populate List<MediaFile> for later consumption by tests  
            images = Images.FindImages(imagesDirectory);
        }

        private string imagesDirectory;
        private List<MediaFile> images;


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
        public void TestFilteringByYear()
        {
            var imagesFiltered = Images.FilterImages(images, 2017, 0, 0);
            Assert.AreEqual(5, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonth()
        {
            var imagesFiltered = Images.FilterImages(images, 2017, 2, 0);
            Assert.AreEqual(2, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonthShouldBeOne()
        {
            var imagesFiltered = Images.FilterImages(images, 2017, 1, 0);
            Assert.AreEqual(1, imagesFiltered.Count);
        }

        [Test]
        public void TestFilteringByYearAndMonthShouldBeZero()
        {
            var imagesFiltered = Images.FilterImages(images, 2017, 5, 0);
            Assert.AreEqual(0, imagesFiltered.Count);
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
        public void TestMediaFileStringRepresentation()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            Assert.AreEqual(testImagePng.ToString(), "Media file: computer-2893112_640.png");
        }

        [Test]
        public void TestPngFile()
        {
            var testImagePng = images.Where(s => s.FileName == "computer-2893112_640.png").FirstOrDefault();
            Assert.IsTrue(testImagePng.IsImage);
            Assert.AreEqual(testImagePng.MimeType, "image/png");
        }

        [Test]
        public void TestMd5Hash()
        {
            var md5Hash = SortPicsLib.Common.FileHash.GetMd5Hash(Path.Combine(imagesDirectory, "20170224_115931000_iOS.png"));
            Assert.AreEqual("28-F5-99-56-F0-F6-FA-98-E2-53-A7-56-55-AC-A1-54", md5Hash);
        }


        [Test]
        public void TestFilesAreTheSame()
        {
            var fileName1 = Path.Combine(imagesDirectory, "20170224_115931000_iOS.png");
            Assert.IsTrue(SortPicsLib.Common.FileHash.FilesAreTheSame(fileName1, fileName1));
        }


        [Test]
        public void TestFilesAreNotTheSame()
        {
            var fileName1 = Path.Combine(imagesDirectory, "20170224_115931000_iOS.png");
            var fileName2 = Path.Combine(imagesDirectory, "computer-2893112_640.png");
            Assert.IsFalse(SortPicsLib.Common.FileHash.FilesAreTheSame(fileName1, fileName2));

        }
    }
}