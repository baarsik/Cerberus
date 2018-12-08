using System;
using System.Linq;

namespace Cerberus.Models.Helpers
{
    public static class StringHelpers
    {
        private static readonly Random Random = new Random();
        
        public static string GenerateRandomString(int length)
        {
            var chosenChars = Enumerable.Repeat(Constants.Misc.RandomStringSymbols, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray();
            return new string(chosenChars);
        }
    }
}