using System;
using System.Collections.Generic;

namespace DataEncoding.JSON
{
    /// <summary>
    /// Provides some useful functions for working with JSON-encoded strings
    /// </summary>
    public static class JSONFunctions
    {
        /// <summary>
        /// Decodes the given JSON-encoded string even if it's a text file with many values in the highest level.
        /// </summary>
        /// <param name="json">The JSON-encoded string to decode.</param>
        /// <param name="start">The index from which to start decoding.</param>
        /// <returns>A list of all decoded top-level values in the JSON-encoded string.</returns>
        public static List<JSONBase> DecodeString(string json, int start)
        {
            List<JSONBase> result = new List<JSONBase>();

            JSONBase dataType;
            int lastEnd = start;
            while (true)
            {
                dataType = GetDatatype(json, lastEnd, out int dataStart);
                if (dataType != null)
                {
                    lastEnd = dataType.Decode(json, dataStart);
                    result.Add(dataType);
                }
                else
                    break;
            }

            return result;
        }

        /// <summary>
        /// Beautifies the given JSON-encoded string.
        /// </summary>
        /// <param name="json">JSON-encoded string to beautify.</param>
        /// <returns>The beautified JSON-encoded string.</returns>
        public static string Beautify(string json)
        {
            string result = "";
            int depth = 0;
            bool inString = false;

            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '\"')
                {
                    if (i > 0)
                    {
                        if (json[i - 1] != '\\')
                            inString = !inString;
                    }
                    else
                        inString = !inString;
                }

                if (!inString)
                {
                    switch (json[i])
                    {
                        case '{':
                        case '[':
                            depth++;
                            result += json[i] + Environment.NewLine + MultiplyString("\t", depth);
                            break;
                        case '}':
                        case ']':
                            depth--;
                            result += Environment.NewLine + MultiplyString("\t", depth) + json[i];
                            break;
                        case ',':
                            result += ',' + Environment.NewLine + MultiplyString("\t", depth);
                            break;
                        case ':':
                            result += ": ";
                            break;
                        default:
                            result += json[i];
                            break;
                    }
                }
                else
                {
                    result += json[i];
                }

            }

            return result;
        }

        private static string MultiplyString(string format, int times)
        {
            string result = "";

            for (int i = 0; i < times; i++)
            {
                result += format;
            }

            return result;
        }

        /// <summary>
        /// Minifies the given JSON-encoded string.
        /// </summary>
        /// <param name="json">The JSON-encoded string to minify.</param>
        /// <returns>The minified JSON-encoded string.</returns>
        public static string Minify(string json)
        {
            string result = "";
            bool inString = false;

            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '\"')
                {
                    if (i > 0)
                    {
                        if (json[i - 1] != '\\')
                            inString = !inString;
                    }
                    else
                        inString = !inString;
                }

                if (!inString)
                {
                    switch (json[i])
                    {
                        case ' ':
                        case '\n':
                        case '\r':
                        case '\t': break;
                        default: result += json[i]; break;
                    }
                }
                else
                {
                    result += json[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the datatype of the first detected data and returns a new JSON object accordingly to that datatype.
        /// </summary>
        /// <param name="json">The JSON string in which to search for data.</param>
        /// <param name="start">The index from which to start the search.</param>
        /// <returns>A JSON object that represents the detected datatype.</returns>
        internal static JSONBase GetDatatype(string json, int start, out int dataStartIndex)
        {
            for (int i = start; i < json.Length; i++)
            {
                dataStartIndex = i;

                switch (json[i])
                {
                    case '\"': return new JSONString();
                    case '{': return new JSONObject();
                    case '[': return new JSONArray();
                    case 't':
                        if (json.Substring(i, 4) == "true")
                            return new JSONBool();
                        break;
                    case 'f':
                        if (json.Substring(i, 5) == "false")
                            return new JSONBool();
                        break;
                    case 'n':
                        if (json.Substring(i, 4) == "null")
                            return new JSONNull();
                        break;
                    default:
                        if ((json[i] >= 48 && json[i] <= 57) || json[i] == '+' || json[i] == '-')
                            return new JSONNumber();
                        break;
                }
            }

            dataStartIndex = -1;
            return null;
        }
    }
}
