using System;
using System.Collections.Generic;
using System.Collections;

namespace TrickyLib.Parser.Json
{
    /// <summary>
    /// JsonObject class
    /// </summary>
    public class JsonObject : IJsonContainer, IDictionary<string, object> {
        /// <summary>
        /// The properties of this JSON Object
        /// </summary>

        internal Dictionary<string, object> Entries;


        #region Constructor
        /// <summary>
        /// Creates an empty JsonObject
        /// </summary>
        public JsonObject() {
            Entries = new Dictionary<string, object>();
        }

        /// <summary>
        /// Create a new JsonObject from a string
        /// </summary>
        /// <param name="jsonString">string that represents JSON Object</param>
        /// <exception cref="FormatException">JsonString represents JsonArray instead of JsonObject</exception>
        public JsonObject(string jsonString) {
            JsonObject jo = TrickyLib.Parser.Json.JsonParser.Parse(jsonString) as JsonObject;
            if (jo == null) throw new FormatException("JsonString represents JsonArray instead of JsonObject");
            this.Entries = jo.Entries;
        }
        #endregion

        void IJsonContainer.InternalAdd(string key, object value) {
            Entries.Add(key, value);
        }
        bool IJsonContainer.IsArray { get { return false; } }


        /// <summary>
        /// Gets a property of the current JSON Object by key
        /// </summary>
        /// <param name="key">Key of property</param>
        /// <returns>Value of property. Returns null if property is not found.</returns>
        public object this[string key] {
            get {
                if (Entries.ContainsKey(key)) return Entries[key];
                return null;
            }
            set {
                JsonHelper.CheckValidType(value);
                Entries[key] = value;
            }
        }

        #region Interface
        /// <summary>
        /// The number of key/value pairs contained in the JsonObject
        /// </summary>
        public int Count { get { return Entries.Count; } }
        /// <summary>
        /// Whether the JsonObject is read-only. This value is always true.
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// All the keys in the JsonObject
        /// </summary>
        public ICollection<string> Keys { get { return Entries.Keys; } }
        /// <summary>
        /// All the values in the JsonObject
        /// </summary>
        public ICollection<object> Values { get { return Entries.Values; } }
        /// <summary>
        /// Adds the specified key and value to the JsonObject.
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
            JsonHelper.CheckValidType(item.Value);
            Add(item.Key, item.Value);
        }
        /// <summary>
        /// Adds the specified key and value to the JsonObject.
        /// </summary>
        /// <param name="key">Key of entry</param>
        /// <param name="value">Value of entry</param>
        public void Add(string key, object value) {
            JsonHelper.CheckValidType(value);
            Entries.Add(key, value);
        }
        /// <summary>
        /// Removes all keys and values from the JsonObject.
        /// </summary>
        public void Clear() { Entries.Clear(); }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) { throw new NotImplementedException(); }
        /// <summary>
        /// Removes the item with the specified key from the JsonObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key) { return Entries.Remove(key); }
        /// <summary>
        /// Copy all the entries to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
            int i = 0;
            foreach (KeyValuePair<string, object> KVP in Entries) {
                array[arrayIndex + (i++)] = KVP;
            }
        }
        /// <summary>
        /// Determines whether the JsonObject contains the specified key/value pair.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item) {
            return Entries.ContainsKey(item.Key) && Entries[item.Key].Equals(item.Value);
        }
        /// <summary>
        /// Determines whether the JsonObject contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            return Entries.ContainsKey(key);
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value) {
            return Entries.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
            return Entries.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return Entries.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Returns the shortest string representation of the current JsonObject
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString() {
            JsonStringLevel tsl = new JsonStringLevel();
            tsl.enumerator = Entries.GetEnumerator();
            tsl.HasValue = false;
            return JsonStringGenerator.GetJsonString(tsl,string.Empty,string.Empty);
        }
        /// <summary>
        /// Returns a string representation of the current JsonObject in indented format
        /// </summary>
        /// <param name="newline">newline characters</param>
        /// <param name="indent">indent characters</param>
        /// <returns>A string</returns>
        public string ToString(string newline, string indent) {
            JsonStringLevel tsl = new JsonStringLevel();
            tsl.enumerator = Entries.GetEnumerator();
            tsl.HasValue = false;
            return JsonStringGenerator.GetJsonString(tsl, newline, indent);
        }
    }
}
