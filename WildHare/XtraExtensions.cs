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

        /// <summary>(EXPERIMENTAL) Gets the root path of an application. This can have different meanings in
        /// different types of applications, so check that your usage fully meets your needs before proceeding...</summary>
        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Debug.WriteLine("exePath: " + exePath);

			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
			Debug.WriteLine("appRoot: " + appRoot);

			string entryAssembly = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			Debug.WriteLine("entryAssembly: " + entryAssembly);

			return appRoot;
        }

        /// <example>
        /// var list = CreateListOfType(typeof(Category)); // Is IList of object
        /// list.Add(new Category() { CategoryName = "Will"});
        /// </example>
        public static dynamic DynamicListOfType(dynamic t)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var instance = Activator.CreateInstance(constructedListType);

            return instance;
        }

        /// Alternative to DynamicListOfType. Not sure which is better.
        /// <example>
        /// var list = CreateListOfType(typeof(Category)); // Is IList of object
        /// list.Add(new Category() { CategoryName = "Will"});
        /// </example>
        public static dynamic[] DynamicArrayOfType(dynamic t)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var instance = Activator.CreateInstance(constructedListType);

            return (dynamic[])instance.ToArray();
        }

        // DOES NOT WORK: IQueryable 3.1 can no longer be cast to DbSet per:
        // https ://stackoverflow.com/questions/21533506/find-a-specified-generic-dbset-in-a-dbcontext-dynamically-when-i-have-an-entity

    }
}
