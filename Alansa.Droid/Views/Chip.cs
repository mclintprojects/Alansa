using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Widget;
using System;

namespace Alansa.Droid.Views
{
    internal class Chip : TextView
    {
        public Chip(Context context) : base(context)
        {
        }

        public Chip(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public Chip(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public Chip(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        protected Chip(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            if (attrs != null)
            {
                var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.Chip, 0, 0);
                Text = array.GetString(Resource.Styleable.Chip_chipText);
                var chipBgColor = array.GetColor(Resource.Styleable.Chip_chipColor, Color.Gray);

                array.Recycle();

                SetTextColor(Color.White);
                Background = ContextCompat.GetDrawable(context, Resource.Drawable.chip_bg);
                Background.SetColorFilter(chipBgColor, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}