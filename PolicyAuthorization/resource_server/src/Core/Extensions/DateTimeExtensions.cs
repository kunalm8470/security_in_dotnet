using System;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this string s)
        {
            DateTime d = Convert.ToDateTime(s);

            int calculatedAge = DateTime.Today.Year - d.Year;
            if (d > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            return calculatedAge;
        }
    }
}
