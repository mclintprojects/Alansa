using System;
using System.Collections.Generic;

namespace Alansa.Droid.Utils
{
    /// <summary>
    /// Provides useful static methods to be used when interacting with dates
    /// </summary>
    public static class BetterDate
    {
        private static List<string> shortMonths = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private static string[] months = months = App.CurrentActivity.Resources.GetStringArray(Resource.Array.months);

        /// <summary>
        /// Returns the name of the month in range 1 - 12. eg. 1 returns January
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNameOfMonth(int value)
        {
            if (value >= 1 && value <= 12)
                return months[value - 1];
            else
                throw new ArgumentOutOfRangeException("The month has to be between 1 and 12");
        }

        /// <summary>
        /// Returns the month index. eg January returns 1
        /// </summary>
        /// <param name="monthName">The name of the month to get the index of</param>
        /// <returns></returns>
        public static int GetNumberOfMonth(string monthName) => Array.IndexOf(months, monthName) + 1;

        /// <summary>
        /// Returns the numerical equivalent of the short form of the name of a month. For example: Jan returns 1, Mar returns 3
        /// </summary>
        /// <param name="shortMonthName">The short form of the name of the month you want the numerical equivalent of</param>
        /// <returns>numerical equivalent of the short form of the name of a month</returns>
        public static int GetNumberOfShortMonth(string shortMonthName) => shortMonths.IndexOf(shortMonthName) + 1;
    }
}