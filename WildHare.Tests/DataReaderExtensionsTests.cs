using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
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

            Assert.AreEqual(1,      first.TestId);
            Assert.AreEqual("One",  first.TestName);
            Assert.AreEqual(123,    first.TestNumber);

            var second = tests.ElementAt(1);

            Assert.AreEqual(2,      second.TestId);
            Assert.AreEqual("Two",  second.TestName);
            Assert.AreEqual(23456,  second.TestNumber);

            var third = tests.ElementAt(2);

            Assert.AreEqual(3,      third.TestId);
            Assert.AreEqual("Three",third.TestName);
            Assert.AreEqual(345690, third.TestNumber);
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

            Assert.AreEqual(1,          first.TestId);
            Assert.AreEqual("One",      first.TestName);
            Assert.AreEqual(123,        first.TestNumber);
            Assert.AreEqual(null,       first.TestNull);
            Assert.AreEqual("Default",  first.TestNullDefault);

            var second = tests.ElementAt(1); // zero-based second element

            Assert.AreEqual(2,          second.TestId);
            Assert.AreEqual("Two",      second.TestName);
            Assert.AreEqual(23456,      second.TestNumber);
            Assert.AreEqual(null,       second.TestNull);
            Assert.AreEqual("Default",  second.TestNullDefault);

            var third = tests.ElementAt(2);

            Assert.AreEqual(3,          third.TestId);
            Assert.AreEqual("Three",    third.TestName);
            Assert.AreEqual(345690,     third.TestNumber);
            Assert.AreEqual(null,       third.TestNull);
            Assert.AreEqual("Default",  third.TestNullDefault);
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