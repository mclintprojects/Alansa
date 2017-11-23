using Newtonsoft.Json;

namespace Alansa.Droid.Extensions
{
    internal static class JsonExtensions
    {
        /// <summary>
        /// Deserialize a string to typeof(T)
        /// </summary>
        /// <typeparam name="T">The type to deserialize the string to</typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string me)
        {
            if (me != null)
                return JsonConvert.DeserializeObject<T>(me);

            return default(T);
        }

        /// <summary>
        /// Serializes an object to a json string
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToJson(this object me) => me == null ? string.Empty : JsonConvert.SerializeObject(me);
    }
}