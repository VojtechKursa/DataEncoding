using System.Collections;
using System.Collections.Generic;

namespace DataEncoding.XML
{
    public class XMLContentCollection : IEnumerable<XMLBase>
    {
        #region Properties

        /// <summary>
        /// Contains all <see cref="XMLBase"/>s in the <see cref="XMLContentCollection"/>.
        /// </summary>
        public List<XMLBase> Items { get; set; } = new List<XMLBase>();

        /// <summary>
        /// Gets the number of items in the <see cref="Items"/>.
        /// </summary>
        public int Count => Items.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initiates a new, empty <see cref="XMLAttributeCollection"/>.
        /// </summary>
        public XMLContentCollection()
        { }

        /// <summary>
        /// Initiates a new <see cref="XMLAttributeCollection"/> and copies all items from the given collection to it.
        /// </summary>
        /// <param name="collection">A collection of items to copy.</param>
        public XMLContentCollection(IEnumerable<XMLBase> collection)
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
        public void Add(XMLBase item) => Items.Add(item);

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void Remove(XMLBase item) => Items.Remove(item);

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
        public IEnumerator<XMLBase> GetEnumerator()
        {
            return ((IEnumerable<XMLBase>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }

        #endregion

        #region Name-based methods

        /// <summary>
        /// Returns an <see cref="XMLElement"/> from the <see cref="XMLContentCollection"/> based on it's name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>
        /// The <see cref="XMLElement"/> with the <see cref="XMLElement.Name"/> matching the given name.<br />
        /// null of no <see cref="XMLElement"/> with the given name was found.
        /// </returns>
        public XMLElement Find(string name)
        {
            foreach (XMLBase element in Items)
            {
                if (element is XMLElement elem)
                {
                    if (elem.Name == name)
                        return elem;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes an <see cref="XMLElement"/> item from the <see cref="XMLContentCollection"/> based on it's name.
        /// </summary>
        /// <param name="name">The <see cref="XMLElement.Name"/> of the <see cref="XMLElement"/> item to remove.</param>
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
