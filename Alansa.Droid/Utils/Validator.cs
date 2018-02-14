using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Widget;
using System;
using System.Text.RegularExpressions;

namespace Alansa.Droid.Utils
{
    /// <summary>
    /// Util that simplifies validating entry fields
    /// </summary>
    public class Validator : IDisposable
    {
        private bool passed = true, alreadyFailed;
        public bool PassedValidation => passed;
        private readonly Drawable errorIcon;

        public Validator()
        {
            errorIcon = ContextCompat.GetDrawable(App.CurrentActivity, Resource.Drawable.error);
        }

        /// <summary>
        /// Validates if the text in the entry field is a valid phone number
        /// </summary>
        /// <param name="editText"></param>
        public void ValidatePhoneNumber(EditText editText)
        {
            if (!alreadyFailed)
            {
                if (editText.Text.Length < 6 || editText.Text.Length > 15)
                {
                    passed = Regex.Match(editText.Text, @"^(\+[0-9]{9})$").Success;
                    alreadyFailed = true;
                    string response = editText.Text.Length == 0 ? "Phone number is empty." : "Invalid phone number.";
                    editText.SetError(response, errorIcon);
                    editText.RequestFocus();
                }
                else
                    passed = true;
            }
        }

        /// <summary>
        /// Validates that an entry field is not empty
        /// </summary>
        /// <param name="editText"></param>
        /// <param name="isRequired">Set to true if the data in the entry field is required</param>
        public void ValidateIsNotEmpty(EditText editText, bool isRequired = false)
        {
            if (!alreadyFailed)
            {
                if (String.IsNullOrEmpty(editText.Text) || String.IsNullOrWhiteSpace(editText.Text))
                {
                    passed = false;
                    alreadyFailed = true;
                    string response = !isRequired ? "is empty." : "is required.";
                    editText.SetError($"This {response}", errorIcon);
                    editText.RequestFocus();
                }
                else
                    passed = true;
            }
        }

        /// <summary>
        /// Validates if the text in the entry field is a valid amount
        /// </summary>
        /// <param name="editText"></param>
        /// <returns></returns>
        public decimal ValidateAmount(EditText editText)
        {
            decimal output = 0;
            if (!alreadyFailed)
            {
                if (editText.Text != string.Empty && decimal.TryParse(editText.Text, out output))
                    passed = true;
                else
                {
                    passed = false;
                    alreadyFailed = true;
                    editText.SetError("This is not a valid amount.", errorIcon);
                    editText.RequestFocus();
                }
            }

            return output;
        }

        /// <summary>
        /// Validates if the text in both entry fields are the same
        /// </summary>
        /// <param name="passwordTb"></param>
        /// <param name="retypePasswordTb"></param>
        public void ValidateIsSame(EditText passwordTb, EditText retypePasswordTb)
        {
            if (!alreadyFailed)
            {
                if (passwordTb.Text == retypePasswordTb.Text)
                    passed = true;
                else
                {
                    passed = false;
                    alreadyFailed = true;
                    Toast.MakeText(App.CurrentActivity, "Passwords are not the same.", ToastLength.Long).Show();
                }
            }
        }

        /// <summary>
        /// Validates if the text in the entry field is a valid email address
        /// </summary>
        /// <param name="editText"></param>
        public void ValidateEmail(EditText editText)
        {
            if (!alreadyFailed)
            {
                if (Regex.IsMatch(editText.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    passed = true;
                else
                {
                    editText.SetError("Email address is invalid.", errorIcon);
                    editText.RequestFocus();
                    passed = false;
                    alreadyFailed = true;
                }
            }
        }

        public void Dispose() => alreadyFailed = false;
    }
}