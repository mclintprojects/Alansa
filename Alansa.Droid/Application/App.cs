using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static Android.App.Application;

namespace Alansa.Droid
{
    /// <summary>
    /// The base app class. This class comes with a lot of handy functionality. It is ideal that you inherit from it.
    /// </summary>
    public class App : Application, IActivityLifecycleCallbacks
    {
        private static Activity _currentActivity;
        public static Activity CurrentActivity => _currentActivity;

        /// <summary>
        /// Checks if there's is an internet connection ie. app is offline
        /// </summary>
        public static bool IsOffline
        {
            get
            {
                var connected = ConnectivityManager.FromContext(Context).ActiveNetworkInfo?.IsConnected;
                if (connected == null)
                    return true;
                else
                    return !connected.Value && !IsHostReachable().Result;
            }
        }

        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public static float ConvertPixelsToDp(float px)
        {
            var metrics = _currentActivity.Resources.DisplayMetrics;
            float dp = px / ((float)metrics.DensityDpi / (float)DisplayMetricsDensity.Default);
            return dp;
        }

        /// <summary>
        /// Invokes an action on the UI thread.
        /// </summary>
        /// <param name="action">The delegate to be invoked.</param>
        public static void Post(Action action) => _currentActivity.RunOnUiThread(() => action.Invoke());

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            _currentActivity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            _currentActivity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            _currentActivity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override async void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        private static async Task<bool> IsHostReachable()
        {
            try
            {
                var client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(2500) };
                var response = await client.GetAsync("http://www.google.com");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}