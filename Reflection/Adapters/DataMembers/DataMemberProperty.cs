using System;
using System.Reflection;

namespace DataEncoding.Reflection.Adapters.DataMembers
{
    internal class DataMemberProperty : IDataMemberInfo
    {
        private PropertyInfo property;

        public DataMemberProperty(PropertyInfo property)
        {
            this.property = property;
        }

        public string Name => property.Name;

        public bool IsPublic => property.GetMethod.IsPublic || property.SetMethod.IsPublic;

        public MemberTypes MemberType => property.MemberType;

        public Type DataType => property.PropertyType;

        public Type DeclaringType => property.DeclaringType;

        public T GetCustomAttribute<T>() where T : Attribute => property.GetCustomAttribute<T>();
        public object GetValue(object obj) => property.GetValue(obj);
        public void SetValue(object obj, object value) => property.SetValue(obj, value);
    }
}
