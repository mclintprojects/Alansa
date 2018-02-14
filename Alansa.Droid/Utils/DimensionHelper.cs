using Android.Util;

namespace Alansa.Droid.Utils
{
    public static class DimensionHelper
    {
        public static float DpToPx(float dpValue)
        {
            var metrics = App.CurrentActivity.Resources.DisplayMetrics;
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, metrics);
        }

        public static float SpToPx(float spValue)
        {
            var metrics = App.CurrentActivity.Resources.DisplayMetrics;
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Sp, spValue, metrics);
        }
    }
}