using Android.Content;
using System;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace Alansa.Droid.Extensions
{
    public static class DialogExtensions
    {
        /// <summary>
        /// Shows a generic dialog with neutral, positive and negative action buttons.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="positiveText"></param>
        /// <param name="negativeText"></param>
        /// <param name="neutralText"></param>
        /// <param name="positive"></param>
        /// <param name="negative"></param>
        /// <param name="neutral"></param>
        public static void ShowDialog(this Context context, string title, string message, string positiveText = "Dismiss", string negativeText = "Cancel", string neutralText = "More", Action positive = null, Action negative = null, Action neutral = null)
        {
            App.Post(() =>
            {
                new AlertDialog.Builder(context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton(positiveText, (s, e) => positive?.Invoke())
                .SetNegativeButton(negativeText, (s, e) => negative?.Invoke())
                .SetNeutralButton(neutralText, (s, e) => neutral?.Invoke())
                .Create()
                .Show();
            });
        }

        /// <summary>
        /// Shows a generic dialog with only a positive action button
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="positiveText"></param>
        /// <param name="positive"></param>
        /// <param name="cancelable"></param>
        public static void ShowPositiveDialog(this Context context, string title, string message, string positiveText = "Dismiss", Action positive = null, bool cancelable = true)
        {
            App.Post(() =>
            {
                new AlertDialog.Builder(context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton(positiveText, (s, e) => positive?.Invoke())
                .SetCancelable(cancelable)
                .Create()
                .Show();
            });
        }

        /// <summary>
        /// Shows a generic dialog with both positive and negative action buttons
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="positiveText"></param>
        /// <param name="negativeText"></param>
        /// <param name="positive"></param>
        /// <param name="negative"></param>
        /// <param name="cancelable"></param>
        public static void ShowNegativeDialog(this Context context, string title, string message, string positiveText = "Dismiss", string negativeText = "Cancel", Action positive = null, Action negative = null, bool cancelable = true)
        {
            App.Post(() =>
            {
                new AlertDialog.Builder(context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton(positiveText, (s, e) => positive?.Invoke())
                .SetNegativeButton(negativeText, (s, e) => negative?.Invoke())
                .SetCancelable(cancelable)
                .Create()
                .Show();
            });
        }

        /// <summary>
        /// Shows a deletion confirmation dialog. "Do you want to delete this item" with Yes, No action buttons?
        /// </summary>
        /// <param name="context"></param>
        /// <param name="positive"></param>
        public static void ShowDeleteDialog(this Context context, Action positive) => ShowNegativeDialog(context, "Confirm delete", "Do you want to delete this item?", "Yes", "No", positive, null);
    }
}