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
        /// <summary>(EXPERIMENTAL) Turns a relative path in an application into an
        /// absolute file path similar the old MapPath function.</summary>
        /// <example>
        ///		string pathToTestXmlFile = @"\SourceFiles\xmlSeedSourcePlus.xml".ToMapPath()
        /// </example>
        public static string ToMapPath(this string fileName)
        {
            var appRoot = GetApplicationRoot();
            string[] characters = { "~", "/", @"\" };
            string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

            return Path.Combine(appRoot, filePath);
        }

        /// <summary>(EXPERIMENTAL) Gets the root path of an application. This can have 
        /// different meanings in different types of applications, so check that your
        /// usage fully meets your needs before proceeding...</summary>
        /// <example>
		///		string pathToTestXmlFile = $@"{GetApplicationRoot()}\Logic\SourceFiles\xmlSeedSourcePlus.xml"
		/// </example>
        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
        }

        /// <example>
        ///     var list = CreateListOfType(typeof(Category)); // Is IList of object
        ///     list.Add(new Category() { CategoryName = "Will"});
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
        ///     var list = CreateListOfType(typeof(Category)); // Is IList of object
        ///     list.Add(new Category() { CategoryName = "Will"});
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
