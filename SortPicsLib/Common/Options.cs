using System;
using CommandLine;
using CommandLine.Text;

namespace SortPicsLib.Common
{
    public class Options
    {
        [Option('y', "year", Required = false, DefaultValue = null, HelpText = "Filter year")]
        //todo: only accept valid years 
        //todo: don't accept negative numbers
        public int FilterYear { get; set; }

        [Option('m', "month", Required = false, DefaultValue = null, HelpText = "Filter month")]
        public int FilterMonth { get; set; }

        [Option('d', "day", Required = false, DefaultValue = null, HelpText = "Filter day")]
        public int FilterDay { get; set; }

        [Option("source", Required = false, DefaultValue = null, HelpText = "Images source path")]
        public string ImagesSourcePath { get; set; }

        [Option("destination", Required = false, DefaultValue = null, HelpText = "Images destination path")]
        public string ImagesDestinationPath { get; set; }

        /// <summary>
        ///     Construct Usage message.
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
    }
}