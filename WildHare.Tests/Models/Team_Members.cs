using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Tests.Models
{
    public class Team_Member
    {

        public int Id { get; set; }

        public int TeamId { get; set; }

        public string UserId { get; set; }


		[MinLength(8), MaxLength(50)]
		public string UserName  { get; set; }

        public override string ToString()
        {
            return $"{Id} on team {TeamId} member: {UserName} ({UserId})";
        }
    }
}
