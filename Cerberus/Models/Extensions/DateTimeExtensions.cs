using System;

namespace Cerberus.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedString(this DateTime dateTime)
            => dateTime.Date == DateTime.Now.Date
                ? "Today"
                : dateTime.ToString(Constants.Misc.DateFormat);
    }
}