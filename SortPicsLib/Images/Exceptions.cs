using System;

namespace SortPicsLib.Images
{
    public class FilesAreTheSameException : Exception
    {
        public FilesAreTheSameException()
        {
        }

        public FilesAreTheSameException(string message) : base(message)
        {
        }


    }

    public class FilesAreTheSameButContentsDifferException : Exception
    {
        public FilesAreTheSameButContentsDifferException()
        {
        }

        public FilesAreTheSameButContentsDifferException(string message) : base(message)
        {
        }


    }

    public class UnsupportedFileTypeException : Exception
    {

        public UnsupportedFileTypeException(string message) : base(message)
        {
        }

    }
}