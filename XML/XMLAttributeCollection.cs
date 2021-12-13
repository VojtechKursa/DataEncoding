using System.Collections;
using System.Collections.Generic;

namespace DataEncoding.XML
{
    /// <summary>
    /// Represents a collection of <see cref="XMLAttribute"/>s.
    /// </summary>
    public class XMLAttributeCollection : IEnumerable<XMLAttribute>
    {
        #region Properties

        /// <summary>
        /// Contains all <see cref="XMLAttribute"/>s in the <see cref="XMLAttributeCollection"/>.
        /// </summary>
        public List<XMLAttribute> Items { get; set; } = new List<XMLAttribute>();

        /// <summary>
        /// Gets the number of items in the <see cref="Items"/>.
        /// </summary>
        public int Count => Items.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initiates a new, empty <see cref="XMLAttributeCollection"/>.
        /// </summary>
        public XMLAttributeCollection()
        { }

        /// <summary>
        /// Initiates a new <see cref="XMLAttributeCollection"/> and copies all items from the given collection to it.
        /// </summary>
        /// <param name="collection">A collection of items to copy.</param>
        public XMLAttributeCollection(IEnumerable<XMLAttribute> collection)
        {
            Items.AddRange(collection);
        }

        #endregion

        #region Methods
        #region Methods from List

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(XMLAttribute item) => Items.Add(item);

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void Remove(XMLAttribute item) => Items.Remove(item);

        /// <summary>
        /// Removes an item at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index) => Items.RemoveAt(index);

        /// <summary>
        /// Clears the collection. (Removes all it's items)
        /// </summary>
        public void Clear() => Items.Clear();

        /// <inheritdoc/>
        public IEnumerator<XMLAttribute> GetEnumerator()
        {
            return ((IEnumerable<XMLAttribute>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }

        #endregion

        #region Name-based methods

        /// <summary>
        /// Returns an <see cref="XMLAttribute"/> from the <see cref="XMLAttributeCollection"/> based on it's name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>
        /// The <see cref="XMLAttribute"/> with the <see cref="XMLAttribute.Name"/> matching the given name.<br />
        /// null of no <see cref="XMLAttribute"/> with the given name was found.
        /// </returns>
        public XMLAttribute Find(string name)
        {
            foreach (XMLAttribute attribute in Items)
            {
                if (attribute.Name == name)
                    return attribute;
            }

            return null;
        }

        /// <summary>
        /// Searches for <see cref="XMLAttribute"/> from the <see cref="XMLAttributeCollection"/> based on it's name and returns it's <see cref="XMLAttribute.Value"/>.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>
        /// The <see cref="XMLAttribute.Value"/> of the <see cref="XMLAttribute"/> with the <see cref="XMLAttribute.Name"/> matching the given name.<br />
        /// null of no <see cref="XMLAttribute"/> with the given name was found.
        /// </returns>
        public string FindValue(string name)
        {
            return Find(name)?.Value;
        }

        /// <summary>
        /// Removes an <see cref="XMLAttribute"/> item from the <see cref="XMLAttributeCollection"/> based on it's name.
        /// </summary>
        /// <param name="name">The <see cref="XMLAttribute.Name"/> of the <see cref="XMLAttribute"/> item to remove.</param>
        public void Remove(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        #endregion
        #endregion
    }
}
