using Android.Views;
using Android.Views.Animations;
using GhalaniDroid.Animation;
using System.Collections.Generic;

namespace GhalaniDroid.Utils
{
    internal class MultiPageSwitcher
    {
        private List<View> children = new List<View>();
        private int lastHiddenChildIndex = 1, currentIndex;

        public void AddChild(View child)
        {
            children.Add(child);
            HideOtherChildren();
        }

        private void HideOtherChildren()
        {
            if (children.Count > 1)
            {
                for (; lastHiddenChildIndex < children.Count; lastHiddenChildIndex++)
                    children[lastHiddenChildIndex].Visibility = ViewStates.Gone;
            }
        }

        public void AddChild(params View[] childrenArray)
        {
            for (int i = 0; i < childrenArray.Length; i++)
                children.Add(childrenArray[i]);
            HideOtherChildren();
        }

        public void Next()
        {
            if (currentIndex < children.Count)
            {
                var currentPage = children[currentIndex];
                var nextPage = children[++currentIndex];
                AnimHelper.Animate(nextPage, "translationX", 700, new DecelerateInterpolator(), 1000, 0);
                AnimHelper.Animate(currentPage, "translationX", 700, new DecelerateInterpolator(), 0, 1000);
                currentPage.Visibility = ViewStates.Gone;
                nextPage.Visibility = ViewStates.Visible;
            }
        }

        public void Previous()
        {
            if (currentIndex > 0)
            {
                var currentPage = children[currentIndex];
                var prevPage = children[--currentIndex];
                AnimHelper.Animate(currentPage, "translationX", 700, new DecelerateInterpolator(), 0, -1000);
                AnimHelper.Animate(prevPage, "translationX", 700, new DecelerateInterpolator(), -1000, 0);
                currentPage.Visibility = ViewStates.Gone;
                prevPage.Visibility = ViewStates.Visible;
            }
        }
    }
}