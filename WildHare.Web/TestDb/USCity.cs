using System;
using System.ComponentModel.DataAnnotations;

// Generated from table: USCities

namespace Me.Logic.Models
{
    public class USCity
    {
        public int id { get; set; }

		[StringLength(50)]
		public string city { get; set; }

		[StringLength(50)]
		public string city_ascii { get; set; }

		[StringLength(50)]
		public string state_id { get; set; }

		[StringLength(50)]
		public string state_name { get; set; }

		public int county_fips { get; set; }

		[StringLength(50)]
		public string county_name { get; set; }

		public double lat { get; set; }

		public double lng { get; set; }

		public int population { get; set; }

		public int density { get; set; }

		[StringLength(50)]
		public string source { get; set; }

		public bool military { get; set; }

		public bool incorporated { get; set; }

		[StringLength(50)]
		public string timezone { get; set; }

		[StringLength(50)]
		public string ranking { get; set; }

		[StringLength(1900)]
		public string zips { get; set; }

		public override string ToString()
		{
			return $"id: {id}";
		}
    }
}