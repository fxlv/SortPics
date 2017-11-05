using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SortPicsLib.Common;
using SortPicsLib.Images;
[TestFixture]
public class MediaFilesTest
{
    [SetUp]
    public void SetUpImages()
    {
        imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testfiles");
        // in order for tests to work, we need to ensure that file modification dates are set accordingly
        // as they will break if doing a clean checkout
        var testFile1 = Path.Combine(imagesDirectory, "20170224_115931000_iOS.png");
        var testFile2 = Path.Combine(imagesDirectory, "20170126_012712498_iOS.jpg");
        var testFile3 = Path.Combine(imagesDirectory, "computer-2893112_640.png");
        File.SetLastWriteTime(testFile1, new DateTime(2017, 1, 10));// January 10th 2017
        File.SetLastWriteTime(testFile2, new DateTime(2017, 2, 11));// February 11th 2017
        File.SetLastWriteTime(testFile3, new DateTime(2017, 2, 12));// February 12th 2017
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
}