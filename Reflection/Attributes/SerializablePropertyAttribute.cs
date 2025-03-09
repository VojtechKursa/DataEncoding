using System;
using DataEncoding.DER;
using DataEncoding.Reflection.Data;

namespace DataEncoding.Reflection.Attributes
{
    /// <summary>
    /// Sets attributes of a data field for serialization and deserialization using DataEncoding library.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerializablePropertyAttribute : Attribute
    {
        /// <summary>
        /// Instance of <see cref="SerializablePropertyAttribute"/> with default values.
        /// </summary>
        public static readonly SerializablePropertyAttribute Default = new SerializablePropertyAttribute();

        /// <summary>
        /// Name of the property to use in textual formats.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Custom order in which this property appears in serialized form.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Whether to forcefully include (<see langword="true"/>), forcefully exclude (<see langword="false"/>) or use structure's defaults (<see langword="null"/>) for this property.
        /// </summary>
        public bool? Include { get; set; }

        /// <summary>
        /// Whether to use case-sensitive matching when deserializing this property or field.
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Whether to force certain casing when serializing this property. Also taken into account when deserializing, if <see cref="CaseSensitive"/> is <see langword="true"/>.
        /// </summary>
        public ForcedCase ForcedCase { get; set; }

        /// <summary>
        /// Valid only for XML. Whether to represent the property as attribute, rather than element. Ignored if annotated property isn't a value type.
        /// </summary>
        public bool XmlAttribute { get; set; }

        /// <summary>
        /// Custom DataType to use in the DER format.
        /// </summary>
        public DERDataType DerDataType { get; set; }

        /// <param name="Name">Name of the property to use in textual formats.</param>
        /// <param name="Order">Custom order in which this property appears in serialized form.</param>
        /// <param name="Include">Whether to forcefully include (<see langword="true"/>), forcefully exclude (<see langword="false"/>) or use structure's defaults (<see langword="null"/>) for this property.</param>
        /// <param name="CaseSensitive">Whether to use case-sensitive matching when deserializing this property or field.</param>
        /// <param name="ForcedCase">Whether to force certain casing when serializing this property. Also taken into account when deserializing, if <see cref="CaseSensitive"/> is <see langword="true"/>.</param>
        /// <param name="XmlAttribute">Valid only for XML. Whether to represent the property as attribute, rather than element. Ignored if annotated property isn't a value type.</param>
        /// <param name="DerDataType">Custom DataType to use in the DER format.</param>
        public SerializablePropertyAttribute(string Name = null, int? Order = null, bool? Include = null, bool CaseSensitive = true, ForcedCase ForcedCase = ForcedCase.PreserveSource, bool XmlAttribute = false, DERDataType DerDataType = null)
        {
            this.Name = Name;
            this.Order = Order;
            this.Include = Include;
            this.CaseSensitive = CaseSensitive;
            this.ForcedCase = ForcedCase;
            this.XmlAttribute = XmlAttribute;
            this.DerDataType = DerDataType;
        }
    }
}
