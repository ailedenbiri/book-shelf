using System;
namespace Exoa.Utils
{
    public static class Date
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static long UnixTime
        {
            get
            {
                return Date.DateTime2UnixTime(DateTime.UtcNow);
            }
        }
        public static long UnixTimeMilliSeconds
        {
            get
            {
                return Date.DateTime2UnixTimeMilliSeconds(DateTime.UtcNow);
            }
        }
        public static DateTime UnixTime2DateTime(long unixTime)
        {
            return Date.UnixEpoch.AddSeconds((double)unixTime);
        }
        public static DateTime UnixTime2DateTimeMilliSeconds(long unixMilliSeconds)
        {
            return Date.UnixEpoch.AddMilliseconds((double)unixMilliSeconds);
        }
        public static long DateTime2UnixTime(DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();
            return (long)dateTime.Subtract(Date.UnixEpoch).TotalSeconds;
        }
        public static long DateTime2UnixTimeMilliSeconds(DateTime dateTime)
        {
            return Date.DateTime2UnixTime(dateTime) * 1000L + (long)dateTime.Millisecond;
        }
        public static int GetDayNumber()
        {
            return Date.GetDayNumber(DateTime.UtcNow);
        }
        public static int GetDayNumber(DateTime dateTime)
        {
            return (int)System.Math.Floor((double)((float)Date.DateTime2UnixTime(dateTime) / 86400f));
        }
        public static int GetDays(DateTime endDate)
        {
            return Date.GetDays(DateTime.UtcNow, endDate);
        }
        public static int GetDays(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }
        public static int GetWeekNumber(DayOfWeek startDay = DayOfWeek.Sunday)
        {
            return Date.GetWeekNumber(DateTime.UtcNow, startDay);
        }
        public static int GetWeekNumber(DateTime dateTime, DayOfWeek startDay = DayOfWeek.Sunday)
        {
            return (int)System.Math.Floor((double)((float)Date.DateTime2UnixTime(dateTime.AddDays((double)Date.DayOfAdjust(startDay - dateTime.DayOfWeek))) / 604800f));
        }
        public static DateTime GetDayOfThisWeek(DayOfWeek startDay = DayOfWeek.Sunday)
        {
            return Date.GetDayOfThisWeek(DateTime.UtcNow, startDay);
        }
        public static DateTime GetDayOfThisWeek(DateTime dateTime, DayOfWeek startDay = DayOfWeek.Sunday)
        {
            return dateTime.AddDays((double)Date.DayOfAdjust(startDay - dateTime.DayOfWeek));
        }
        public static DateTime GetDayOfThisMonth(int startDay = 1)
        {
            return Date.GetDayOfThisMonth(DateTime.UtcNow, startDay);
        }
        public static DateTime GetDayOfThisMonth(DateTime dateTime, int startDay = 1)
        {
            if (dateTime.Day < startDay)
            {
                DateTime dateTime2 = dateTime.AddMonths(-1);
                return new DateTime(dateTime2.Year, dateTime2.Month, startDay);
            }
            return new DateTime(dateTime.Year, dateTime.Month, startDay);
        }
        private static int DayOfAdjust(int diff)
        {
            if (diff > 0)
            {
                diff -= 7;
            }
            return diff;
        }
        public static int GetMonthNumber(int startDay = 1)
        {
            if (startDay < 1)
            {
                startDay = 1;
            }
            if (startDay > 31)
            {
                startDay = 31;
            }
            return Date.GetMonthNumber(DateTime.UtcNow, startDay);
        }
        public static int GetMonthNumber(DateTime dateTime, int startDay = 1)
        {
            dateTime = dateTime.AddDays((double)(startDay - 1));
            return (dateTime.Year - 1970) * 12 + dateTime.Month;
        }
        public static DateTime MonthNumber2DateTime(int monthNumber, bool atLastDay = false)
        {
            if (monthNumber < 0)
            {
                monthNumber = 0;
            }
            int num = (int)System.Math.Floor((double)((float)monthNumber / 12f)) + 1970;
            int month = monthNumber - (num - 1970) * 12;
            if (atLastDay)
            {
                return new DateTime(num, month, DateTime.DaysInMonth(num, month));
            }
            return new DateTime(num, month, 1);
        }
    }
}
