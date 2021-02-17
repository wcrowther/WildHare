
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Tests.Models
{
    public class Phrase : Token
    {
        public List<Word> Words { get; set; } = new List<Word>();

        public override string Text => string.Join(' ', Words.Select(s => s.Text));

        public override string ToString() => $"{Words.Count} Words : {string.Join(' ', Words)}";
    }
}
