using System;
using System.Collections.Generic;
using DataEncoding.Reflection.Data;
using DataEncoding.Reflection.Exceptions;
using DataEncoding.XML;

namespace DataEncoding.Reflection.Serializers
{
    public class SerializerXML : Serializer<string, XMLElement>
    {
        internal override XMLElement SerializeInternal(string classname, List<Tuple<PropertySerializationData, object>> properties)
        {
            XMLElement result = new XMLElement(classname);

            foreach (var property in properties)
            {
                if (property.Item1.XmlAttribute)
                    result.Attributes.Add(ToAttribute(property.Item1.Name, property.Item2));
                else
                    result.Content.Add(ToElement(property.Item1.Name, property.Item2));
            }

            return result;
        }

        internal XMLAttribute ToAttribute(string name, object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (value is bool b)
            {
                return new XMLAttribute(name, b.ToString());
            }
            else if (value.IsInteger())
            {
                return new XMLAttribute(name, ((long)value).ToString());
            }
            else if (value.IsFloat())
            {
                return new XMLAttribute(name, ((double)value).ToString());
            }
            else if (value.IsCollection() || value.GetType().IsClass)
            {
                throw new InvalidXmlAttributeException(name, value.GetType());
            }
            else
            {
                return new XMLAttribute(name, $"{value}");
            }
        }

        internal XMLElement ToElement(string name, object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (value is bool b)
            {
                return new XMLElement(name, new XMLString(b.ToString()));
            }
            else if (value.IsInteger())
            {
                return new XMLElement(name, new XMLString(((long)value).ToString()));
            }
            else if (value.IsFloat())
            {
                return new XMLElement(name, new XMLString(((double)value).ToString()));
            }
            else if (value.IsCollection())
            {
                XMLContentCollection collection = new XMLContentCollection();
                ulong counter = 0;
                foreach (object val in value.ToEnumerable())
                {
                    collection.Add(ToElement(counter.ToString(), val));

                    counter++;
                }

                return new XMLElement(name, collection);
            }
            else if (value.GetType().IsClass)
            {
                XMLElement element = SerializeInternal(value);
                element.Name = name;
                return element;
            }
            else
            {
                return new XMLElement(name, new XMLString($"{value}"));
            }
        }
    }
}
