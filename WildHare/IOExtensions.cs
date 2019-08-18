using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WildHare.Extensions
{
    public static class IOExtensions
    {

        /// <summary>Writes the {stringToWrite} to the {fileName} string. If {overwrite} is true, it will
        /// overwrite existing file returning a sucess boolean It will create the file if it does not exist,
        /// but will not create the parent folder structure if that is not in place.</summary>
        public static bool WriteToFile(this string stringToWrite, string fileName, bool overwrite = false)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.WriteToFile(file, overwrite);
        }

        /// <summary>Writes the {stringToWrite} to the {fileName} FileInfo. If {overwrite} is true, it will
        /// overwrite existing file returning a sucess boolean It will create the file if it does not exist,
        /// but will not create the parent folder structure if that is not in place.</summary>
        public static bool WriteToFile(this string stringToWrite, FileInfo file, bool overwrite = false)
        {
            if (file.Exists && overwrite == false)
            {
                return false;
            }
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

        /// <summary>Turns a relative path in an application into an absolute file path similar the old MapPath function.</summary>
        public static string ToMapPath(this string fileName)
        {
            var appRoot = GetApplicationRoot();
            string[] characters = { "~", "/", @"\" };
            string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

            return Path.Combine(appRoot, filePath);
        }

        /// <summary>Gets the root path of an application. This can have different meanings it different types of 
        /// applications, so check that your usage fully meets your needs before proceeding...</summary>
        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			Debug.WriteLine("exePath: " + exePath);

			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
			Debug.WriteLine("appRoot: " + appRoot);

			string entryAssembly = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			Debug.WriteLine("entryAssembly: " + entryAssembly);


			return appRoot;
        }
    }
}
