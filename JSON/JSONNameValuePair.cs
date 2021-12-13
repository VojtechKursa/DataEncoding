using System;

namespace DataEncoding.JSON
{
    public class JSONNameValuePair
    {
        public JSONNameValuePair()
        { }

        public JSONNameValuePair(string name, JSONBase value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get => NameJSON?.Content;
            set => NameJSON = new JSONString(value);
        }
        protected JSONString NameJSON { get; set; } = null;

        public JSONBase Value { get; set; } = null;

        public string Encode()
        {
            return NameJSON.Encode() + ":" + Value.Encode();
        }

        public int Decode(string json, int start)
        {
            JSONString name = new JSONString();
            int nameEnd = name.Decode(json, start);

            JSONBase value = JSONFunctions.GetDatatype(json, nameEnd + 1, out int dataStart);
            if (value != null)
            {
                int result = value.Decode(json, dataStart);

                NameJSON = name;
                Value = value;

                return result;
            }
            else
                throw new ArgumentException("Value not found in " + nameof(json) + ".");
        }

        public static JSONNameValuePair FromEncoded(string json, int start, out int end)
        {
            JSONNameValuePair result = new JSONNameValuePair();

            end = result.Decode(json, start);
            return result;
        }
    }
}
