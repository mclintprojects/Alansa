using Android.Graphics;
using Android.Graphics.Drawables;
using System;
using System.Threading.Tasks;

namespace Alansa.Droid.Utils
{
    internal static class BitmapManager
    {
        public static Bitmap Resize(Bitmap bitmap, int startSize, bool startSizeIsWidth = true)
        {
            var dimens = GetResizeDimensions(startSize, startSizeIsWidth, bitmap);
            return Bitmap.CreateScaledBitmap(bitmap, (int)dimens.Width, (int)dimens.Height, true);
        }

        private static (float Width, float Height) GetResizeDimensions(float startSize, bool startSizeIsWidth, Bitmap bitmap)
        {
            var scale = (float)bitmap.Width / (float)bitmap.Height;
            if (startSizeIsWidth) return (startSize, startSize / scale);
            else return (startSize * scale, startSize);
        }

        public static async Task<Bitmap> GetBitmapAtFilePathAsync(string path)
        {
            var options = new BitmapFactory.Options();
            return await BitmapFactory.DecodeFileAsync(path, options);
        }

        internal static Drawable GetAssetsImage(string filename)
        {
            try
            {
                var imageStream = App.CurrentActivity.Assets.Open(filename);
                using (imageStream)
                {
                    if (imageStream == null)
                        return null;
                    else
                        return Drawable.CreateFromStream(imageStream, null);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
                return null;
            }
        }
    }
}