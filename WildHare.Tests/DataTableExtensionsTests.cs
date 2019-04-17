using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
	[TestFixture]
	public class DataTableExtensionsTests
	{
		[Test]
		public void DataTableToListOfObject_Test()
		{
			var prescriptionTable = GetPrescriptionTable();
			var prescriptionList = prescriptionTable.DataTableToList<Prescription>();

			Assert.AreEqual(prescriptionTable.Rows.Count, prescriptionList.Count);

		}

		static DataTable GetPrescriptionTable()
		{
			return new DataTable()
			{
				Columns = {
					{ "Dosage",		typeof(int)},
					{ "Drug",		typeof(string) },
					{ "Patient",	typeof(string) },
					{ "Created",	typeof(DateTime) }
				},
				Rows = {
					{ 25, "Indocin", "David", DateTime.Now },
					{ 50, "Enebrel", "Sam", DateTime.Now },
					{ 10, "Hydralazine", "Christoff", DateTime.Now },
					{ 21, "Combivent", "Janet", DateTime.Now },
					{ 100, "Dilantin", "Melanie", DateTime.Now }
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
}