using System;
using System.Collections.Generic;
using System.Collections;
using TrickyLib.Parser.Json;

namespace TrickyLib.Parser.Json
{
    /// <summary>
    /// JsonArray class
    /// </summary>
    public class JsonArray : IJsonContainer, IList {
        internal List<object> _Objects;

        #region Constructor
        /// <summary>
        /// Creates an empty JsonArray
        /// </summary>
        public JsonArray() {
            _Objects = new List<object>();
        }

        /// <summary>
        /// Create a new JsonArray
        /// </summary>
        /// <param name="jsonString"></param>
        /// <exception cref="FormatException">JsonString represents JsonObject instead of JsonArray</exception>
        public JsonArray(string jsonString) {
            JsonArray ja = TrickyLib.Parser.Json.JsonParser.Parse(jsonString) as JsonArray;
            if (ja == null) throw new FormatException("JsonString represents JsonObject instead of JsonArray");
            this._Objects = ja._Objects;
        }
        #endregion

        void IJsonContainer.InternalAdd(string key, object value) {
            _Objects.Add(value);
        }
        bool IJsonContainer.IsArray { get { return true; } }

        /// <summary>
        /// Gets the object located at the specified index in the JsonArray
        /// </summary>
        /// <param name="index">Index of object</param>
        /// <returns></returns>
        public object this[int index] {
            get {
                return _Objects[index];
            }
            set {
                JsonHelper.CheckValidType(value);
                _Objects[index] = value;
            }
        }

        #region Interface
        /// <summary>
        /// The number of objects contained in this JsonArray
        /// </summary>
        public int Count { get { return _Objects.Count; } }

        bool IList.IsFixedSize { get { return false; } }

        bool IList.IsReadOnly { get { return false; } }

        bool ICollection.IsSynchronized { get { return false; } }

        object ICollection.SyncRoot { get { return this; } }

        void ICollection.CopyTo(Array array, int startIndex) {
            for (int i = 0; i < this._Objects.Count; i++) {
                array.SetValue(_Objects[i], i + startIndex);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Objects.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the JsonArray
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <returns>Index of the added item</returns>
        public int Add(object item) {
            JsonHelper.CheckValidType(item);
            _Objects.Add(item);
            return _Objects.Count - 1;
        }

        /// <summary>
        /// Removes all items in the JsonArray
        /// </summary>
        public void Clear() {
            _Objects.Clear();
        }

        /// <summary>
        /// Determines whether the JsonArray contains a specific value
        /// </summary>
        /// <param name="item">Value to be checked</param>
        /// <returns>True if the specified value is found in the JsonArray, otherwise False</returns>
        public bool Contains(object item) {
            return _Objects.Contains(item);
        }

        /// <summary>
        /// Determines the index of the first occurrence of a specific value
        /// </summary>
        /// <param name="item">Value to be checked</param>
        /// <returns>Index of the first occurrence of the specified value, -1 if the value is not found</returns>
        public int IndexOf(object item) {
            return _Objects.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the JsonArray at the specified index
        /// </summary>
        /// <param name="index">Index of item to be inserted</param>
        /// <param name="item">Value of item to be inserted</param>
        public void Insert(int index, object item) {
            JsonHelper.CheckValidType(item);
            _Objects.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specified value from the JsonArray
        /// </summary>
        /// <param name="item">Value to be removed</param>
        public void Remove(object item) {
            _Objects.Remove(item);
        }

        /// <summary>
        /// Removes the item at the specified index
        /// </summary>
        /// <param name="index">Index of item to be removed</param>
        public void RemoveAt(int index) {
            _Objects.RemoveAt(index);
        }

        #endregion

        /// <summary>
        /// Returns the shortest string representation of the current JsonArray
        /// </summary>
        /// <returns>A string representation of the current JsonArray</returns>
        public override string ToString() {
            JsonStringLevel tsl = new JsonStringLevel();
            tsl.enumerator = _Objects.GetEnumerator();
            tsl.HasValue = false;
            return JsonStringGenerator.GetJsonString(tsl,string.Empty,string.Empty);
        }
        /// <summary>
        /// Returns a string representation of the current JsonArray in indented format
        /// </summary>
        /// <param name="newline">newline characters</param>
        /// <param name="indent">indent characters</param>
        /// <returns>A string</returns>
        public string ToString(string newline, string indent) {
            JsonStringLevel tsl = new JsonStringLevel();
            tsl.enumerator = _Objects.GetEnumerator();
            tsl.HasValue = false;
            return JsonStringGenerator.GetJsonString(tsl, newline, indent);
        }
    }
}
