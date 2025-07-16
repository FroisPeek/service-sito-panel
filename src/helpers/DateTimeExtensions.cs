using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime NowInBrasilia(this DateTime dateTime)
        {
            var brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, brasiliaTimeZone);
        }
    }
}