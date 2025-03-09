using System.Reflection;
using DataEncoding.DER;
using DataEncoding.Reflection.Adapters.DataMembers;
using DataEncoding.Reflection.Attributes;

namespace DataEncoding.Reflection.Data
{
    internal class PropertySerializationData
    {
        public string Name { get; }
        public IDataMemberInfo Member { get; }
        public bool CaseSensitive { get; }

        public bool XmlAttribute { get; }
        public DERDataType DerDataType { get; }

        public PropertySerializationData(string name, IDataMemberInfo member, bool caseSensitive, bool xmlAttribute, DERDataType derDataType)
        {
            Name = name;
            Member = member;
            CaseSensitive = caseSensitive;
            XmlAttribute = xmlAttribute;
            DerDataType = derDataType;
        }

        public static PropertySerializationData FromInfo(IDataMemberInfo info, SerializablePropertyAttribute propertyAttribute = null, SerializableStructureAttribute structureAttribute = null)
        {
            if (structureAttribute == null)
            {
                structureAttribute = info.DeclaringType.GetCustomAttribute<SerializableStructureAttribute>();
            }
            if (propertyAttribute == null)
            {
                propertyAttribute = info.GetCustomAttribute<SerializablePropertyAttribute>();
            }

            string name = null;
            bool caseSensitive;
            DERDataType derDataType;
            bool xmlAttribute;

            ForcedCase forcedCase;

            if (propertyAttribute != null)
            {
                caseSensitive = propertyAttribute.CaseSensitive;

                if (propertyAttribute.Name != null)
                    name = propertyAttribute.Name;

                forcedCase = propertyAttribute.ForcedCase;

                derDataType = propertyAttribute.DerDataType;
                xmlAttribute = propertyAttribute.XmlAttribute;
            }
            else if (structureAttribute != null)
            {
                forcedCase = structureAttribute.ForcedCase;
                caseSensitive = structureAttribute.CaseSensitive;
                derDataType = SerializablePropertyAttribute.Default.DerDataType;
                xmlAttribute = SerializablePropertyAttribute.Default.XmlAttribute;
            }
            else
            {
                var defaults = SerializablePropertyAttribute.Default;
                forcedCase = defaults.ForcedCase;
                caseSensitive = defaults.CaseSensitive;
                derDataType = defaults.DerDataType;
                xmlAttribute = defaults.XmlAttribute;
            }

            if (name == null)
            {
                switch (forcedCase)
                {
                    case ForcedCase.Upper: name = info.Name.ToUpperInvariant(); break;
                    case ForcedCase.Lower: name = info.Name.ToLowerInvariant(); break;
                    case ForcedCase.PreserveSource: name = info.Name; break;
                    default: name = info.Name; break;
                }
            }

            return new PropertySerializationData(name, info, caseSensitive, xmlAttribute, derDataType);
        }
    }
}
