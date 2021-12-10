using System.Collections.Generic;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Provides an easy way to decode JSON strings with multiple data<br />
    /// (i.e. JSON strings that have more than 1 object or don't have data contained in an object)
    /// </summary>
    public static class JSONDecoder
    {
        /// <summary>
        /// Decodes the given JSON-encoded string.
        /// </summary>
        /// <param name="json">The JSON-encoded string to decode.</param>
        /// <param name="start">The index from which to start decoding.</param>
        /// <returns>A list of any detected data in the JSON string.</returns>
        public static List<JSONBase> DecodeString(string json, int start)
        {
            List<JSONBase> result = new List<JSONBase>();

            JSONBase dataType;
            int lastEnd = start;
            while (true)
            {
                dataType = JSONBase.GetDatatype(json, lastEnd);
                if (dataType != null)
                {
                    lastEnd = dataType.Decode(json, lastEnd);
                    result.Add(dataType);
                }
                else
                    break;
            }

            return result;
        }
    }
}
