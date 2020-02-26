using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WildHare.Extensions
{
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

        // ==============================================================================
        // Gets FileSystemInfo - the base class for both FileInfo and DirectoryInfo
        // ==============================================================================


        /// <summary>Gets a recursive list of FileSystemInfos (both directories and files) to a depth of {maxDepth}. Defaults to a depth of 2.</summary>
        public static List<FileSystemInfo> GetAllDirectoriesAndFiles(this DirectoryInfo directory, int maxDepth = 2)
        {
            int level = 0;
            var directoriesAndFiles = new List<FileSystemInfo>();

            EnumerateFileSystemInfos(directory, directoriesAndFiles, level, maxDepth);

            return directoriesAndFiles;
        }

        internal static void EnumerateFileSystemInfos(DirectoryInfo directory, List<FileSystemInfo> fileInfoList, int depth, int maxDepth)
        {
            try
            {
                var list = directory.GetFileSystemInfos();
                var dirs = new List<FileSystemInfo>();
                var files = new List<FileSystemInfo>();

                foreach (FileSystemInfo info in list)
                {
                    if (info.IsDirectory())
                    {
                        dirs.Add(info);
                    }
                    else
                    {
                        files.Add(info);
                    }
                }

                if (dirs.Count == 0)
                    return;

                if (depth <= maxDepth)
                {
                    foreach (var dir in dirs)
                    {
                        EnumerateFileSystemInfos((DirectoryInfo)dir, fileInfoList, depth + 1, maxDepth);
                    }
                }

                fileInfoList.AddRange(dirs);
                fileInfoList.AddRange(files);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllFileSystemInfos Exception: {ex.Message}");
            }
        }

        public static bool IsDirectory(this FileSystemInfo info)
        {
            return (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        // ==============================================================================
        // Only Gets Files
        // ==============================================================================


        public static List<FileInfo> GetAllFiles(this string path)
        {
            var fileList = new List<FileInfo>();

            EnumerateFiles(path, fileList);

            return fileList;
        }

        internal static void EnumerateFiles(string sFullPath, List<FileInfo> fileInfoList)
        {
            try
            {
                var di = new DirectoryInfo(sFullPath);
                FileInfo[] files = di.GetFiles();

                foreach (FileInfo file in files)
                {
                    fileInfoList.Add(file);
                }

                //Scan recursively
                DirectoryInfo[] dirs = di.GetDirectories();

                if (dirs == null || dirs.Length < 1)
                {
                    return;
                }

                foreach (DirectoryInfo dir in dirs)
                {
                    EnumerateFiles(dir.FullName, fileInfoList);
                }
            }
            catch (Exception ex)
            {
                // Logger.Write("Exception in Helper.EnumerateFiles", ex);

                Debug.WriteLine($"EnumerateFiles Exception: {ex.Message}" );
            }
        }
    }
}
