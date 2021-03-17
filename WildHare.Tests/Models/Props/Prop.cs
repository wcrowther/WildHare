
namespace WildHare.Tests.Models
{
    public abstract class Prop
    {
        public int PropId { get; set; }

        public string Value { get; set; }

        public string Name => GetType().Name;

        public override string ToString() => $"{Name} : {Value}";
    }
}
