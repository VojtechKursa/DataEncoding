using System;

namespace DataEncoding.JSON
{
    public class JSONNull : JSONBase
    {
        public JSONNull()
        { }

        public override string Encode()
        {
            return "null";
        }

        public override int Decode(string json, int start)
        {
            int valueIndex = json.IndexOf("null", start);

            if (valueIndex != -1)
            {
                return valueIndex + 4;
            }
            else
                throw new ArgumentException("No null found in " + nameof(json) + ".");
        }
    }
}
