namespace Cerberus.Models.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Trims and removes double spacing
        /// </summary>
        /// <param name="value">Source string</param>
        public static string FixSpacing(this string value)
        {
            value = value.Trim();
            while (value.Contains("  "))
                value = value.Replace("  ", " ");

            return value;
        }
    }
}