using Alansa.Droid.Adapters;
using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Alansa.Droid.Interfaces;
using Alansa.Droid.Utils;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace Alansa.Droid.Fragments
{
    internal class SearchResourceDialog<T> : DialogFragment where T : new()
    {
        protected RecyclerView recycler;
        protected ProgressBar loadingCircle;
        protected View searchEmptyState, emptyState;
        private SearchAdapter<ISearchable> adapter;
        protected EditText searchView;
        private Button createButton;
        private readonly Func<string, Task<IEnumerable<ISearchable>>> searchHandler;
        private ObservableCollection<ISearchable> collection;
        private IEnumerable<ISearchable> backupList;
        protected readonly Action OnEmpty;
        public Action<int, object> OnDatumSelected;
        public Action<string> OnDialogDismiss;
        protected string searchViewHint, dbPath, emptyStateText;
        protected readonly int selectionId, emptyStateIconResId;

        /// <summary>
        /// Resources like StockItem need to have a custom offline search handler. Setting this property to true will cause the search dialog to use the custom search handler when the app is offline
        /// </summary>
        public bool HasCustomOfflineSearchHandler { get; set; }

        /// <summary>
        /// Pops-up a dialog populated with the resources returned by the search handler
        /// </summary>
        /// <param name="dbPath">The path to the offline version of the resources supposed to be returned by the search handler</param>
        /// <param name="searchHandler">An action that when invoked returns the resources required</param>
        /// <param name="emptyStateHandler">An action to be invoked when a user searches and the search handler returns no results</param>
        /// <param name="datumSelectedHandler">An action to be invoked when a resource is selected/tapped on</param>
        /// <param name="emptyStateIconResId">The resource id for the icon to be used in the empty state when no search results are returned. This icon should be in the drawable folder</param>
        /// <param name="selectionId">The id of the item that will be set as the selected object when the resources are populated</param>
        public SearchResourceDialog(string dbPath, Func<string, Task<IEnumerable<ISearchable>>> searchHandler, Action emptyStateHandler, ref Action<int, object> datumSelectedHandler, int emptyStateIconResId, int selectionId = 0)
        {
            this.dbPath = dbPath;
            this.searchHandler = searchHandler;
            OnEmpty = emptyStateHandler;
            OnDatumSelected = datumSelectedHandler;
            this.selectionId = selectionId;
            this.emptyStateIconResId = emptyStateIconResId;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.dialog_search_resource, null);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.resourceRecycler);
            searchEmptyState = view.FindViewById(Resource.Id.emptySearch);
            emptyState = view.FindViewById(Resource.Id.empty);
            loadingCircle = view.FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            searchView = view.FindViewById<EditText>(Resource.Id.searchView);

            if (emptyStateIconResId != 0) // 0 means use the default smiley face
                EmptyStateManager.SetEmptyStateV2(emptyState, emptyStateIconResId, "Oops! Couldn't find anything");

            createButton.Click += delegate { OnEmpty.Invoke(); Dismiss(); };

            loadingCircle.Visibility = ViewStates.Visible;
            Task.Run(() => SetupList());
            return new AlertDialog.Builder(Activity)
                .SetView(view)
                .Create();
        }

        public void SetHint(string hint) => searchViewHint = hint;

        protected async void OnSearch(string query)
        {
            loadingCircle.Visibility = ViewStates.Visible;
            emptyState.Visibility = ViewStates.Gone;
            searchEmptyState.Visibility = ViewStates.Gone;

            if (App.IsOffline && collection != null)
            {
                if (!backupList.Any(x => x.GetPrimaryText().Contains(query, true)))
                    ShowEmptyState();
                adapter.ShowSearchResults(backupList, query);
            }
            else
            {
                var searchResult = await searchHandler.Invoke(query);
                if (searchResult != null && searchResult.ToList().Count == 0)
                    ShowEmptyState();
                adapter?.ShowSearchResults(searchResult?.ToList());
            }

            loadingCircle.Visibility = ViewStates.Gone;
        }

        internal void SetEmptyStateText(string emptyStateText) => this.emptyStateText = emptyStateText;

        public void SetSelectedItem()
        {
            if (selectionId != 0 && collection != null)
            {
                var existingItem = collection.FirstOrDefault(x => x.GetId() == selectionId);
                if (existingItem != null)
                    OnDatumSelected(selectionId, existingItem);
            }
        }

        protected virtual void SetupList()
        {
            App.Post(async () =>
            {
                IEnumerable<ISearchable> searchResult = null;
                if (App.IsOffline && !HasCustomOfflineSearchHandler)
                {
                    var cache = (await DBAssist.DeserializeDBAsync<T>(dbPath))?.GetList();
                    searchResult = cache;
                    backupList = cache; // This is the list we'd use when the user is searching offline
                }
                else
                {
                    searchResult = (await DBAssist.DeserializeDBAsync<T>(dbPath))?.GetList(); // Get offline if exists first then invalidate
                    InvalidateCache();
                }

                if (searchResult == null)
                    searchResult = new List<ISearchable>();

                if (searchResult.ToList().Count == 0)
                    ShowEmptyState();
                collection = new ObservableCollection<ISearchable>(searchResult);
                adapter = new SearchAdapter<ISearchable>(collection, recycler, Resource.Layout.row_search_result);
                adapter.ItemClicked += ItemClicked;
                recycler.SetLayoutManager(new LinearLayoutManager(Activity));
                recycler.SetAdapter(adapter);
                recycler.SetItemAnimator(new DefaultItemAnimator());
                SetSelectedItem();

                loadingCircle.Visibility = ViewStates.Gone;
            });
        }

        private async Task InvalidateCache()
        {
            var result = await searchHandler.Invoke(string.Empty);
            searchEmptyState.Visibility = ViewStates.Gone;
            emptyState.Visibility = ViewStates.Gone;
            if (result != null)
            {
                if (result.ToList().Count == 0)
                    ShowEmptyState();

                collection.Clear();
                collection.AddRange(result);
            }
            else
                Snackbar.Make(loadingCircle, "Can't load data. Please check your connection.", Snackbar.LengthIndefinite)
                    .SetAction("Retry", (v) => { loadingCircle.Visibility = ViewStates.Visible; Task.Run(() => SetupList()); })
                    .Show();
        }

        protected virtual void ShowEmptyState()
        {
            if (!string.IsNullOrEmpty(searchView.Text)) // When the user searches and found nothing
            {
                searchEmptyState.Visibility = ViewStates.Visible;
                searchEmptyState.FindViewById<TextView>(Resource.Id.infoText).Text = $"{searchView.Text} not found";

                if (OnEmpty != null)
                    createButton.Visibility = ViewStates.Visible;
            }
            else // When the list is empty without user searching
            {
                emptyState.Visibility = ViewStates.Visible;
                if (!string.IsNullOrEmpty(emptyStateText))
                    emptyState.FindViewById<TextView>(Resource.Id.infoText).Text = emptyStateText;
            }
        }

        protected virtual void ItemClicked(int pos)
        {
            OnDatumSelected?.Invoke(collection[pos].GetId(), collection[pos]);
            Dismiss();
            OnDialogDismiss?.Invoke(collection[pos].GetPrimaryText());
        }

        public override void OnDestroy()
        {
            if (adapter != null)
                adapter.ItemClicked -= ItemClicked;

            backupList = null;
            base.OnDestroy();
        }
    }
}