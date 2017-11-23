using Android.Support.V4.App;
using System.Collections.Generic;

namespace GhalaniDroid.Adapters
{
    internal class GenericFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Fragment> fragments;
        private readonly string[] titles;

        public GenericFragmentPagerAdapter(FragmentManager manager, List<Fragment> fragments, params string[] titles) : base(manager)
        {
            this.fragments = fragments;
            this.titles = titles;
        }

        public override int Count => fragments.Count;

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position]);
        }
    }
}