using System;

namespace Web.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedString(this DateTime dateTime)
            => dateTime.ToString(Constants.Misc.DateFormat);
    }
}