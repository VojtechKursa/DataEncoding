using System;
using DataEncoding.Interfaces;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Serves as a base for all JSON data objects.
    /// </summary>
    public abstract class JSONBase : ISupportsEncode<string>, ISupportsDecode<string>
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
        /// <returns>The index of the first character after the decoded value.</returns>
        /// <exception cref="ArgumentException"/>
        public int Decode(string json) => Decode(json, 0);

        /// <inheritdoc cref="Decode(string)"/>
        /// <param name="start">The index from which to start decoding.</param>
        public abstract int Decode(string json, int start);

        /// <summary>
        /// Initiates a new instance of the <see cref="JSONBase"/>, based on the data decoded from the given JSON-encoded text.
        /// </summary>
        /// <param name="json">The JSON data to decode.</param>
        /// <param name="start">The index from which to start decoding.</param>
        /// <param name="end">The index of the first character after the decoded value.</param>
        /// <returns>
        /// The new instance of <see cref="JSONBase"/> with the decoded value inside (to access the value, retyping to the correct JSON object is required).<br />
        /// null if no JSON value was found in the input.
        /// </returns>
        public static JSONBase FromEncoded(string json, int start, out int end)
        {
            JSONBase result = JSONFunctions.GetDatatype(json, start, out int dataStartIndex);

            if (result != null)
            {
                end = result.Decode(json, dataStartIndex);
                return result;
            }
            else
            {
                end = -1;
                return null;
            }
        }
    }
}
