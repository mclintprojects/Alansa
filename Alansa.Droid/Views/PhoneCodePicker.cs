using Alansa.Droid.Dialogs;
using Alansa.Droid.Models;
using Alansa.Droid.Utils;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alansa.Droid.Views
{
    internal class PhoneCodePicker : LinearLayout
    {
        private TextView countryLbl;
        private EditText phoneNumberTb;
        private ImageView flagHolder;
        private IEnumerable<Country> countries;
        private LinearLayout kitkatView;
        private bool userDeviceIsKitkatOrBelow;
        private string selectedPhoneCode;
        public string SelectedPhoneCode => selectedPhoneCode;

        public EditText InputLayout => phoneNumberTb;

        public string PhoneNumber => GetPhoneNumber();

        public PhoneCodePicker(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public PhoneCodePicker(Context ctx) : base(ctx)
        {
            Init(ctx);
        }

        public PhoneCodePicker(Context ctx, IAttributeSet attrs) : base(ctx, attrs)
        {
            Init(ctx, attrs);
        }

        private void Init(Context ctx, IAttributeSet attrs = null)
        {
            var view = LayoutInflater.From(ctx).Inflate(Resource.Layout.layout_phonecode_picker, this);
            countryLbl = view.FindViewById<TextView>(Resource.Id.countryLbl);
            phoneNumberTb = view.FindViewById<EditText>(Resource.Id.phoneNumberTb);
            var phoneTIL = view.FindViewById<TextInputLayout>(Resource.Id.phoneNumberTIL);
            flagHolder = view.FindViewById<ImageView>(Resource.Id.flagHolder);
            kitkatView = FindViewById<LinearLayout>(Resource.Id.kitkatView);

            var array = ctx.ObtainStyledAttributes(attrs, Resource.Styleable.PhoneCodePicker, 0, 0);
            var hint = array.GetString(Resource.Styleable.PhoneCodePicker_phonePickerHint);
            var defaultCountry = array.GetString(Resource.Styleable.PhoneCodePicker_phonePickerDefaultCountry);

            GetListOfCountriesAndSetDefaultCountry(ctx, defaultCountry);
            countryLbl.Click += CountryLbl_Click;
            kitkatView.Click += CountryLbl_Click;

            array.Recycle();

            phoneTIL.Hint = hint;
            countryLbl.SetCompoundDrawablesWithIntrinsicBounds(null, null, AppCompatDrawableManager.Get().GetDrawable(ctx, Resource.Drawable.ic_arrow_drop_down_black_24dp), null); // Set down arrow button

            if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
                userDeviceIsKitkatOrBelow = true;
        }

        private void CountryLbl_Click(object sender = null, EventArgs e = null)
        {
            var dialog = new CountryPickerDialog(countries);
            dialog.OnDatumSelected += (index, country) =>
            {
                SetSelectedCountry((country as Country).alpha3);
            };

            dialog.Show(App.CurrentActivity.FragmentManager, string.Empty);
        }

        private async void GetListOfCountriesAndSetDefaultCountry(Context ctx, string defaultCountry)
        {
            // Countries json gotten from here: https://github.com/OpenBookPrices/country-data/blob/master/data/countries.json
            using (var reader = new StreamReader(ctx.Assets.Open("countries.json")))
            {
                var countriesJson = await reader.ReadToEndAsync();
                countries = JsonConvert.DeserializeObject<List<Country>>(countriesJson);
                SetSelectedCountry(defaultCountry);
            }
        }

        private void SetSelectedCountry(string alpha3) // Alpha 3 is the 3 digit form of country code eg: USA, GHA, NIG
        {
            var validCountry = countries.FirstOrDefault(x => x.alpha3 == alpha3);
            SetCountryText(validCountry);
        }

        private void SetCountryText(Country validCountry)
        {
            if (validCountry == null)
                countryLbl.Text = "N/A";
            else
            {
                if (!userDeviceIsKitkatOrBelow)
                    countryLbl.Text = $"{validCountry.emoji}";
                else
                {
                    countryLbl.Visibility = ViewStates.Gone;
                    kitkatView.Visibility = ViewStates.Visible;
                    flagHolder.SetImageDrawable(BitmapManager.GetAssetsImage($"flags/{validCountry.alpha2.ToLower()}.png"));
                }
                selectedPhoneCode = validCountry.countryCallingCodes.Count > 0 ? validCountry.countryCallingCodes[0] : string.Empty;
            }
        }

        private string GetPhoneNumber() => $"{Sanitize(selectedPhoneCode)}{Sanitize(phoneNumberTb.Text)}";

        private string Sanitize(string phoneNumber)
        {
            var cleanPhoneNumber = phoneNumber.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty);
            if (cleanPhoneNumber.StartsWith("0")) // For backend we don't want the zero infront
                return cleanPhoneNumber.Remove(0, 1);
            else
                return cleanPhoneNumber;
        }
    }
}