using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models.Helpers
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

        public static string GetCommaSpaceSeparatedString(params object[] args)
        {
            return args.Where(arg => arg != null)
                .Aggregate(string.Empty, (current, arg) => current.Length > 0
                    ? $"{current}, {arg}"
                    : $"{arg}");
        }
        
        public static string GetCommaSpaceSeparatedString(IEnumerable<object> args)
        {
            return GetCommaSpaceSeparatedString(args.ToArray());
        }
    }
}