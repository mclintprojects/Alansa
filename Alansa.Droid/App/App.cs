using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using GhalaniDroid.Activities;
using GhalaniDroid.API;
using GhalaniDroid.Extensions;
using GhalaniDroid.Utils;
using Plugin.CurrentActivity;
using Segment;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Android.App.Application;

namespace GhalaniDroid
{
    [Application]
    public class App : MultiDexApplication, IActivityLifecycleCallbacks
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

        public static float ConvertPixelsToDp(float px)
        {
            var metrics = _currentActivity.Resources.DisplayMetrics;
            float dp = px / ((float)metrics.DensityDpi / (float)DisplayMetricsDensity.Default);
            return dp;
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            _currentActivity = activity;
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
            ColorManager.ResetCount();
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            _currentActivity = activity;
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            _currentActivity = activity;
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        /// <summary>
        /// Invokes an action on the UI thread.
        /// </summary>
        /// <param name="action">The delegate to be invoked.</param>
        public static void Post(Action action) => _currentActivity.RunOnUiThread(() => action.Invoke());

        /// <summary>
        /// Starts an activity with push left in animation
        /// </summary>
        /// <param name="intent">The intent for the activity to start</param>
        public static new void StartActivity(Intent intent)
        {
            CurrentActivity.StartActivity(intent);
            CurrentActivity.OverridePendingTransition(Resource.Animation.push_left_in, Resource.Animation.push_left_out);
        }

        /// <summary>
        /// Logs the user out of the app.
        /// </summary>
        public static void Logout()
        {
            //If you do not reset this, logging out and back in as a different user will still use the old user's data
            GhalaniApi.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);
            File.Delete(Global.CRED_PATH);
            Global.CurrentOrganization = null;
            Global.Token = null;
            Global.ProfilingSelectedTabIndex = 0;
            Global.FinanceSelectedTabIndex = 0;
            Global.HarvestSelectedTabIndex = 0;
            Global.ProfileStats = null;
            Global.CachedCredentials = null;

            // Create cache folder in preparation for a new login. Fixes part of URI not found exception
            Directory.CreateDirectory(Global.CACHE_PATH);
            File.Create(Path.Combine(Global.CACHE_PATH, "null")).Close();
            var intent = new Intent(_currentActivity, typeof(LoginActivity));
            intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            _currentActivity.StartActivity(intent);
        }

        public override async void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);

            // Creates app cache folder if it doesn't already exist
            if (!Directory.Exists(Global.CACHE_PATH))
                Directory.CreateDirectory(Global.CACHE_PATH);

            Task.Run(() => CheckForAppUpdate());
            Task.Run(() => DownloadTutorials());

            // Setup segment analytics
            await Task.Run(() =>
            {
                Analytics.Initialize(AppResources.SegmentKey);
                Analytics.SetInDebugMode(false);

                // Sets colors from assets for use everywhere in application
                ColorManager.Initialize(Assets);
                if (!TutorialManager.Initialized)
                    TutorialManager.Initialize(Assets);
            });
        }

        // Downloads the latest version of the target tutorials from the Firebase DB
        private async void DownloadTutorials()
        {
            var targetTutPath = Path.Combine(Global.APP_PATH, "targetTutorial.json");
            var targetTutorial = await GhalaniApi.DownloadTargetTutorialAsync();
            DBAssist.SerializeDBAsync(targetTutPath, targetTutorial);
        }

        // Checks if an update is available for download and take them to the PlayStore if they choose to get the latest version
        private async Task CheckForAppUpdate()
        {
            var packageInfo = PackageManager.GetPackageInfo(PackageName, 0);
            var versionCode = packageInfo.VersionCode;

            var updateAvailable = await GhalaniApi.IsAppUpdateAvailable(versionCode);
            if (updateAvailable)
                ShowUpdateAvailableDialog();
        }

        private void ShowUpdateAvailableDialog()
        {
            CurrentActivity.ShowNegativeDialog("Update available", "There's a newer version of Ghalani available. Update?",
                         "Update", "Dismiss", () =>
                         {
                             try
                             {
                                 _currentActivity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse($"market://details?id={PackageName}")));
                             }
                             catch (ActivityNotFoundException)
                             {
                                 _currentActivity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id={PackageName}")));
                             }
                         }, null);
        }
    }
}