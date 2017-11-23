using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GhalaniDroid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alansa.Droid.Adapters
{
    internal class DoubleItemAdapter<T> : SmartAdapter<T> where T : IDoubleItem
    {
        private readonly ObservableCollection<T> items;
        private readonly bool showDeleteBtn;
        public Action<int> OnDeleteClicked;

        public DoubleItemAdapter(ObservableCollection<T> items, RecyclerView recyclerView, bool showDeleteBtn = false) : base(items, recyclerView, Resource.Layout.row_double_item)
        {
            this.items = items;
            this.showDeleteBtn = showDeleteBtn;
        }

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            var PrimaryLbl = layout.FindViewById<TextView>(Resource.Id.primaryTextLbl);
            var SecondaryLbl = layout.FindViewById<TextView>(Resource.Id.secondaryTextLbl);
            var DeleteBtn = layout.FindViewById<ImageView>(Resource.Id.deleteBtn);

            viewHolder.AddView("PrimaryLbl", PrimaryLbl);
            viewHolder.AddView("SecondaryLbl", SecondaryLbl);

            DeleteBtn.Click += delegate { OnDeleteClicked?.Invoke(viewHolder.AdapterPosition); };
            if (showDeleteBtn)
                DeleteBtn.Visibility = ViewStates.Visible;
        }

        protected override void OnUpdateView(GenericViewHolder holder, T datum)
        {
            holder.GetView<TextView>("PrimaryLbl").Text = datum.GetPrimaryText();
            holder.GetView<TextView>("SecondaryLbl").Text = datum.GetSecondaryText();
        }

        internal void DisplayServerSearchResults(IEnumerable<T> tempItems)
        {
            if (tempItems == null)
                items.Clear();
            else
                items.AddRange(tempItems);
        }

        internal void ShowSearchResults(string query, IEnumerable<T> tempItems)
        {
            if (tempItems != null)
            {
                items.Clear();
                if (query != string.Empty)
                    items.AddRange(tempItems.ToList().FindAll(x => x.GetPrimaryText().Contains(query, true)));
                else
                    items.AddRange(tempItems);
            }
        }
    }
}