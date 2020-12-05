using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WildHare.Extensions.Xtra
{
    public static partial class XtraExtensions
    {
        // =========================================================================================
        // Object Is and Object IsNull
        // =========================================================================================

        /// <summary>(EXPERIMENTAL)A simple shortcut method to test if an object {obj} is NOT null.</summary>
        public static bool Is(this object obj)
        {
            return (obj != null);
        }

        /// <summary>(EXPERIMENTAL) A simple shortcut method to test if an object {obj} is null.</summary>
        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }

        // =========================================================================================

        /// <summary>(EXPERIMENTAL) Turns a relative path in an application into an absolute file path similar the old MapPath function.</summary>
        public static string ToMapPath(this string fileName)
        {
            var appRoot = GetApplicationRoot();
            string[] characters = { "~", "/", @"\" };
            string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

            return Path.Combine(appRoot, filePath);
        }

        /// <summary>(EXPERIMENTAL) Gets the root path of an application. This can have different meanings it
        /// different types of applications, so check that your usage fully meets your needs before proceeding...</summary>
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

        // Example Usage:
        // var list = CreateListOfType(typeof(Category)); // Is IList of object
        // list.Add(new Category() { CategoryName = "Will"});
        public static dynamic CreateListOfType(dynamic t)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var instance = Activator.CreateInstance(constructedListType);

            return instance;
        }
    }
}
