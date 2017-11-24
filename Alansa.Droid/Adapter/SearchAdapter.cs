using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Alansa.Droid.Interfaces;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;

namespace Alansa.Droid.Adapters
{
    internal class SearchAdapter<T> : SmartAdapter<T> where T : ISearchable
    {
        private ObservableCollection<T> items;

        public SearchAdapter(ObservableCollection<T> items, RecyclerView recyclerView, int layoutId) : base(items, recyclerView, layoutId)
        {
            this.items = items;
        }

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            var PrimaryLabel = layout.FindViewById<TextView>(Resource.Id.primaryLbl);

            viewHolder.AddView("Primary", PrimaryLabel);
        }

        protected override void OnUpdateView(GenericViewHolder holder, T datum)
        {
            holder.GetView<TextView>("Primary").Text = datum.GetPrimaryText();
        }

        public void ShowSearchResults(List<T> result)
        {
            if (result == null) items.Clear();
            else
            {
                items.Clear();
                items.AddRange(result);
            }
        }

        internal void ShowSearchResults(IEnumerable<T> collection, string query)
        {
            if (collection != null)
            {
                var searchResult = collection.Where(x => x.GetPrimaryText().Contains(query, true));
                if (searchResult == null) items.Clear();
                else
                {
                    items.Clear();
                    items.AddRange(searchResult);
                }
            }
        }
    }
}