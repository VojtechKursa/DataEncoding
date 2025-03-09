using System;
using System.Reflection;

namespace DataEncoding.Reflection.Adapters.DataMembers
{
    internal interface IDataMemberInfo
    {
        string Name { get; }
        bool IsPublic { get; }
        MemberTypes MemberType { get; }
        Type DataType { get; }
        Type DeclaringType { get; }

        object GetValue(object obj);
        void SetValue(object obj, object value);
        T GetCustomAttribute<T>() where T : Attribute;
    }
}
