using Android.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Alansa.Droid.Utils
{
    /// <summary>
    /// A class that exposes static methods for serializing and deserializing json databases.
    /// </summary>
    public class JsonDbManager
    {
        private const string dbTag = "ALANSA-DB";
        private static JsonDbManager _instance;

        public static JsonDbManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new JsonDbManager();

                return _instance;
            }
        }

        /// <summary>
        /// Deserializes a json database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the database</typeparam>
        /// <param name="location">The location of the database file</param>
        /// <param name="TObject">A sample object of the type of the database. eg: "new Car()"</param>
        /// <param name="decryptObject">Set to true if you want to decrypt the database file.</param>
        /// <returns>The database</returns>
        public async Task<T> DeserializeDBAsync<T>(string location, bool decryptObject = false) where T : new()
        {
            try
            {
                string value;
                using (var reader = new StreamReader(new FileStream(location, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    value = await reader.ReadToEndAsync();
                    if (value != null && value.Length != 0)
                    {
                        if (decryptObject)
                            value = Cryptor.Decrypt(value);
                    }
                    else
                        return default(T);
                }

                return JsonConvert.DeserializeAnonymousType(value, new T());
            }
            catch (Exception e)
            {
                Log.Error(dbTag, e.Message);
                return default(T);
            }
        }

        public static T DeserializeDB<T>(string location, bool decryptObject = false) where T : new()
        {
            try
            {
                string value;
                using (var reader = new StreamReader(new FileStream(location, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    value = reader.ReadToEnd();
                    if (value != null && value.Length != 0)
                    {
                        if (decryptObject)
                            value = Cryptor.Decrypt(value);
                    }
                    else
                        return default(T);
                }

                return JsonConvert.DeserializeAnonymousType(value, new T());
            }
            catch (Exception e)
            {
                Log.Error(dbTag, e.Message);
                return default(T);
            }
        }

        public static void WriteJsonAsync(string location, string json)
        {
            if (!string.IsNullOrEmpty(json) && !string.IsNullOrEmpty(location) && File.Exists(location))
                Task.Run(() => File.WriteAllText(location, json));
        }

        /// <summary>
        /// Serializes a json database asynchronously.
        /// </summary>
        /// <param name="location">Where to write the database file to</param>
        /// <param name="objectToSerialize">The database object to write to json</param>
        /// <param name="encryptObject">Set to true if you want to encrypt the database</param>
        public static async Task SerializeDBAsync(string location, object objectToSerialize, bool encryptObject = false)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (objectToSerialize != null)
                    {
                        using (StreamWriter w = new StreamWriter(new FileStream(location, FileMode.Create, FileAccess.ReadWrite, FileShare.None)))
                        {
                            if (encryptObject)
                                w.Write(Cryptor.Encrypt(JsonConvert.SerializeObject(objectToSerialize)));
                            else
                                w.Write(JsonConvert.SerializeObject(objectToSerialize));

                            Log.Debug(dbTag, $"Successfully wrote DB at {location}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(dbTag, e.Message);
                }
            });
        }
    }
}