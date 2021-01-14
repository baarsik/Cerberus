using System.Text;
using AdvancedStringBuilder;

namespace Web.Models.Extensions
{
    public static class IntExtensions
    {
        public static string ToStringDisplayFormat(this int value)
        {
            var sb = new StringBuilder();
            while (value > 0)
            {
                sb.Insert(0, value < 1000 ? $" {value % 1000}" : $" {value % 1000:000}");
                value /= 1000;
            }
            return sb.TrimStart().ToString();
        }
    }
}