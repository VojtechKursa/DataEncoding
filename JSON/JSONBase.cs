using System;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Serves as a base for all JSON data objects.
    /// </summary>
    public abstract class JSONBase
    {
        /// <summary>
        /// Gets or sets the encoded string.
        /// </summary>
        public string Encoded { get; protected set; }

        /// <summary>
        /// Encodes the data into the JSON format and sets it to the <see cref="Encoded"/> variable.
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

        /// <summary>
        /// Beautifies the given JSON string.
        /// </summary>
        /// <param name="json">JSON-encoded text to beautify.</param>
        /// <returns>The beautified JSON string.</returns>
        public static string Beautify(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Minifies the given JSON string.
        /// </summary>
        /// <param name="json">JSON-encoded text to minify.</param>
        /// <returns>The minified JSON string.</returns>
        public static string Minify(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines the datatype of the first detected data and returns a new JSON object accordingly to that datatype.
        /// </summary>
        /// <param name="json">The JSON string in which to search for data.</param>
        /// <param name="start">The index from which to start the search.</param>
        /// <returns>A JSON object that represents the detected datatype.</returns>
        internal static JSONBase GetDatatype(string json, int start)
        {
            for (int i = start; i < json.Length; i++)
            {
                switch (json[i])
                {
                    case '\"': return new JSONString();
                    case '{': return new JSONObject();
                }
            }

            return null;
        }
    }
}
