using Android.App;
using Android.Content;
using Newtonsoft.Json;

namespace Alansa.Droid.Utils
{
    public class PreferenceManager
    {
        private readonly ISharedPreferences prefs;
        private readonly ISharedPreferencesEditor editor;

        private PreferenceManager _instance;

        public PreferenceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PreferenceManager();

                return _instance;
            }
        }

        private PreferenceManager()
        {
            prefs = Application.Context.GetSharedPreferences("app-prefs", FileCreationMode.Private);
            editor = prefs.Edit();
        }

        /// <summary>
        /// Gets a boolean entry in the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, bool @default = false) => prefs.GetBoolean(key, @default);

        /// <summary>
        /// Gets a string entry in the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key) => prefs.GetString(key, null);

        /// <summary>
        /// Adds a boolean entry to the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddEntry(string key, bool value)
        {
            editor.PutBoolean(key, value);
            editor.Commit();
        }

        /// <summary>
        /// Adds a string entry to the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddEntry(string key, string value)
        {
            editor.PutString(key, value);
            editor.Commit();
        }

        /// <summary>
        /// Add a json entry to the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        public void AddJsonEntry(string key, string json) => AddEntry(key, json);

        /// <summary>
        /// Gets a json entry in the app preferences
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetJsonEntry(string key) => GetString(key);

        /// <summary>
        /// Deserializes a json entry in the app preferences to a POCO of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetJsonEntryAs<T>(string key) where T : new()
        {
            var jsonEntry = GetJsonEntry(key);
            if (jsonEntry == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonEntry);
        }
    }
}