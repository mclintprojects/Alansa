using Alansa.Droid.Dialogs;
using Alansa.Droid.Interfaces;
using Android.Content;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alansa.Droid.Views
{
    internal class ResourceSearchView : LinearLayout
    {
        protected EditText queryBar;
        protected ImageView clearBtn;
        protected TextInputLayout resourceTIL;
        public Action<int, object> OnDatumSelected;
        protected int? objId;
        protected object selectedObject;
        protected string dialogHint;

        public string Text { get => queryBar.Text; set => queryBar.Text = value; }

        public string EmptyStateText { get; set; }

        public new bool Enabled { get => queryBar.Enabled; set => queryBar.Enabled = value; }

        public object SelectedObject => selectedObject;

        public EditText QueryBar => queryBar;

        public int? SelectedItemId
        {
            get => objId;
            set => objId = value;
        }

        public T GetSelectedObject<T>() => selectedObject == null ? default(T) : (T)selectedObject;

        public void SetSelectedObject(object datum) => selectedObject = datum;

        public ResourceSearchView(Context ctx) : base(ctx)
        {
            Init(ctx);
        }

        public ResourceSearchView(Context ctx, IAttributeSet attrs) : base(ctx, attrs)
        {
            Init(ctx, attrs);
        }

        private void Init(Context ctx, IAttributeSet attrs = null)
        {
            var view = LayoutInflater.From(ctx).Inflate(Resource.Layout.layout_search_resource, this);
            resourceTIL = view.FindViewById<TextInputLayout>(Resource.Id.resourceTIL);
            clearBtn = view.FindViewById<ImageView>(Resource.Id.clearBtn);
            queryBar = view.FindViewById<EditText>(Resource.Id.queryTb);

            OnDatumSelected += (id, obj) =>
            {
                objId = id;
                selectedObject = obj;
            };

            clearBtn.Click += delegate
            {
                selectedObject = null;
                objId = 0;
                queryBar.Text = string.Empty;
            };

            var hint = GetHint(ctx, attrs);
            if (hint != null)
                resourceTIL.Hint = hint;
        }

        /// <summary>
        /// Sets up the ResourceSearchView
        /// </summary>
        /// <typeparam name="T">The type of the resource being search eg. People, Income etc</typeparam>
        /// <param name="searchHandler">The handler that when invoked returns the data to populate the list with</param>
        /// <param name="OnEmpty">The method to call when the list is empty</param>
        public virtual void SetupSearch<T>(Func<string, Task<IEnumerable<ISearchable>>> searchHandler, Action OnEmpty, int emptyStateIconResId = 0) where T : new()
        {
            queryBar.Click += (s, e) => ShowSearchDialog(searchHandler, OnEmpty, emptyStateIconResId);
            resourceTIL.Click += (s, e) => ShowSearchDialog(searchHandler, OnEmpty, emptyStateIconResId);

            queryBar.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    ShowSearchDialog(searchHandler, OnEmpty, emptyStateIconResId);
                    clearBtn.SetColorFilter(Color.Black, PorterDuff.Mode.SrcAtop);
                    clearBtn.Visibility = ViewStates.Visible;
                }
                else
                {
                    clearBtn.SetColorFilter(Color.ParseColor("#BABABA"), PorterDuff.Mode.SrcAtop);
                    clearBtn.Visibility = ViewStates.Gone;
                }
            };
        }

        protected virtual void ShowSearchDialog(Func<string, Task<IEnumerable<ISearchable>>> searchHandler, Action OnEmpty, int emptyStateIconResId)
        {
            var dialog = new SearchResourceDialog(searchHandler, OnEmpty, ref OnDatumSelected, emptyStateIconResId, objId ?? 0);

            dialog.OnDialogDismiss += (primaryText) => queryBar.Text = primaryText;

            dialog.SetHint(dialogHint);
            dialog.SetEmptyStateText(EmptyStateText);
            dialog.Show(App.CurrentActivity.FragmentManager, string.Empty);
        }

        /// <summary>
        /// Sets the hint of the search entry to "Search for <paramref name="whatUserWillBeSearchingFor"/>"
        /// </summary>
        /// <param name="whatUserWillBeSearchingFor">Name of the item the user will be searching for. eg. Location etc</param>
        public void SetDialogHint(string whatUserWillBeSearchingFor) => dialogHint = $"Search for {whatUserWillBeSearchingFor.ToLower()}";

        private string GetHint(Context ctx, IAttributeSet attrs)
        {
            if (attrs != null)
            {
                var array = ctx.ObtainStyledAttributes(attrs, Resource.Styleable.ResourceSearchView, 0, 0);
                var hint = array.GetString(Resource.Styleable.ResourceSearchView_searchViewHint);

                array.Recycle();
                return hint;
            }

            return null;
        }

        public static implicit operator EditText(ResourceSearchView searchView) => searchView.queryBar;
    }
}