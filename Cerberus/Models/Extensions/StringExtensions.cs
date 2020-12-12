using System.Web;
using Ganss.XSS;

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
        
        /// <summary>
        /// Removes all extra HTML tags and attributes (Note: this method allows class attribute to be set)
        /// </summary>
        /// <param name="value">Source string</param>
        /// <returns></returns>
        public static string SanitizeHTML(this string value)
        {
            var htmlSanitizer = new HtmlSanitizer();
            htmlSanitizer.AllowedAttributes.Add("class");
            return htmlSanitizer.Sanitize(value);
        }

        /// <summary>
        /// Completely removes all HTML tags while saving all inner content
        /// </summary>
        /// <param name="value">Source string</param>
        /// <returns></returns>
        public static string RemoveHTML(this string value)
        {
            var htmlSanitizer = new HtmlSanitizer
            {
                KeepChildNodes = true
            };
            htmlSanitizer.AllowedTags.Clear();
            return htmlSanitizer.Sanitize(value);
        }

        /// <summary>
        /// Completely removes all HTML tags while saving all inner content, unescapes HTML symbols and counts length
        /// </summary>
        /// <param name="value">Source string</param>
        /// <returns></returns>
        public static int GetPureTextLength(this string value)
        {
            return HttpUtility.HtmlDecode(value.RemoveHTML()).Length;
        }
    }
}