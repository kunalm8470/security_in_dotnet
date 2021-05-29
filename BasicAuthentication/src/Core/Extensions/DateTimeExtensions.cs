using System;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToLocalFromUTC(this DateTime d, string timezoneId = "India Standard Time")
        {
            TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(d, info);
        }
    }
}
