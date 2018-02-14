using Alansa.Droid.Enums;
using Alansa.Droid.Utils;
using Java.Util;
using System;

namespace Alansa.Droid.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a DateTime object to a time string like this "2017-08-01T12:12:00.234Z"
        /// </summary>
        /// <param name="time">the DateTime object to be converted</param>
        /// <returns>string</returns>
        public static string ToServerTime(this DateTime time) => $"{time.Year}-{time.Month:d2}-{time.Day:d2}T{time.Hour:d2}:{time.Minute:d2}:{time.Second:d2}.{time.Millisecond}Z";

        /// <summary>
        /// Tells you how long since an item was added. eg "Added 1 day ago"
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToReadableTimeStamp(this DateTime time)
        {
            var days = time - DateTime.Now;
            if (days.Days == 0)
                return "Added today.";
            else if (days.Days == -1)
                return "Added yesterday";
            else
            {
                var addedDays = Math.Abs(days.Days);
                return $"Added {addedDays} " + (addedDays == 1 ? "day" : "days") + " ago";
            }
        }

        /// <summary>
        /// The JAVA equivalent of Calendar.getTimeInMillis for use with the compactcalendarview
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeInMillis(this DateTime time) => (long)(time.ToUniversalTime() - Jan1st1970).TotalMilliseconds;

        /// <summary>
        /// Gets the day from a Java date object
        /// </summary>
        /// <param name="date"></param>
        public static int GetDay(this Date date) => 0.ParseOrDefault(date.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2]);

        /// <summary>
        /// Converts a string to a DateTime object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="delimiter">The delimiter of the date string. Eg: '-', '/' etc </param>
        /// <param name="dateFormat">The format of the date string.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, DateFormat dateFormat = DateFormat.YYYYMMDD, char delimiter = '-')
        {
            if (value != null && value.Length >= 8)
            {
                int[] splitDate;
                switch (dateFormat)
                {
                    case DateFormat.YYYYMMDD:
                        var splitString = value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        splitDate = splitString.ToIntArray();
                        return new DateTime(splitDate[0], splitDate[1], splitDate[2]);

                    case DateFormat.MMMDDYYYY:
                        var dateSplit = value.Replace(",", string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        return new DateTime(int.Parse(dateSplit[2]), BetterDate.GetNumberOfShortMonth(dateSplit[0]), int.Parse(dateSplit[1]));

                    default:
                        return new DateTime();
                }
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Gets the time in a format like Jan 12, 2017
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToPrettyDate(this DateTime time) => $"{BetterDate.GetNameOfMonth(time.Month).Substring(0, 3)} {time.Day}, {time.Year}";

        public static int[] ToIntArray(this String[] value)
        {
            try
            {
                var returnArray = new int[value.Length];
                for (int i = 0; i < value.Length; i++)
                    returnArray[i] = Convert.ToInt32(value[i]);

                return returnArray;
            }
            catch { return new int[] { }; }
        }

        /// <summary>
        /// Changes a server time string eg. 2017-08-01T12:12:00.234Z to a normal date string like 01-08-17
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this string dateString) => string.IsNullOrEmpty(dateString) || dateString.Length < 10 ? "N/A" : dateString?.Substring(0, 10);
    }
}