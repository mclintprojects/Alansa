using Alansa.Droid.Collections;
using Alansa.Droid.Utils;
using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Object = Java.Lang.Object;

namespace Alansa.Droid.Adapters
{
    /// <summary>
    /// Smart implementation of a RecyclerView adapter that simplifies common operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Android.Support.V7.Widget.RecyclerView.Adapter" />
    public abstract class SmartAdapter<T> : RecyclerView.Adapter
    {
        private ObservableCollection<T> items;
        private int layoutId;
        private RecyclerView recyclerView;
        public Action<int> ItemClicked;
        public Action OnScrolledToBottom;
        private readonly RecyclerScrollListener scrollListener = new RecyclerScrollListener();

        protected SmartAdapter(IEnumerable<T> items, RecyclerView recyclerView, int layoutId)
        {
            Setup(new ObservableCollection<T>(items), recyclerView, layoutId);
        }

        protected SmartAdapter(ObservableCollection<T> items, RecyclerView recyclerView, int layoutId)
        {
            Setup(items, recyclerView, layoutId);
        }

        private void Setup(ObservableCollection<T> items, RecyclerView recyclerView, int layoutId)
        {
            this.items = items;
            items.CollectionChanged += OnCollectionChanged;
            this.layoutId = layoutId;
            this.recyclerView = recyclerView;
            if (recyclerView != null)
            {
                this.recyclerView.SetAdapter(this);
                this.recyclerView.AddOnChildAttachStateChangeListener(new AttachStateChangeListener(this));
                this.recyclerView.AddOnScrollListener(scrollListener);
                scrollListener.OnScrollDown += ScrollListener_OnScrollDown;
            }
        }

        public override int ItemCount => items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            GenericViewHolder genericViewHolder = (GenericViewHolder)holder;
            OnUpdateView(genericViewHolder, items[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View layout = LayoutInflater.From(parent.Context).Inflate(layoutId, parent, false);
            GenericViewHolder genericViewHolder = new GenericViewHolder(layout);
            OnLookupViewItems(layout, genericViewHolder);
            return genericViewHolder;
        }

        public override int GetItemViewType(int position)
        {
            return GetViewIdForType(items[position]);
        }

        protected abstract void OnLookupViewItems(View layout, GenericViewHolder viewHolder);

        protected abstract void OnUpdateView(GenericViewHolder holder, T datum);

        protected virtual int GetViewIdForType(T item)
        {
            return 0;
        }

        protected virtual void OnItemSelected(int position)
        {
            ItemClicked?.Invoke(position);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NotifyItemInserted(e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRemoved(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    NotifyItemChanged(e.OldStartingIndex);
                    NotifyItemChanged(e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Move:
                    NotifyItemRemoved(e.OldStartingIndex);
                    NotifyItemRemoved(e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ScrollListener_OnScrollDown()
        {
            // Can't scroll down anymore, 1 for down, -1 for up
            if (recyclerView.CanScrollVertically(1) == false)
                OnScrolledToBottom?.Invoke();
        }

        /// <summary>
        /// Subscribes to view click so that we can have ItemSelected w/o any custom code
        /// </summary>
        /// <seealso cref="Android.Support.V7.Widget.RecyclerView.Adapter" />
        internal class AttachStateChangeListener : Object, RecyclerView.IOnChildAttachStateChangeListener
        {
            private readonly SmartAdapter<T> parentAdapter;

            public AttachStateChangeListener(SmartAdapter<T> parentAdapter)
            {
                this.parentAdapter = parentAdapter;
            }

            public void OnChildViewAttachedToWindow(View view)
            {
                view.Click += View_Click;
            }

            public void OnChildViewDetachedFromWindow(View view)
            {
                view.Click -= View_Click;
            }

            private void View_Click(object sender, EventArgs e)
            {
                GenericViewHolder holder = (GenericViewHolder)parentAdapter.recyclerView.GetChildViewHolder(((View)sender));
                int clickedPosition = holder.AdapterPosition;
                parentAdapter.OnItemSelected(clickedPosition);
            }
        }
    }
}