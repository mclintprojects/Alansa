using Alansa.Droid.Utils;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using System;

namespace Alansa.Droid.Views
{
    public class IconView : View
    {
        protected readonly int iconLabelBottomMargin = (int)DimensionHelper.DpToPx(8);
        protected Drawable _icon;
        protected string _labelText;
        protected bool _showIconLabel;
        protected Color iconBackgroundColor, iconLabelTextColor;
        protected Paint iconBackgroundPaint, iconLabelTextColorPaint;
        protected float iconLabelTextSize = DimensionHelper.SpToPx(13);
        protected int iconMargins, iconLabelTextOffset = 0, iconLabelTopMargin = 0;
        private const int TEXTSIZESCALEFACTOR = 9, ICONMARGINSCALEFACTOR = 6;
        private float viewWidth = DimensionHelper.DpToPx(144), viewHeight = DimensionHelper.DpToPx(144), backgroundCircleRadius;

        /// <summary>
        /// The icon to show in the icon view.
        /// </summary>
        public Drawable Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The text to show as the icon view's label.
        /// </summary>
        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Show or hide a label for the icon view.
        /// </summary>
        public bool ShowIconLabel
        {
            get => _showIconLabel;
            set
            {
                _showIconLabel = value;
                Invalidate();
            }
        }

        public IconView(Context context) : base(context)
        {
            Initialize(context);
        }

        public IconView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs);
        }

        protected IconView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (viewWidth == 0 || viewHeight == 0)
                return;

            if (_showIconLabel)
                DrawIconLabel(canvas);

            DrawBackgroundCircle(canvas);

            // Draws icon/image inside background circle
            if (_icon != null)
                DrawIcon(canvas);
        }

        protected virtual void DrawBackgroundCircle(Canvas canvas)
        {
            canvas.DrawCircle(viewWidth / 2, (viewWidth / 2) - iconLabelTopMargin, backgroundCircleRadius, iconBackgroundPaint);
        }

        protected virtual void DrawIcon(Canvas canvas)
        {
            var leftBounds = iconMargins + iconLabelTextOffset;
            var topBounds = iconMargins + iconLabelTextOffset - iconLabelTopMargin;
            var rightBounds = (int)(viewWidth - iconMargins - iconLabelTextOffset);
            var bottomBounds = (int)(viewHeight - iconMargins - iconLabelTextOffset - iconLabelTopMargin);

            _icon.SetBounds(leftBounds, topBounds, rightBounds, bottomBounds);
            _icon.Draw(canvas);
        }

        protected virtual void DrawIconLabel(Canvas canvas)
        {
            iconLabelTopMargin = (int)DimensionHelper.DpToPx(16);
            iconLabelTextOffset = (int)(iconLabelTextSize + iconLabelTopMargin + iconLabelBottomMargin);

            canvas.DrawText(_labelText, viewWidth / 2, viewHeight - iconLabelBottomMargin, iconLabelTextColorPaint);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            SetOptimumWidth(MeasureSpec.GetMode(widthMeasureSpec), widthMeasureSpec);
            SetOptimumHeight(MeasureSpec.GetMode(heightMeasureSpec), heightMeasureSpec);

            backgroundCircleRadius = (viewWidth / 2f) - (iconLabelTextOffset / 2);
            iconMargins = (int)(backgroundCircleRadius * 2) / ICONMARGINSCALEFACTOR;
            iconLabelTextSize = viewWidth / TEXTSIZESCALEFACTOR;
            iconLabelTextColorPaint.TextSize = iconLabelTextSize;

            SetMeasuredDimension((int)viewWidth, (int)viewHeight);
        }

        private void Initialize(Context context, IAttributeSet attrs = null)
        {
            if (attrs != null)
            {
                var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.IconView, 0, 0);

                iconBackgroundColor = array.GetColor(Resource.Styleable.IconView_bg_color, Color.Gray);
                iconLabelTextColor = array.GetColor(Resource.Styleable.IconView_iconLabelTextColor, Color.ParseColor("#D9000000"));
                _labelText = array.GetString(Resource.Styleable.IconView_iconLabelText);
                _showIconLabel = array.GetBoolean(Resource.Styleable.IconView_showIconLabel, false);

                var iconResId = array.GetResourceId(Resource.Styleable.IconView_src, 0);
                if (iconResId != 0)
                    _icon = AppCompatDrawableManager.Get().GetDrawable(context, iconResId);

                if (_labelText != null)
                    _showIconLabel = true;

                array.Recycle();
            }

            iconBackgroundPaint = new Paint(PaintFlags.AntiAlias) { Color = iconBackgroundColor };
            iconLabelTextColorPaint = new Paint(PaintFlags.AntiAlias)
            {
                Color = iconLabelTextColor,
                TextSize = iconLabelTextSize,
                TextAlign = Paint.Align.Center
            };
        }

        private void SetOptimumHeight(MeasureSpecMode heightMeasureSpecMode, int heightMeasureSpec)
        {
            var height = MeasureSpec.GetSize(heightMeasureSpec);

            switch (heightMeasureSpecMode)
            {
                case MeasureSpecMode.Exactly:
                    viewHeight = height;
                    viewWidth = height;
                    break;
            }
        }

        private void SetOptimumWidth(MeasureSpecMode widthMeasureSpecMode, int widthMeasureSpec)
        {
            var width = MeasureSpec.GetSize(widthMeasureSpec);

            switch (widthMeasureSpecMode)
            {
                case MeasureSpecMode.Exactly:
                    viewWidth = width;
                    viewHeight = width;
                    break;
            }
        }
    }
}