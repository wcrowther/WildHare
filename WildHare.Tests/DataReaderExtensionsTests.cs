using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class DataReaderExtensionsTests
    {
        [Test]
        public void DataReader_Test_With_SQLite_ADO_Basic()
        {
            var tests = new List<Test>();

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM tblTest";

                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var test = new Test
                        {
                            TestId      = dr.GetInt32(0),
                            TestName    = dr.GetString(1),
                            TestNumber  = dr.GetInt32(2)

                            // TestNull    = dr.GetString(3) 
                            // BREAKS: need to test for null etc...
                        };
                        tests.Add(test);
                    }
                }
            }

            var first = tests.First();

            ClassicAssert.AreEqual(1,      first.TestId);
            ClassicAssert.AreEqual("One",  first.TestName);
            ClassicAssert.AreEqual(123,    first.TestNumber);

            var second = tests.ElementAt(1);

            ClassicAssert.AreEqual(2,      second.TestId);
            ClassicAssert.AreEqual("Two",  second.TestName);
            ClassicAssert.AreEqual(23456,  second.TestNumber);

            var third = tests.ElementAt(2);

            ClassicAssert.AreEqual(3,      third.TestId);
            ClassicAssert.AreEqual("Three",third.TestName);
            ClassicAssert.AreEqual(345690, third.TestNumber);
        }

        [Test]
        public void DataReader_Test_With_SQLite_DataReader_Extension_Basic()
        {
            var tests = new List<Test>();

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM tblTest";

                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var test = new Test
                        {
                            TestId              = dr.Get<int>("TestId"),
                            TestName            = dr.Get("TestName"),
                            TestNumber          = dr.Get<int>("TestNumber"),
                            TestNull            = dr.Get("TestNull"),
                            TestNullDefault     = dr.Get("TestNull", "Default")

                            // Extension methods returns null (with optional default)
                        };
                        tests.Add(test);
                    }
                }
            }

            var first = tests.First();

            ClassicAssert.AreEqual(1,          first.TestId);
            ClassicAssert.AreEqual("One",      first.TestName);
            ClassicAssert.AreEqual(123,        first.TestNumber);
            ClassicAssert.AreEqual(null,       first.TestNull);
            ClassicAssert.AreEqual("Default",  first.TestNullDefault);

            var second = tests.ElementAt(1); // zero-based second element

            ClassicAssert.AreEqual(2,          second.TestId);
            ClassicAssert.AreEqual("Two",      second.TestName);
            ClassicAssert.AreEqual(23456,      second.TestNumber);
            ClassicAssert.AreEqual(null,       second.TestNull);
            ClassicAssert.AreEqual("Default",  second.TestNullDefault);

            var third = tests.ElementAt(2);

            ClassicAssert.AreEqual(3,          third.TestId);
            ClassicAssert.AreEqual("Three",    third.TestName);
            ClassicAssert.AreEqual(345690,     third.TestNumber);
            ClassicAssert.AreEqual(null,       third.TestNull);
            ClassicAssert.AreEqual("Default",  third.TestNullDefault);
        }

        private IDbConnection GetConnection()
        {
            var builder     = new ConfigurationBuilder()
                                //.SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appSettings.json")
                                .Build();

            string connStr = builder["App:SQLiteData"];

            return new SqliteConnection(connStr);
        }
    }

    public class Test
    {
        public int TestId { get; set; }

        public string TestName { get; set; }

        public int TestNumber { get; set; }

        public string TestNull { get; set; }

        public string TestNullDefault { get; set; }

	   public override string ToString() => $"{TestId} TestName: {TestName} - {TestNumber}";
    }
}