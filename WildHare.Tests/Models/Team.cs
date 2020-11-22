using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Tests.Models
{
    public class Team
    {

        public int Id { get; set; }

        public string TeamName { get; set; }

        public int Branch  { get; set; }

        public int Year { get; set; } 

        public override string ToString()
        {
            return $"{TeamName} : {Id}";
        }
    }
}
