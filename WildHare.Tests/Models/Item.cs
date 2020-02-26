using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Tests.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Editable(true)]
        public string ItemName { get; set; }

        public DateTime Created { get; set; }

        public List<string> Stuff { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{ItemName} ({ItemId})";
        }
    }
}
