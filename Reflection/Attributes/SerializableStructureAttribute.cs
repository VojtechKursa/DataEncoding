using System;
using DataEncoding.Reflection.Data;

namespace DataEncoding.Reflection.Attributes
{
    /// <summary>
    /// Sets attributes of a data structure for serialization and deserialization using DataEncoding library.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SerializableStructureAttribute : Attribute
    {
        /// <summary>
        /// Instance of <see cref="SerializableStructureAttribute"/> with default values.
        /// </summary>
        public static readonly SerializableStructureAttribute Default = new SerializableStructureAttribute();

        /// 
        /// <summary>
        /// Name of this data structure. Appears as a tag in XML and in header and footer in PEM.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether to include fields of this data structure in serialized output.
        /// </summary>
        public bool IncludeFields { get; set; }

        /// <summary>
        /// Whether to include properties of this data structure in serialized output.
        /// </summary>
        public bool IncludeProperties { get; set; }

        /// <summary>
        /// Whether to include private properties and fields of this data structure in serialized output.
        /// </summary>
        public bool IncludePrivate { get; set; }

        /// <summary>
        /// Whether to use case-sensitive matching when deserializing properties and fields in this data structure.
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Whether to force certain casing when serializing properties and fields in this data structure.
        /// Also taken into account when deserializing, if <see cref="CaseSensitive"/> is <see langword="true"/>.
        /// </summary>
        public ForcedCase ForcedCase { get; set; }

        /// <param name="Name">Name of this data structure. Appears as a tag in XML and in header and footer in PEM.</param>
        /// <param name="IncludeFields">Whether to include fields of this data structure in serialized output.</param>
        /// <param name="IncludeProperties">Whether to include properties of this data structure in serialized output.</param>
        /// <param name="IncludePrivate">Whether to include private properties and fields of this data structure in serialized output.</param>
        /// <param name="CaseSensitive">Whether to use case-sensitive matching when deserializing properties and fields in this data structure.</param>
        /// <param name="ForcedCase">Whether to force certain casing when serializing properties and fields in this data structure. Also taken into account when deserializing, if <paramref name="CaseSensitive"/> is <see langword="true"/>.</param>
        public SerializableStructureAttribute(string Name = null, bool IncludeFields = true, bool IncludeProperties = true, bool IncludePrivate = false, bool CaseSensitive = true, ForcedCase ForcedCase = ForcedCase.PreserveSource)
        {
            this.Name = Name;
            this.IncludeFields = IncludeFields;
            this.IncludeProperties = IncludeProperties;
            this.IncludePrivate = IncludePrivate;
            this.CaseSensitive = CaseSensitive;
            this.ForcedCase = ForcedCase;
        }
    }
}
