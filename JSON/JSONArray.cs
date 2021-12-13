using System;
using System.Collections.Generic;

namespace DataEncoding.JSON
{
    public class JSONArray : JSONBase
    {
        public List<JSONBase> Content { get; set; } = new List<JSONBase>();

        public JSONArray()
        { }

        public JSONArray(IEnumerable<JSONBase> content)
        {
            Content.AddRange(content);
        }

        public override string Encode()
        {
            string result = "[";

            foreach (JSONBase item in Content)
            {
                result += item.Encode() + ",";
            }

            if (result == "[")
                return "[]";
            else
                return result.Remove(result.Length - 1) + "]";
        }

        public override int Decode(string json, int start)
        {
            int arrayStart = json.IndexOf('[', start);

            if (arrayStart != -1)
            {
                int arrayEnd = FindArrayEnd(json, arrayStart + 1);
                if (arrayEnd != -1)
                {
                    string arrayStr = json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);

                    int lastEnd = 0;
                    JSONBase value;

                    while (true)
                    {
                        value = FromEncoded(arrayStr, lastEnd, out lastEnd);

                        if (value != null)
                        {
                            Content.Add(value);
                        }
                        else
                            break;
                    }

                    return arrayEnd + 1;
                }
                else
                    throw new ArgumentException("End of array not found.");
            }
            else
                throw new ArgumentException("No array found in " + nameof(json) + ".");
        }

        private static int FindArrayEnd(string json, int start)
        {
            int depth = 0;
            bool inString = false;

            for (int i = start; i < json.Length; i++)
            {
                if (json[i] == '\"')
                {
                    if (json[i - 1] != '\\')
                        inString = !inString;
                }
                else if (!inString)
                {
                    if (json[i] == '[')
                        depth++;
                    else if (json[i] == ']')
                    {
                        if (depth > 0)
                            depth--;
                        else
                            return i;
                    }
                }
            }

            return -1;
        }
    }
}
