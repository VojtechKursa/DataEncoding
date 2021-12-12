using System;

namespace DataEncoding.JSON
{
    public class JSONObject : JSONBase
    {
        /// <summary>
        /// Gets or sets the content of the <see cref="JSONObject"/>.
        /// </summary>
        public JSONNameValuePairCollection Content { get; set; } = new JSONNameValuePairCollection();

        public JSONObject()
        { }

        public JSONObject(JSONNameValuePairCollection content)
        {
            Content = content;
        }

        public override string Encode()
        {
            string result = "{";

            foreach (JSONNameValuePair nameValuePair in Content)
            {
                result += nameValuePair.Encode() + ",";
            }

            if (result == "{")
                result += "}";
            else
                result = result.Remove(result.Length - 1) + "}";

            return result;
        }

        public override int Decode(string json, int start)
        {
            int objectStart = json.IndexOf('{', start);
            if (objectStart == -1)
                throw new ArgumentException("Beginning of the object ({) not found in " + nameof(json) + ".");

            int objectEnd = FindObjectEnd(json, objectStart + 1);
            if (objectEnd == -1)
                throw new ArgumentException("End of the object (}) not found in " + nameof(json) + ".");

            string objectString = json.Substring(objectStart, objectEnd - objectStart + 1);

            int nameStart, lastEnd = 0;
            while (true)
            {
                nameStart = objectString.IndexOf('\"', lastEnd);

                if (nameStart == -1)
                    break;
                else
                {
                    JSONNameValuePair pair = new JSONNameValuePair();
                    lastEnd = pair.Decode(objectString, nameStart);
                    Content.Add(pair);
                }
            }

            return objectEnd + 1;
        }

        private int FindObjectEnd(string json, int start)
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
                    if (json[i] == '{')
                        depth++;
                    else if (json[i] == '}')
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
