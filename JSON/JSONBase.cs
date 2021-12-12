using System;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Serves as a base for all JSON data objects.
    /// </summary>
    public abstract class JSONBase
    {
        /// <summary>
        /// Encodes the data into the JSON format.
        /// </summary>
        /// <returns>The encoded string.</returns>
        public abstract string Encode();

        /// <summary>
        /// Decodes the given JSON data and sets object's properties based on the decoded data.
        /// </summary>
        /// <param name="json">The JSON data to decode.</param>
        /// <param name="start">The index from which to start decoding.</param>
        /// <returns>The index of the first character after the decoded value.</returns>
        /// <exception cref="ArgumentException"/>
        public abstract int Decode(string json, int start);
    }
}
