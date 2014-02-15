using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure
{
    public static class DateTimeExtension
    {
        public static List<int> GetLastSixMonths(this DateTime date, int noofmonth)
        {
            var month = date.Month;
            var monthList = new List<int>();
            for (int i = 1; i <= noofmonth; i++)
            {
                if (month > 12)
                    month = 1;
                monthList.Add(month);
                month++;
            }
            return monthList;
        }
        public static string ReturnTimeString(this DateTime time)
        {
            string strTime = string.Empty;
            TimeSpan span = DateTime.Now - time;
            if (span.TotalHours > 12)
                strTime = time.ToString("dd/MMM/yyyy h:mm tt");
            else if (span.TotalHours >= 1)
                strTime = span.Hours.ToString() + " hours ago";
            else if (span.TotalMinutes >= 1)
                strTime = span.Minutes.ToString() + " minutes ago";
            else
                strTime = "Just now";
            return strTime;
        }
    }
}
