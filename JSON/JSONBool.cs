using System;

namespace DataEncoding.JSON
{
    public class JSONBool : JSONBase
    {
        public JSONBool()
        { }

        public JSONBool(bool value)
        {
            Content = value;
        }

        public bool Content { get; set; }

        public override string Encode()
        {
            if (Content)
                return "true";
            else
                return "false";
        }

        public override int Decode(string json, int start)
        {
            int valueStart = json.IndexOfAny(new char[] { 't', 'f' }, start);

            if (valueStart != -1)
            {
                if (json.Substring(valueStart, 4) == "true")
                {
                    Content = true;
                    return valueStart + 4;
                }
                else if (json.Substring(valueStart, 5) == "false")
                {
                    Content = false;
                    return valueStart + 5;
                }
            }

            throw new ArgumentException("No true nor false was found in the " + nameof(json) + ".");
        }
    }
}
