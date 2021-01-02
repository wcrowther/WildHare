namespace WildHare.Tests.Models
{
    public class Word
    {
        public int WordId { get; set; }

        public string WordName { get; set; }

        public override string ToString()
        {
            return $"{WordName} {WordId}";
        }
    }
}
