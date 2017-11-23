namespace Alansa.Droid.Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Try to parse the string into a value or return the default value
        /// </summary>
        /// <param name="me"></param>
        /// <param name="value">The string to convert</param>
        /// <param name="default">The default value to return incase the conversion fails</param>
        /// <returns></returns>
        public static int ParseOrDefault(this int me, string value, int @default = 0)
        {
            if (int.TryParse(value, out int result))
                return result;
            return @default;
        }

        /// <summary>
        /// Limits a integer to a certain range. If the integer is than the lowest limit, it's set to the lowest limit.
        /// Likewise for being higher than the higherLimit.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="lowerLimit">The lower limit of the limit range</param>
        /// <param name="higherLimit">The higher limit of the limit range</param>
        /// <returns></returns>
        public static int Limit(this int me, int lowerLimit = 0, int higherLimit = int.MaxValue)
        {
            if (me < lowerLimit)
                return lowerLimit;
            else if (me > higherLimit)
                return higherLimit;
            else
                return me;
        }

        /// <summary>
        /// Converts an integer like 1,000 to 1K, 10,000 to 10K and so on.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToShortCount(this int value)
        {
            if (value >= 1000 && value < 1000000)
                return $"{(value / 1000):n}K";
            else if (value >= 1000000 && value < 1000000000)
                return $"{(value / 1000000):n}M";
            else if (value >= 1000000000)
                return $"{(value / 1000000000):n}B";
            else
                return value.ToString();
        }

        /// <summary>
        /// Converts an decimal like 1,000 to 1K, 10,000 to 10K and so on.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToShortCount(this decimal value)
        {
            if (value >= 1000 && value < 1000000)
                return $"{value / 1000}K";
            else if (value >= 1000000)
                return $"{value / 1000000}M";
            else
                return value.ToString();
        }

        /// <summary>
        /// Converts an double like 1,000 to 1K, 10,000 to 10K and so on.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToShortCount(this double value)
        {
            if (value >= 1000 && value < 1000000)
                return $"{value / 1000}K";
            else if (value >= 1000000)
                return $"{value / 1000000}M";
            else
                return value.ToString();
        }
    }
}