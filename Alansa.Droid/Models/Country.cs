using Alansa.Droid.Interfaces;
using System.Collections.Generic;

namespace Alansa.Droid.Models
{
    public class Country : ISearchable
    {
        public string alpha2 { get; set; }
        public string alpha3 { get; set; }
        public List<string> countryCallingCodes { get; set; }
        public string emoji { get; set; }
        public string name { get; set; }
        private string callingCode => countryCallingCodes.Count > 0 ? countryCallingCodes[0] : null;

        public int GetId() => 0;

        public string GetPrimaryText() => callingCode == null ? $"{name}" : $"{name} ({callingCode})";
    }
}