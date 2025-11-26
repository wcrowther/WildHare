using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
	[TestFixture]
	public class DataTableExtensionsTests
	{
		[Test]
		public void DataTable_ToList_Of_Object_Test()
		{
			var prescriptionTable = GetPrescriptionTable();
			var prescriptionList  = prescriptionTable.ToList<Prescription>();

            ClassicAssert.AreEqual(prescriptionTable.Rows.Count, prescriptionList.Count);
            ClassicAssert.AreEqual(prescriptionTable.Columns.Count, prescriptionList.GetMetaProperties().Count);

            var merchandiseTable = GetMerchandiseTable();
            var merchandiseList  = GetMerchandiseTable().ToList<Merchandise>();

            ClassicAssert.AreEqual(merchandiseTable.Rows.Count,    merchandiseList.Count);
            ClassicAssert.AreEqual(merchandiseTable.Columns.Count, merchandiseList.GetMetaProperties().Count);
        }

        [Test]
        public void DataSet_ToList_Of_Object_Test()
        {
            var dataSet = new DataSet("DataSet1")
            { 
                Tables = 
                {
                    GetPrescriptionTable(),
                    GetMerchandiseTable()
                }
            };

            // =======================================================================
            var table0 = dataSet.Tables[0];

            ClassicAssert.AreEqual(5,          table0.Rows.Count);
            ClassicAssert.AreEqual(25,         (int)table0.Rows[0]["Dosage"]);
            ClassicAssert.AreEqual("Indocin",  table0.Rows[0]["Drug"]);
            ClassicAssert.AreEqual("David",    table0.Rows[0]["Patient"]);

            // =======================================================================
            // Using System.Data.DataSetExtensions DataRow.Field extension method

            ClassicAssert.AreEqual(50,         table0.Rows[1].Field<int>("Dosage") );
            ClassicAssert.AreEqual("Enebrel",  table0.Rows[1].Field<string>("Drug"));
            ClassicAssert.AreEqual("Sam",      table0.Rows[1].Field<string>("Patient"));

            // =======================================================================

            ClassicAssert.AreEqual(5,         dataSet.Tables[1].Rows.Count);
            ClassicAssert.AreEqual(1,         (int)dataSet.Tables[1].Rows[0]["ProductId"]);
            ClassicAssert.AreEqual("Toy",     dataSet.Tables[1].Rows[0]["ProductName"]);

            // =======================================================================
        }

		  [Test]
		  public void DataSet_ToList_Prescriptions()
		  {
			   var dataSet = new DataSet("DataSet1")
			   {
					Tables =
					{
						GetPrescriptionTable(),
						GetMerchandiseTable()
					}
			   };

			   var prescriptions = dataSet.Tables[0].ToList<Prescription>();

			   ClassicAssert.AreEqual(25,			 prescriptions.First().Dosage);
			   ClassicAssert.AreEqual("Indocin",	 prescriptions.First().Drug);
			   ClassicAssert.AreEqual("David",		 prescriptions.First().Patient);
		  }


		  [Test]
		  public void DataSet_ToList_Merchandise()
		  {
			   var dataSet = new DataSet("DataSet1")
			   {
					Tables =
					{
						GetPrescriptionTable(),
						GetMerchandiseTable()
					}
			   };

			   var merchandise = dataSet.Tables[1].ToList<Merchandise>();

			   ClassicAssert.AreEqual(1, merchandise.First().ProductId);
			   ClassicAssert.AreEqual("Toy", merchandise.First().ProductName);
			   ClassicAssert.AreEqual(typeof(DateTime), merchandise.First().Created.GetType());
		  }

		  // ===================================================================
		  // LOAD DATA TABLES
		  // ===================================================================

		  static DataTable GetPrescriptionTable()
		  {
		  	   return new DataTable()
		  	   {
		  	   		Columns = 
					{
		  	   			{ "Dosage",	 typeof(int)      },
		  	   			{ "Drug",	 typeof(string)   },
		  	   			{ "Patient", typeof(string)   },
		  	   			{ "Created", typeof(DateTime) }
		  	   		},
		  	   		Rows = 
					{
		  	   			{ 25, "Indocin",     "David",     DateTime.Now },
		  	   			{ 50, "Enebrel",     "Sam",       DateTime.Now },
		  	   			{ 10, "Hydralazine", "Christoff", DateTime.Now },
		  	   			{ 21, "Combivent",   "Janet",     DateTime.Now },
		  	   			{ 100,"Dilantin",    "Melanie",   DateTime.Now }
		  	   		}
		  	   };
		  }

		  static DataTable GetMerchandiseTable()
		  {
		      return new DataTable()
		      {
		          Columns = 
		          {
		              { "ProductId",      typeof(int)      },
		              { "ProductName",    typeof(string)   },
		              { "Created",        typeof(DateTime) }
		          },
		          Rows = 
		          {
		              { 1, "Toy",         DateTime.Now },
		              { 2, "Candy",       DateTime.Now },
		              { 3, "Toothpaste",  DateTime.Now },
		              { 4, "Toothbrush",  DateTime.Now },
		              { 5, "Soda",        DateTime.Now }
		          }
		      };
		  }
    }

    public class Prescription
    {
        public int Dosage { get; set; }

        public string Drug { get; set; }

        public string Patient { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{Patient} Drup: {Drug}";
        }
    }

    public class Merchandise
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{ProductId} ProductName: {ProductName}";
        }
    }
}