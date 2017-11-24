using System.Runtime.CompilerServices;

namespace Alansa.Droid.Utils
{
    public static class Logger
    {
        private static readonly string TAG = "ALANSA-DEBUG";

        public static void Log(string message, [CallerMemberName] string caller = "") => Android.Util.Log.Debug($"{TAG}-{caller}", message);
    }
}