using System;

namespace DataEncoding.Reflection.Exceptions
{
    public class InvalidXmlAttributeException : Exception
    {
        public InvalidXmlAttributeException(string name, Type type)
            : base($"Element named {name} of type {type.Name} cannot be XML attribute. Only primitive types allowed.")
        { }
    }
}
