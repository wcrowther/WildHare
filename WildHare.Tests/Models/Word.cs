namespace WildHare.Tests.Models
{
    public class Word : Token
    {
        public int WordId { get; set; }

        public override string Text { get; set; }

        public string Info { get; set; }

        public override string ToString()
        {
            return $"{Text} {WordId}";
        }
    }
}
