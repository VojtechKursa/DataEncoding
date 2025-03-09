using System;
using System.Collections.Generic;
using System.Text;
using DataEncoding.DER;
using DataEncoding.Reflection.Data;

namespace DataEncoding.Reflection.Serializers
{
    public class SerializerDER : Serializer<byte[]>
    {
        public override byte[] Serialize(object obj) => GetSequence(obj).Encode();

        internal DERSequence GetSequence(object obj) => GetSequence(GetPropertyValuePairs(obj));

        internal DERSequence GetSequence(IEnumerable<Tuple<PropertySerializationData, object>> properties)
        {
            List<DERBase> contents = new List<DERBase>();

            foreach (var property in properties)
            {
                DERDataType dataType = property.Item1.DerDataType;
                object value = property.Item2;

                contents.Add(Encode(value, dataType));
            }

            return new DERSequence(contents);
        }

        private DERBase Encode(object value, DERDataType dataType = null)
        {
            if (value == null)
                dataType = new DERDataType(0, true, DataType.Null);

            if (dataType == null)
            {
                if (value.IsInteger())
                    dataType = new DERDataType(0, true, DataType.Integer);
                else if (value.IsFloat())
                    dataType = new DERDataType(0, true, DataType.Float);
                else if (value is bool)
                    dataType = new DERDataType(0, true, DataType.Boolean);
                else if (value is char || value is string)
                    dataType = new DERDataType(0, true, DataType.UTF8String);
                else if (value.IsCollection())
                    dataType = new DERDataType(0, false, DataType.Sequence);
                else if (value.GetType().IsClass)
                    return GetSequence(value);
                else
                    throw new Exception();
            }

            byte[] content;

            switch (dataType.DataType)
            {
                case DataType.Null:
                case DataType.EndOfContent:
                    content = new byte[0];
                    break;
                case DataType.Boolean:
                    content = new byte[1] { (byte)((bool)value == true ? 1 : 0) };
                    break;
                case DataType.Integer:
                case DataType.Float:
                    content = value.GetNumberBytes();
                    if (content == null)
                        throw new Exception();
                    break;
                case DataType.CharacterString:
                    content = Encoding.ASCII.GetBytes((string)value);
                    break;
                case DataType.UTF8String:
                    content = Encoding.UTF8.GetBytes((string)value);
                    break;
                case DataType.Sequence:
                    List<DERBase> list = new List<DERBase>();
                    foreach (object val in value.ToEnumerable())
                    {
                        list.Add(Encode(val));
                    }
                    return new DERSequence(list);
                default:
                    throw new Exception();
            }

            return new DERGeneric(dataType.TagClass, dataType.Primitive, dataType.DataType, content);
        }
    }
}
