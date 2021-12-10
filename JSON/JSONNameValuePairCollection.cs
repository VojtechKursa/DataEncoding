using System;
using System.Collections;
using System.Collections.Generic;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Provides a container for the <see cref="JSONNameValuePair"/>.
    /// </summary>
    public class JSONNameValuePairCollection : IEnumerable<JSONNameValuePair>
    {
        #region Properties

        /// <summary>
        /// Gets the list of <see cref="JSONNameValuePair"/> stored in the collection.
        /// </summary>
        public List<JSONNameValuePair> Items { get; } = new List<JSONNameValuePair>();

        /// <summary>
        /// Gets the amount of items stored in the collection.
        /// </summary>
        public int Count { get => Items.Count; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initiates an empty <see cref="JSONNameValuePairCollection"/>.
        /// </summary>
        public JSONNameValuePairCollection()
        { }

        /// <summary>
        /// Initiates a <see cref="JSONNameValuePairCollection"/> that contains the items in the passed collection.
        /// </summary>
        /// <param name="collection">A collection of <see cref="JSONNameValuePair"/> values to initialize the collection with.</param>
        public JSONNameValuePairCollection(IEnumerable<JSONNameValuePair> collection)
        {
            Items.AddRange(collection);
        }

        #endregion

        #region Methods
        #region List methods

        /// <summary>
        /// Adds in item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(JSONNameValuePair item) => Items.Add(item);

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if item was removed from the list. False if item was not found or it's removal failed.</returns>
        public bool Remove(JSONNameValuePair item) => Items.Remove(item);

        /// <summary>
        /// Removes the item at the specified index from the collection.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void RemoveAt(int index) => Items.RemoveAt(index);

        /// <summary>
        /// Removes a range of items from the collection.
        /// </summary>
        /// <param name="index">The index from which to start removing.</param>
        /// <param name="count">The number of items to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void RemoveRange(int index, int count) => Items.RemoveRange(index, count);

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear() => Items.Clear();

        /// <summary>
        /// Inserts an item at a specified position.
        /// </summary>
        /// <param name="index">Index to which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, JSONNameValuePair item) => Items.Insert(index, item);

        /// <summary>
        /// Gets the index of a specified item in the collection.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the specified item in the collection. -1 if item was not found.</returns>
        public int IndexOf(JSONNameValuePair item) => Items.IndexOf(item);

        /// <summary>
        /// Reverses the collection.
        /// </summary>
        public void Reverse() => Items.Reverse();

        /// <summary>
        /// Converts the collection to an array.
        /// </summary>
        /// <returns>The resulting array.</returns>
        public JSONNameValuePair[] ToArray() => Items.ToArray();

        /// <summary>
        /// Determines whether an item is present in the collection.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>True if item was found in the collection, otherwise false.</returns>
        public bool Contains(JSONNameValuePair item) => Items.Contains(item);

        public IEnumerator<JSONNameValuePair> GetEnumerator() => ((IEnumerable<JSONNameValuePair>)Items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Items).GetEnumerator();

        #endregion

        #region Custom methods

        /// <summary>
        /// Determines whether an item with the specified name is present in the collection.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>True if item was found in the collection, otherwise false.</returns>
        public bool Contains(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an item with the specified name.
        /// </summary>
        /// <param name="name">The name of the item to remove.</param>
        public void Remove(string name)
        {
            int index = IndexOf(name);

            if (index != -1)
                RemoveAt(index);
        }

        /// <summary>
        /// Gets the index of an item with the specified name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The index of the specified item in the collection. -1 if item was not found.</returns>
        public int IndexOf(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Returns the <see cref="JSONNameValuePair"/> with the specified name from the collection.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The <see cref="JSONNameValuePair"/> with the specified name or null, if the item was not found.</returns>
        public JSONNameValuePair Find(string name)
        {
            int index = IndexOf(name);

            return index != -1 ? Items[index] : null;
        }

        /// <summary>
        /// Returns the value of a <see cref="JSONNameValuePair"/> with the specified name from the collection.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The value of a <see cref="JSONNameValuePair"/> with the specified name or null, if the name was not found.</returns>
        public JSONBase FindValue(string name)
        {
            int index = IndexOf(name);

            return index != -1 ? Items[index].Value : null;
        }

        #endregion
        #endregion
    }
}
