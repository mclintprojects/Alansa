using System;
using System.Text;

namespace Alansa.Droid.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if this string contains another string, with no respect to casing.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="query">The string being searched for</param>
        /// <param name="placeholder">Just a placeholder. Just allows this method as an overload since String.Contains already exists</param>
        /// <returns></returns>
        public static bool Contains(this String value, string query, bool placeholder = false) => value.ToLower().Contains(query.ToLower());

        /// <summary>
        /// Capitalizes every word in a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Capitalize(this String value)
        {
            if (value != null)
            {
                StringBuilder sentence = new StringBuilder();
                string oneString;
                var splitStrings = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in splitStrings)
                {
                    oneString = Char.ToUpper(s[0]) + s.Substring(1, s.Length - 1).ToLower();
                    sentence.Append($"{oneString} ");
                }
                return sentence.ToString();
            }
            return "";
        }

        /// <summary>
        /// Capitalizes every word up to the end index specified
        /// </summary>
        /// <param name="value"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string CapitalizeUpTo(this String value, int endIndex)
        {
            if (value != null)
            {
                StringBuilder sentence = new StringBuilder();
                string oneString;
                var splitStrings = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < splitStrings.Length; i++)
                {
                    if (i < endIndex)
                    {
                        oneString = splitStrings[i];
                        sentence.Append($"{oneString.Capitalize()} ");
                    }
                    else
                        sentence.Append($"{splitStrings[i]} ");
                }
                return sentence.ToString();
            }
            return "";
        }

        /// <summary>
        /// Gets the acronym of a string. Example: Kofi Boahene becomes KB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAcronym(this string name, int length = 2, int startIndex = 0)
        {
            if (name != null)
            {
                var split = name.Capitalize().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 1)
                    return split[0][0].ToString().ToUpper();
                else
                {
                    var payload = new StringBuilder();
                    for (int i = startIndex; i < length; i++)
                        payload.Append(split[i][0].ToString().ToUpper());

                    return payload.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Used with a StringBuilder object when you want to append a line of text and an empty line below it
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="text">The text to append and then append an empty line below</param>
        public static void AppendLineAndNewLine(this StringBuilder builder, string text) => builder.AppendLine($"{text}\r\n");
    }
}