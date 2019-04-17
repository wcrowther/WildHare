
using System;

namespace WildHare.Web.Models
{
	public class Tevents
	{
		public int TeventId { get; set; }

		public uniqueidentifier TeventGuid { get; set; }

		public nvarchar TeventName { get; set; }

		public nvarchar TeventSummary { get; set; }

		public nvarchar TeventInfo { get; set; }

		public datetime2 StartDate { get; set; }

		public int OwnerUserId { get; set; }

		public nvarchar TeaserSrc { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}