using Alansa.Droid.Adapters;
using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Alansa.Droid.Interfaces;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace Alansa.Droid.Dialogs
{
    public class CountryPickerDialog : DialogFragment
    {
        private readonly ObservableCollection<ISearchable> countriesCollection;
        private RecyclerView recycler;
        private SearchAdapter<ISearchable> adapter;
        private readonly IEnumerable<ISearchable> countriesList;
        public Action<int, object> OnDatumSelected;
        private ProgressBar loadingCircle;

        public CountryPickerDialog(IEnumerable<ISearchable> countries)
        {
            var orderedCountries = countries.OrderBy(x => x.GetPrimaryText());
            countriesCollection = new ObservableCollection<ISearchable>(orderedCountries);
            countriesList = orderedCountries;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.dialog_search_resource, null);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.resourceRecycler);
            loadingCircle = view.FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            var searchView = view.FindViewById<EditText>(Resource.Id.searchView);
            searchView.TextChanged += OnSearch;

            SetupList();
            return new AlertDialog.Builder(Activity)
                .SetView(view)
                .Create();
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            var searchResult = countriesList.ToList().FindAll(x => x.GetPrimaryText().Contains(e.Text.ToString(), true));
            adapter.ShowSearchResults(searchResult);
        }

        private void SetupList()
        {
            loadingCircle.Visibility = ViewStates.Visible;
            adapter = new SearchAdapter<ISearchable>(countriesCollection, recycler, Resource.Layout.row_search_result);
            adapter.ItemClicked += ItemClicked;
            recycler.SetLayoutManager(new LinearLayoutManager(Activity));
            recycler.SetAdapter(adapter);
            recycler.SetItemAnimator(new DefaultItemAnimator());
            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void ItemClicked(int pos)
        {
            OnDatumSelected?.Invoke(countriesCollection[pos].GetId(), countriesCollection[pos]);
            Dismiss();
        }
    }
}