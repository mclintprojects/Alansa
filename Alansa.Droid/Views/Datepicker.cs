using Android.App;
using Android.Content;
using Android.Runtime;
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
            get => selectedDate.ToBackendDateString();
            set
            {
                try
                {
                    // Normally the text set to would be in the format Jan, 12 2017. The code below changes that string to a datetime object
                    selectedDate = value.ToDateTime(DateFormat.MMMDDYYYY);
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
                dateTb.Text = e.Date.ToShortDate();
                selectedDate = e.Date;
                OnDateSelected?.Invoke();
            }, date.Year, date.Month - 1, date.Day);
            dateDialog.Show();
        }
    }
}