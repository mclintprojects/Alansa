using Alansa.Droid.Enums;
using Alansa.Droid.Extensions;
using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace Alansa.Droid.Views
{
    internal class Datepicker : LinearLayout
    {
        private EditText dateTb;
        private TextInputLayout dateInputLayout;
        private string hint;
        private DateTime selectedDate = DateTime.Now;

        public string Text
        {
            get => GetDateString(selectedDate);
            set
            {
                try
                {
                    // Normally the text set to would be in the format Jan, 12 2017. The code below changes that string to a datetime object
                    selectedDate = ConvertToDateTime(value, DateFormat.MMMDDYYYY);
                    dateTb.Text = value;
                }
                catch
                {
                    dateTb.Text = value;
                }
            }
        }

        /// <summary>
        /// The date selected by the user
        /// </summary>
        public DateTime SelectedDate => selectedDate;

        public string Hint
        {
            get => hint;
            set

            {
                hint = value;
                dateInputLayout.Hint = hint;
            }
        }

        private bool reactsToFocus = true;

        /// <summary>
        /// Sets whether the Datepicker should pop up the DatePickerDialog when the view is focused on
        /// </summary>
		public bool ReactsToFocus
        {
            set => reactsToFocus = value;
        }

        public Action OnDateSelected;

        public EditText InputLayout => dateTb;

        protected Datepicker(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public Datepicker(Context context) : base(context)
        {
            Init(context);
        }

        public Datepicker(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        private void Init(Context context, IAttributeSet attrs = null)
        {
            var view = LayoutInflater.From(context).Inflate(Resource.Layout.layout_datepicker, this);
            dateTb = view.FindViewById<EditText>(Resource.Id.dateEditText);
            dateInputLayout = FindViewById<TextInputLayout>(Resource.Id.dateInputLayout);

            if (attrs != null)
            {
                var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.Datepicker, 0, 0);
                hint = array.GetString(Resource.Styleable.Datepicker_picker_hint);

                dateInputLayout.Hint = hint;

                array.Recycle();
            }

            dateTb.FocusChange += (s, e) =>
            {
                if (e.HasFocus && reactsToFocus)
                    ShowDateDialog(context);
            };

            dateTb.Click += delegate { ShowDateDialog(context); };
        }

        private void ShowDateDialog(Context ctx)
        {
            DateTime date = DateTime.Now;
            if (!string.IsNullOrEmpty(dateTb.Text))
                date = selectedDate;

            var dateDialog = new DatePickerDialog(ctx, (sender, e) =>
            {
                dateTb.Text = GetDateString(e.Date);
                selectedDate = e.Date;
                OnDateSelected?.Invoke();
            }, date.Year, date.Month - 1, date.Day);
            dateDialog.Show();
        }

        private string GetDateString(DateTime time) => $"{time.Year}-{time.Month}-{time.Day}";

        private DateTime ConvertToDateTime(string value, DateFormat dateFormat = DateFormat.YYYYMMDD, char delimiter = '-')
        {
            if (value != null && value.Length >= 8)
            {
                int[] splitDate;
                switch (dateFormat)
                {
                    case DateFormat.YYYYMMDD:
                        var splitString = value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        splitDate = splitString.ToIntArray();
                        return new DateTime(splitDate[0], splitDate[1], splitDate[2]);

                    case DateFormat.MMMDDYYYY:
                        var dateSplit = value.Replace(",", string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        return new DateTime(int.Parse(dateSplit[2]), BetterDate.GetNumberOfShortMonth(dateSplit[0]), int.Parse(dateSplit[1]));

                    default:
                        return new DateTime();
                }
            }

            return DateTime.Now;
        }
    }
}