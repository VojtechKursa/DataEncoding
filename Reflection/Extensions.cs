using System;
using System.Collections.Generic;
using System.Linq;

namespace DataEncoding.Reflection
{
    internal static class Extensions
    {
        private static readonly HashSet<Type> integers = new HashSet<Type>()
        {
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong)
        };
        private static readonly HashSet<Type> floats = new HashSet<Type>()
        {
            typeof(float), typeof(double)
        };
        private static readonly HashSet<Type> numeric = new HashSet<Type>(integers.Union(floats));

        public static bool IsNumeric(this Type type) => numeric.Contains(type);
        public static bool IsNumeric(this object obj) => IsNumeric(obj.GetType());

        public static bool IsInteger(this Type type) => integers.Contains(type);
        public static bool IsInteger(this object obj) => IsInteger(obj.GetType());

        public static bool IsFloat(this Type type) => floats.Contains(type);
        public static bool IsFloat(this object obj) => IsFloat(obj.GetType());

        public static bool IsCollection(this Type type) => type.GetInterfaces().Where(t => t == typeof(System.Collections.IEnumerable)).Count() > 0;
        public static bool IsCollection(this object obj) => obj is System.Collections.IEnumerable;

        public static IEnumerable<object> ToEnumerable(this object obj)
        {
            if (obj is System.Collections.IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    yield return item;
            }
        }

        public static byte[] GetNumberBytes(this object obj)
        {
            if (obj.IsInteger())
            {
                if (obj is byte b)
                    return BitConverter.GetBytes(b);
                if (obj is sbyte sb)
                    return BitConverter.GetBytes(sb);
                if (obj is short s)
                    return BitConverter.GetBytes(s);
                if (obj is ushort us)
                    return BitConverter.GetBytes(us);
                if (obj is int i)
                    return BitConverter.GetBytes(i);
                if (obj is uint ui)
                    return BitConverter.GetBytes(ui);
                if (obj is long l)
                    return BitConverter.GetBytes(l);
                if (obj is ulong ul)
                    return BitConverter.GetBytes(ul);
            }
            else if (obj.IsFloat())
            {
                if (obj is float f)
                    return BitConverter.GetBytes(f);
                if (obj is double d)
                    return BitConverter.GetBytes(d);
            }

            return null;
        }
    }
}
