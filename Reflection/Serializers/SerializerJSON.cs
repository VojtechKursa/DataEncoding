using System;
using System.Collections.Generic;
using DataEncoding.JSON;
using DataEncoding.Reflection.Data;

namespace DataEncoding.Reflection.Serializers
{
    public class SerializerJSON : Serializer<string, JSONBase>
    {
        internal override JSONBase SerializeInternal(string _, List<Tuple<PropertySerializationData, object>> properties)
        {
            JSONNameValuePairCollection collection = new JSONNameValuePairCollection();

            foreach (var property in properties)
            {
                collection.Add(property.Item1.Name, Convert(property.Item2));
            }

            return new JSONObject(collection);
        }

        internal JSONBase Convert(object value)
        {
            if (value == null)
            {
                return new JSONNull();
            }
            else if (value is bool b)
            {
                return new JSONBool(b);
            }
            else if (value.IsNumeric())
            {
                return new JSONNumber((double)value);
            }
            else if (value.IsCollection())
            {
                JSONArray array = new JSONArray();
                foreach (object val in value.ToEnumerable())
                {
                    array.Content.Add(Convert(val));
                }
                return array;
            }
            else if (value.GetType().IsClass)
            {
                return SerializeInternal(value);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
