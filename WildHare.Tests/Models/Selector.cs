using System.Linq;
using System.Text.RegularExpressions;
using WildHare.Extensions;

namespace WildHare.Tests.Models
{
    public class Selector
    {
        public Selector(string rawSelector)
        {
            RawText = rawSelector;
        }

        public string RawText { get; set; }

        public string Main
        {
            get
            {
                return RawText.GetStart(" ", true).RemoveStart(new[] { ".", "#" });
            }
        }

        public string Secondary
        {
            get
            {
                string start = RawText.GetStart(" ", true);
                return RawText.RemoveStart(start);
            }
        }

        public SelectorType Type {
            get
            {
                char firstChar = RawText.Trim().CharAt(0);

                if (firstChar == '.')
                    return SelectorType.@class;
                else if (firstChar == '#')
                    return SelectorType.id;
                else if (char.IsLetter(firstChar))
                   return SelectorType.element;
                else
                    return SelectorType.other;
            }
        }

        public string Specificity { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{RawText} {Type} in: {Location}";
        }
    }

    public enum SelectorType
    {
        id,
        @class,
        element,
        other
    }
}
