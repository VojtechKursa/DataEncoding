using System;

namespace DataEncoding.JSON
{
    public class JSONString : JSONBase
    {
        public string Content { get; set; } = null;

        public JSONString()
        { }

        public JSONString(string value)
        {
            Content = value;
        }

        //Add escaping
        public override string Encode()
        {
            string result = String.Format("\"{0}\"", Content);

            Encoded = result;
            return result;
        }

        //Add unescaping of characters and decoding of escaped Unicode characters
        public override int Decode(string json, int start)
        {
            int stringStart = json.IndexOf('\"', start);
            if (stringStart == -1)
                throw new ArgumentException("Start of the string (\") not found in " + nameof(json) + ".");

            int stringEnd = json.IndexOf('\"', stringStart + 1);
            if (stringEnd == -1)
                throw new ArgumentException("End of the string (\") not found in " + nameof(json) + ".");

            Content = json.Substring(stringStart + 1, stringEnd - stringStart - 1);

            return stringEnd + 1;
        }

        internal static int FindEndOfString(string json, int start)
        {
            for (int i = start > 0 ? start : 1; i < json.Length; i++)
            {
                if (json[i] == '\"' && json[i - 1] != '\\')
                    return i;
            }

            return -1;
        }
    }
}
