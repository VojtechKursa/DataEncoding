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

            JSONBase value = JSONBase.GetDatatype(json, nameEnd + 1);
            if (value != null)
            {
                int result = value.Decode(json, nameEnd + 1);

                NameJSON = name;
                Value = value;

                return result;
            }
            else
                throw new ArgumentException("Value not found in " + nameof(json) + ".");
        }
    }
}
