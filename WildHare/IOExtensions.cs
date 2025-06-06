using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace WildHare.Extensions
{
    public static class IOExtensions
    {
        /// <summary>Writes the {stringToWrite} to the {fileName} string. If {overwrite} is true, it will
        /// overwrite existing file returning a success boolean. It will create the file if it does not existe.</summary>
        public static bool WriteToFile(this string stringToWrite, string fileName, bool overwrite = false)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.WriteToFile(file, overwrite);
        }

        /// <summary>Writes the {stringToWrite} to the {fileName} FileInfo. If {overwrite} is true, it will
        /// overwrite existing file returning a success boolean. It will create the file if it does not exist.</summary>
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

        /// <summary>Gets a recursive list of FileSystemInfos (both directories and files) to a depth of {maxDepth}. 
        /// Defaults to a depth of 2.</summary>
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

                if (depth < maxDepth)
                {
					Debug.WriteLine($"depth: {depth}");

					int nestedDepth = depth + 1;

					foreach (var dir in dirs)
                    {
                        EnumerateFileSystemInfos((DirectoryInfo)dir, fileInfoList, nestedDepth, maxDepth);
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

        /// <summary>Returns bool whether current FileSystemInfo is a directory.</summary>
        public static bool IsDirectory(this FileSystemInfo info)
        {
            return (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>Gets a list of all files matching the {searchPattern} in current the directory and all subdirectories.</summary>
        public static List<FileInfo> GetAllFiles(this string directoryPath, string searchPattern = "*")
        {
            var di = new DirectoryInfo(directoryPath);

            var fileList = di.GetFiles(searchPattern, SearchOption.AllDirectories);

            return fileList.ToList();
        }

		/// <summary>This overload gets a list of all files matching a string array of {fileExtensions}
		/// in current directory and all subdirectories, and does NOT require a searchPattern. 
		/// The {topDirectoryOnly} defaults to true to only get the top level of files. If false it will
		/// get all matching children and then filter so use with caution if there are many files.
		/// Example: [".cshtml", ".razor"]</summary>
		public static List<FileInfo> GetAllFiles(this string directoryPath, string[] fileExtensions, bool topDirectoryOnly = true)
        {
			// https ://stackoverflow.com/questions/7039580/multiple-file-extensions-searchpattern-for-system-io-directory-getfiles

			var searchOption = topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;

			if (fileExtensions.Length == 0)
				throw new Exception($"The fileExtensions param of {nameof(GetAllFiles)} must be populated with at least 1 value.");

			var fileList = new DirectoryInfo(directoryPath)
							.GetFiles("*", searchOption)
							.Where(f => fileExtensions.Any(a => f.Name.EndsWith(a, true)));

            return fileList.ToList();
        }

        /// <summary>Gets a list of all directories in the current directory and 
        /// all subdirectories to the depth of {maxDepth}. Default {maxDepth} is 2.</summary>
        public static List<DirectoryInfo> GetAllDirectories(this string directoryPath, int maxDepth = 2)
        {
            var di = new DirectoryInfo(directoryPath);

            var directoryList = GetAllDirectoriesAndFiles(di, maxDepth); 

            return directoryList.OfType<DirectoryInfo>().ToList();
        }

        /// <summary>Gets the string content from a System.Io.FileInfo. If {strict} is true (the default),
        /// will throw an exception if the file is not found. If {strict} is false, will return null.</summary>
        public static string ReadFile(this FileInfo fileInfo, bool strict = true)
        {
            try
            {
				using StreamReader reader = fileInfo.OpenText();
				return reader.ReadToEnd();
			}
            catch
            {
                if (strict)
					throw;
				else
                    return null;
            }
        }

		/// <summary>Gets the child DirectoryInfo that equals {directoryName}. Returns null if no matches.</summary>
		public static DirectoryInfo Child(this DirectoryInfo directoryInfo, string directoryName)
        {
            string exMessage = "The DirectoryInfo.Child extension method requires a directoryName that is not null and not just whitespace.";

            if (directoryName.IsNullOrSpace())
                throw new Exception(exMessage);
            
            return directoryInfo.GetDirectories()
                                .FirstOrDefault(f => f.Name == directoryName);
        }

        /// <summary>Gets the sibling directory (at the same level as the DirectoryInfo) that 
        /// equals {directoryName}.  Returns null if no matches.</summary>
        public static DirectoryInfo Sibling(this DirectoryInfo directoryInfo, string directoryName)
        {
            string exMessage = "The DirectoryInfo.Sibling extension method requires a directoryName that is not null and not whitespace.";

            if (directoryName.IsNullOrSpace())
                throw new Exception(exMessage);

            return directoryInfo.Parent.Child(directoryName);
        }

		// =================================================================

		/// <summary>Gets a recursive list of FileSystemInfos (both directories and files) with an integer 
		/// for recursion depth to a depth of {maxDepth}. Defaults to a maxDepth of 2.</summary>
		public static IEnumerable<(FileSystemInfo Info, int Depth)> GetFileSystemHierarchy(this DirectoryInfo directory, 
																					            int maxRecursion = 2,
																								bool hideRoot = true,
																								string[] exclude = null )
		{
			if (directory is null || !directory.Exists)
				throw new DirectoryNotFoundException($"The directory does not exist.");

			exclude ??= [];

			var hiearchy = EnumerateDirectory(directory, 0, maxRecursion, exclude);

			return hideRoot ? hiearchy.Skip(1) : hiearchy;
		}

		// =================================================================

		private static IEnumerable<(FileSystemInfo Info, int Depth)> EnumerateDirectory( DirectoryInfo dir, int depth, 
																						 int maxDepth, string[] exclude )
		{
			if (depth > maxDepth) 
				yield break;

			yield return (dir, depth);

			FileSystemInfo[] entries;
			try
			{
				entries = dir.GetFileSystemInfos();
			}
			catch (UnauthorizedAccessException)
			{
				yield break; // Skip directories you can't access
			}

			foreach (var entry in entries)
			{
				if (entry is DirectoryInfo subDir && !entry.Name.EqualsAny(true, exclude))
				{
					// if (!subDir.Name.EqualsAny(true, exclude))
					//    yield return (entry, depth);

					foreach (var item in EnumerateDirectory(subDir, depth + 1, maxDepth, exclude))
					{
						if(!item.Info.Name.EqualsAny(true, exclude))
							yield return item;
					}
				}
				else
				{
					if (!entry.Name.EqualsAny(true, exclude))
						yield return (entry, depth + 1);
				}
			}
		}

	}
}
