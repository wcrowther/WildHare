using System;

namespace WildHare.Extensions.Xtra
{
    // Not sure of the value. Was a possible solution for a Random tree problem.
    // May remove...
    public static class RandomExtensions
    {
        public static Random Skip(this Random random, int number)
        {
            if (number < 0) number = 0;

            for (int i = 1; i <= number; i++)
            {
                random.Next();
            }
            return random;
        }
    }
}
