using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WildHare.Extensions
{
    public static class IOExtensions
    {

        public static bool WriteToFile(this string stringToWrite, string fileName, bool overWriteExisting = false)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.WriteToFile(file, overWriteExisting);
        }

        public static bool WriteToFile(this string stringToWrite, FileInfo file, bool overWriteExisting = false)
        {
            if (file.Exists && overWriteExisting != true)
            {
                return false;
            }
            using (var tw = new StreamWriter(file.Create()))
            {
                tw.Write(stringToWrite);
            }
            return true;
        }

        public static bool AppendToFile(this string stringToWrite, string fileName)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.AppendToFile(file);
        }

        public static bool AppendToFile(this string stringToWrite, FileInfo file)
        {
            using (var tw = file.AppendText())
            {
                tw.Write(stringToWrite);
            }
            return true;
        }

        public static string ToMapPath(this string fileName)
        {
            var appRoot = GetApplicationRoot();
            string[] characters = { "~", "/", @"\" };
            string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

            return Path.Combine(appRoot, filePath);
        }

        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;

            return appRoot;
        }
    }
}
