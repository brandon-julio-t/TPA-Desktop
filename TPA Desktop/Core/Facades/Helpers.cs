using System;
using System.Linq;

namespace TPA_Desktop.Core.Facades
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();

        public static int RandomDigit()
        {
            return Random.Next(10);
        }

        public static string RandomDigitString(int length)
        {
            return string.Join(
                "",
                Enumerable
                    .Range(0, length)
                    .Select(_ => RandomDigit())
            );
        }
    }
}