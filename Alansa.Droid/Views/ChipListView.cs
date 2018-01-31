using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Alansa.Droid.Adapters;
using Alansa.Droid.Utils;
using System;
using Alansa.Droid.Collections;

namespace Alansa.Droid.Views
{
    public class ChipListView : LinearLayout
    {
        private readonly ObservableCollection<string> chips = new ObservableCollection<string>();
        private RecyclerView recycler;
        private TextView emptyStateLbl;
        private LinearLayoutManager manager;
        private ChipAdapter adapter;
        private bool _reverseLayout = true;
        private bool hideChipRemoveBtn;
        private string emptyStateText;

        public bool ReverseLayout
        {
            get => _reverseLayout;
            set
            {
                _reverseLayout = value;
                manager.ReverseLayout = value;
            }
        }

        public bool HideChipRemoveBtn
        {
            get => hideChipRemoveBtn;
            set => hideChipRemoveBtn = value;
        }

        public int ChipCount => chips.Count;

        /// <summary>
        /// Fires when a chip is being removed
        /// </summary>
        public Action<int, string> OnChipRemove;

        public ChipListView(Context context) : base(context)
        {
            Initialize(context);
        }

        public ChipListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs);
        }

        protected ChipListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Initialize(Context ctx, IAttributeSet attrs = null)
        {
            var inflater = LayoutInflater.FromContext(ctx);
            inflater.Inflate(Resource.Layout.layout_chip_listview, this);
            recycler = FindViewById<RecyclerView>(Resource.Id.chipRecycler);
            emptyStateLbl = FindViewById<TextView>(Resource.Id.chipEmptyStateLbl);

            if (attrs != null)
            {
                var array = ctx.ObtainStyledAttributes(attrs, Resource.Styleable.ChipListView, 0, 0);
                hideChipRemoveBtn = array.GetBoolean(Resource.Styleable.ChipListView_hideRemoveChipBtn, false);
                _reverseLayout = array.GetBoolean(Resource.Styleable.ChipListView_reverseChipLayout, true);

                SetupChips(ctx);

                SetUsesRandomBackground(array.GetBoolean(Resource.Styleable.ChipListView_usesRandomBackgroundColor, false));
                emptyStateText = array.GetString(Resource.Styleable.ChipListView_emptyStateText);

                array.Recycle();
            }

            ShowEmptyState();
        }

        private void SetupChips(Context ctx)
        {
            manager = new LinearLayoutManager(ctx, LinearLayoutManager.Horizontal, _reverseLayout);
            adapter = new ChipAdapter(chips, recycler, hideChipRemoveBtn);
            adapter.OnChipRemove += (pos, name) => { OnChipRemove?.Invoke(pos, name); };
            recycler.SetLayoutManager(manager);
            recycler.SetAdapter(adapter);
            recycler.SetItemAnimator(new DefaultItemAnimator());
        }

        /// <summary>
        /// Adds a chip to the chips list
        /// </summary>
        /// <param name="name">The name of the chip to add</param>
        public void AddChip(string name)
        {
            HideEmptyState();
            if (name != null)
                chips.Add(name);

            recycler.ScrollToPosition(chips.Count - 1);
        }

        /// <summary>
        /// Clears all the chips from the chips listview
        /// </summary>
        public void Clear() => chips.Clear();

        /// <summary>
        /// Removes a chip from the list
        /// </summary>
        /// <param name="name">The name of the chip to remove</param>
        /// <returns></returns>
        public void RemoveChip(string name)
        {
            if (name != null)
                chips.Remove(name);

            if (chips.Count == 0)
                ShowEmptyState();
        }

        /// <summary>
        /// Sets whether the chip's background color is a bland gray or random backround color
        /// </summary>
        /// <param name="value"></param>
        private void SetUsesRandomBackground(bool value) => adapter.UsesRandomBackgroundColor = value;

        public void SetEmptyStateText(string emptyStateText) => this.emptyStateText = emptyStateText;

        public void ShowEmptyState()
        {
            emptyStateLbl.Text = emptyStateText;
            emptyStateLbl.Visibility = ViewStates.Visible;
            recycler.Visibility = ViewStates.Gone;
        }

        public void HideEmptyState()
        {
            emptyStateLbl.Visibility = ViewStates.Gone;
            recycler.Visibility = ViewStates.Visible;
        }
    }
}