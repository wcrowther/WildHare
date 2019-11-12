using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WildHare.Extensions
{
    /// <summary>Input/Output extension methods to help with templating.</summary>
    public static class IOExtensions
    {

        /// <summary>Writes the {stringToWrite} to the {fileName} string. If {overwrite} is true, it will
        /// overwrite existing file returning a success boolean. It will create the file if it does not exist,
        /// but will not create the parent folder structure if that is not in place.</summary>
        public static bool WriteToFile(this string stringToWrite, string fileName, bool overwrite = false)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.WriteToFile(file, overwrite);
        }

        /// <summary>Writes the {stringToWrite} to the {fileName} FileInfo. If {overwrite} is true, it will
        /// overwrite existing file returning a success boolean. It will create the file if it does not exist,
        /// but will not create the parent folder structure if that is not in place.</summary>
        public static bool WriteToFile(this string stringToWrite, FileInfo file, bool overwrite = false)
        {
            if (file.Exists && overwrite == false)
            {
                return false;
            }

            // If directories do not exist create them
            file.Directory.Create();

            using (var tw = new StreamWriter(file.Create()))
            {
                tw.Write(stringToWrite);
            }
            return true;
        }

        /// <summary>Writes the {stringToWrite} to the end of the {fileName} content, returning true or false.</summary>
        public static bool AppendToFile(this string stringToWrite, string fileName)
        {
            var file = new FileInfo(fileName);

            // If directories do not exist create them
            file.Directory.Create();

            return stringToWrite.AppendToFile(file);
        }

        /// <summary>Writes the {stringToWrite} to the end of the {fileName} FileInfo content, returning true or false.</summary>
        public static bool AppendToFile(this string stringToWrite, FileInfo file)
        {
            using (var tw = file.AppendText())
            {
                tw.Write(stringToWrite);
            }
            return true;
        }
    }
}
