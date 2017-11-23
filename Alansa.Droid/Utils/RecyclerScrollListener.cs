using Android.Support.V7.Widget;
using System;

namespace Alansa.Droid.Utils
{
    public class RecyclerScrollListener : RecyclerView.OnScrollListener
    {
        public Action OnScrollDown;

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            if (dy > 0) // scrolling down
                OnScrollDown?.Invoke();

            base.OnScrolled(recyclerView, dx, dy);
        }
    }
}