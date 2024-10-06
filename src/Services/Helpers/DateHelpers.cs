using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
   public static class DateHelpers
    {
        public static DateTime SetMinYear(this DateTime date)
        {
            date = new DateTime(DateTime.MinValue.Year, date.Month, date.Day);
            return date ;
        }
    }
}
