using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataEncoding.Interfaces;
using DataEncoding.Reflection.Adapters.DataMembers;
using DataEncoding.Reflection.Attributes;
using DataEncoding.Reflection.Data;
using DataEncoding.Reflection.Exceptions;

namespace DataEncoding.Reflection.Serializers
{
    public abstract class Serializer<TSerialized, TInternal> where TInternal : ISupportsEncode<TSerialized>
    {
        private static readonly Dictionary<TypeInfo, StructureSerializationData> cache = new Dictionary<TypeInfo, StructureSerializationData>();
        private readonly HashSet<object> visited = new HashSet<object>();

        public virtual TSerialized Serialize(object obj)
        {
            TSerialized result = SerializeInternal(obj).Encode();

            visited.Clear();

            return result;
        }

        internal TInternal SerializeInternal(object obj)
        {
            if (visited.Contains(obj))
                throw new LoopException();

            visited.Add(obj);

            return SerializeInternal(GetStructureName(obj), GetPropertyValuePairs(obj));
        }
        internal abstract TInternal SerializeInternal(string classname, List<Tuple<PropertySerializationData, object>> properties);

        internal static string GetStructureName(object obj) => GetStructureName(obj.GetType());
        internal static string GetStructureName(Type type) => GetStructureSerializationData(type.GetTypeInfo()).Name;

        private static List<Tuple<PropertySerializationData, object>> GetPropertyValuePairs(object obj)
        {
            Type type = obj.GetType();
            var properties = GetStructureSerializationData(type.GetTypeInfo());

            List<Tuple<PropertySerializationData, object>> propertyValues = new List<Tuple<PropertySerializationData, object>>();

            foreach (var property in properties.SerializableData)
            {
                object value = property.Member.GetValue(obj);

                propertyValues.Add(new Tuple<PropertySerializationData, object>(property, value));
            }

            return propertyValues;
        }

        internal static StructureSerializationData GetStructureSerializationData(TypeInfo type)
        {
            if (cache.TryGetValue(type, out StructureSerializationData cached))
                return cached;

            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            List<Tuple<int?, PropertySerializationData>> result = new List<Tuple<int?, PropertySerializationData>>(members.Length);

            var structureAttribute = type.GetCustomAttribute<SerializableStructureAttribute>() ?? SerializableStructureAttribute.Default;

            foreach (MemberInfo member in members)
            {
                var propertyAttribute = member.GetCustomAttribute<SerializablePropertyAttribute>() ?? SerializablePropertyAttribute.Default;

                if (propertyAttribute.Include == false)
                    continue;

                if (!(
                    (propertyAttribute.Include == true) ||
                    (member.MemberType == MemberTypes.Property && structureAttribute.IncludeProperties) ||
                    (member.MemberType == MemberTypes.Field && structureAttribute.IncludeFields)
                ))
                {
                    continue;
                }

                PropertySerializationData property;
                if (member is FieldInfo f)
                {
                    if (f.IsPublic || structureAttribute.IncludePrivate || propertyAttribute.Include == true)
                        property = PropertySerializationData.FromInfo(new DataMemberField(f), propertyAttribute, structureAttribute);
                    else
                        continue;
                }
                else if (member is PropertyInfo p)
                {
                    if (p.GetMethod.IsPublic || structureAttribute.IncludePrivate || propertyAttribute.Include == true)
                        property = PropertySerializationData.FromInfo(new DataMemberProperty(p), propertyAttribute, structureAttribute);
                    else
                        continue;
                }
                else
                    continue;

                result.Add(new Tuple<int?, PropertySerializationData>(propertyAttribute.Order, property));
            }

            string className = structureAttribute.Name ?? type.Name;

            PropertySerializationData[] properties = result.OrderBy(tup => tup.Item1 ?? int.MaxValue).Select(tup => tup.Item2).ToArray();
            StructureSerializationData data = new StructureSerializationData(className, properties);
            cache.Add(type, data);
            return data;
        }
    }
}
