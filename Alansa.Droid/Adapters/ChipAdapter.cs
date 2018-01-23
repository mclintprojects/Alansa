using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Alansa.Droid.Utils;
using System;
using Alansa.Droid.Collections;

namespace Alansa.Droid.Adapters
{
    public class ChipAdapter : SmartAdapter<string>
    {
        private readonly ObservableCollection<string> chips;
        private readonly bool hideRemoveChipBtn;

        /// <summary>
        /// Set to true if the background of each chip should be a bland gray or random color
        /// </summary>
        public bool UsesRandomBackgroundColor { private get; set; }

        /// <summary>
        /// Fires when a chip is being removed
        /// </summary>
        public Action<int, string> OnChipRemove;

        public ChipAdapter(ObservableCollection<string> items, RecyclerView recyclerView, bool hideRemoveChipBtn = false) : base(items, recyclerView, Resource.Layout.layout_chip)
        {
            chips = items;
            this.hideRemoveChipBtn = hideRemoveChipBtn;
        }

        public override int ItemCount => chips.Count;

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            viewHolder.AddView("Label", layout.FindViewById<TextView>(Resource.Id.chipLabel));
            viewHolder.AddView("RemoveBtn", layout.FindViewById<ImageView>(Resource.Id.chipRemoveBtn));
        }

        protected override void OnUpdateView(GenericViewHolder holder, string datum)
        {
            holder.GetView<TextView>("Label").Text = datum;
            holder.GetView<ImageView>("RemoveBtn").Click += delegate { OnChipRemove?.Invoke(holder.AdapterPosition, datum); };

            if (hideRemoveChipBtn)
            {
                holder.GetView<ImageView>("RemoveBtn").Visibility = ViewStates.Gone;
                holder.GetView<TextView>("Label").SetPadding(16, 0, 16, 0);
            }

            if (UsesRandomBackgroundColor)
                holder.GetView<TextView>("Label").Background.SetColorFilter(ColorManager.GetColor(), PorterDuff.Mode.SrcIn);
        }
    }
}