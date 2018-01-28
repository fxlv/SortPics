using System;
using System.Runtime.Serialization;

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

        public FilesAreTheSameException(string message, Exception innerException) : base(message, innerException)
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

        public FilesAreTheSameButContentsDifferException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }

    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException()
        {
        }

        public UnsupportedFileTypeException(string message) : base(message)
        {
        }

        public UnsupportedFileTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}