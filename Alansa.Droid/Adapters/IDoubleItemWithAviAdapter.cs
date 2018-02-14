using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Alansa.Droid.Interfaces;
using Alansa.Droid.Utils;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Alansa.Droid.Adapters
{
    public class DoubleItemWithAviAdapter<T> : SmartAdapter<T> where T : IDoubleItemWithAvi
    {
        private readonly ObservableCollection<T> items;
        public Action<GenericViewHolder> OnMoreClicked;
        private readonly bool showMoreVert;

        public DoubleItemWithAviAdapter(ObservableCollection<T> items, RecyclerView recyclerView, bool showMoreVert = true) : base(items, recyclerView, Resource.Layout.row_doubleitem_avi)
        {
            this.items = items;
            this.showMoreVert = showMoreVert;
        }

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            var Avi = layout.FindViewById<TextView>(Resource.Id.Avi);
            var Name = layout.FindViewById<TextView>(Resource.Id.Name);
            var Description = layout.FindViewById<TextView>(Resource.Id.Description);
            var MoreVert = layout.FindViewById<ImageView>(Resource.Id.moreVert);

            viewHolder.AddView("Avi", Avi);
            viewHolder.AddView("Name", Name);
            viewHolder.AddView("Description", Description);
            viewHolder.AddView("Root", layout.FindViewById(Resource.Id.root));

            if (showMoreVert)
            {
                viewHolder.AddView("MoreVert", MoreVert);
                MoreVert.Click += (s, e) => OnMoreClicked?.Invoke(viewHolder);
            }
            else
                MoreVert.Visibility = ViewStates.Gone;
        }

        protected override void OnUpdateView(GenericViewHolder holder, T datum)
        {
            holder.GetView<TextView>("Avi").Background.SetColorFilter(ColorManager.GetColor(), PorterDuff.Mode.SrcIn);
            holder.GetView<TextView>("Avi").Text = datum.GetAviText();
            holder.GetView<TextView>("Name").Text = datum.GetPrimaryText().Capitalize();
            holder.GetView<TextView>("Description").Text = datum.GetSecondaryText();
        }

        public void DisplayServerSearchResults(List<T> tempItems)
        {
            if (tempItems == null)
                items.Clear();
            else
                items.AddRange(tempItems);
        }

        public void ShowSearchResults(string query, List<T> tempItems)
        {
            if (tempItems != null)
            {
                items.Clear();
                if (query != string.Empty)
                    items.AddRange(tempItems.FindAll(x => x.GetPrimaryText().Contains(query, true)));
                else
                    items.AddRange(tempItems);
            }
        }
    }
}