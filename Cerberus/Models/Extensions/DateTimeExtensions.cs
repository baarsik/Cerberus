using System;

namespace Cerberus.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedString(this DateTime dateTime)
            => dateTime.Date == DateTime.Today
                ? "Today"
                : dateTime.Date == DateTime.Today - TimeSpan.FromDays(1)
                    ? "Yesterday"
                    : dateTime.ToString(Constants.Misc.DateFormat);
    }
}