using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Tests.Models
{
    [Serializable]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Editable(true), MinLength(2), MaxLength(50)]
        public string ItemName { get; set; }

        public DateTime Created { get; set; }

        public List<string> Stuff { get; set; } = new List<string>();

        public bool HasStuff => Stuff.Count > 0;    

        public override string ToString()
        {
            return $"{ItemName} ({ItemId})";
        }
    }
}
