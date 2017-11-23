using System.Collections.Generic;
using System.Collections.Specialized;

namespace AlansaDroid.Collections
{
    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
    {
        public ObservableCollection(IEnumerable<T> list) : base(list)
        {
        }

        public ObservableCollection(List<T> list) : base(list)
        {
        }

        public ObservableCollection()
        {
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                    Items.Add(item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void ReplaceAt(int index, T item)
        {
            base.SetItem(index, item);
        }
    }
}