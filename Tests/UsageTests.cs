using System;
using System.IO;
using NUnit.Framework;
using SortPicsLib.Common;

namespace Tests
{
    [TestFixture]
    public class UsageTests
    {
        private StringWriter sw;
        private TextWriter originalOutput;

        [SetUp]
        public void SetUpImages()

        {
            /*
             Hijack ConsoleOut and direct ot to 'sw'
             Aftewards, restore back the original output.

            To access output in 'sw' use sw.ToString()
             */
            originalOutput = Console.Out;
            sw = new StringWriter();
            Console.SetOut(sw);
        }

        [TearDown]
        public void TearDownImages()
        {
            Console.SetOut(originalOutput);
        }

        [Test]
        public void TestGetUsage()
        {
            var options = new Options();
            var optionsString = options.GetUsage();
            Assert.That(optionsString.Contains("Usage: .\\SortPics -y 2017\n\n"));
        }

       
    }
}