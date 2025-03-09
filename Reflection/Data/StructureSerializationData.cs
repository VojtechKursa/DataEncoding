using System.Collections.Generic;

namespace DataEncoding.Reflection.Data
{
    internal class StructureSerializationData
    {
        public string Name { get; }

        private readonly PropertySerializationData[] serializableData;

        public IEnumerable<PropertySerializationData> SerializableData => serializableData;

        public StructureSerializationData(string name, PropertySerializationData[] serializableData)
        {
            Name = name;
            this.serializableData = serializableData;
        }
    }
}
