using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Alansa.Droid.Utils
{
    internal static class EmptyStateManager
    {
        /// <summary>
        /// Use when emptystate view is inflated from empty_state_2.xml in the app's layout resources folder
        /// </summary>
        /// <param name="emptyState">The emptyState view</param>
        /// <param name="iconResId">The resource id for the icon to be used in the empty state. It should be a vector drawable</param>
        /// <param name="emptyStateText">The text to set for the empty state</param>
        public static void SetEmptyState(View emptyState, int iconResId, string emptyStateText)
        {
            var iconHolder = emptyState.FindViewById<ImageView>(Resource.Id.emptyIcon);
            var emptyStateTextLbl = emptyState.FindViewById<TextView>(Resource.Id.infoText);

            iconHolder.SetImageDrawable(AppCompatDrawableManager.Get().GetDrawable(App.CurrentActivity, iconResId));
            emptyStateTextLbl.Text = emptyStateText;
        }
    }
}