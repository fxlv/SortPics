using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SortPics.Images;

[TestFixture]
public class MediaFilesTest
{
    [SetUp]
    public void SetUpImages()
    {
        imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testfiles");
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