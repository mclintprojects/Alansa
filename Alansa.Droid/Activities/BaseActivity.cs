using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Alansa.Droid.Activities
{
    /// <summary>
    /// The base activity that all activities should derive from.
    /// </summary>
    public abstract class BaseActivity : AppCompatActivity
    {
        /// <summary>
        /// The layout resource ID for the activity. Will be used during view inflation
        /// </summary>
        public abstract int LayoutResource { get; }

        /// <summary>
        /// Do you want to show a back arrow button in the activity toolbar?
        /// </summary>
        public abstract bool HomeAsUpEnabled { get; }

        /// <summary>
        /// Bool that is used to know if the current activity's toolbar should have an elevation or not.
        /// </summary>
        protected virtual bool ToolbarNoElevation => false;

        /// <summary>
        /// Some activities won't have a toolbar and will set this to false to let us know not to inflate a toolbar.
        /// </summary>
        protected virtual bool SetupToolbar => true;

        private Toolbar toolbar;

        public Toolbar Toolbar => toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResource);
            if (SetupToolbar)
            {
                toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (ToolbarNoElevation && Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
                    toolbar.Elevation = 0;
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(HomeAsUpEnabled);
            }
        }

        /// <summary>
        /// A method that is called when the back arrow in an activity's toolbar is pressed
        /// </summary>
        public virtual void OnBackArrowPressed() => NavigateAway();

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackArrowPressed();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Finishes the current activity and animates the transition to the previous activity.
        /// </summary>
        public virtual void NavigateAway()
        {
            Finish();
            OverridePendingTransition(Resource.Animation.push_right_in, Resource.Animation.push_right_out);
        }

        public override void OnBackPressed() => NavigateAway();
    }
}