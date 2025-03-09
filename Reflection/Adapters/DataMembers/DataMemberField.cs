using System;
using System.Reflection;

namespace DataEncoding.Reflection.Adapters.DataMembers
{
    internal class DataMemberField : IDataMemberInfo
    {
        private readonly FieldInfo field;

        public DataMemberField(FieldInfo field)
        {
            this.field = field;
        }

        public string Name => field.Name;

        public bool IsPublic => field.IsPublic;

        public MemberTypes MemberType => field.MemberType;

        public Type DataType => field.FieldType;

        public Type DeclaringType => field.DeclaringType;

        public T GetCustomAttribute<T>() where T : Attribute => field.GetCustomAttribute<T>();
        public object GetValue(object obj) => field.GetValue(obj);
        public void SetValue(object obj, object value) => field.SetValue(obj, value);
    }
}
