using System;
using System.Collections.Generic;
using DataEncoding.PEM;
using DataEncoding.Reflection.Data;

namespace DataEncoding.Reflection.Serializers
{
    public class SerializerPEM : Serializer<string, PEMBase>
    {
        private readonly SerializerDER der = new SerializerDER();

        public override string Serialize(object obj) => new PEMBlock(GetStructureName(obj), der.Serialize(obj)).Encode();

        internal override PEMBase SerializeInternal(string classname, List<Tuple<PropertySerializationData, object>> properties) => throw new NotImplementedException();
    }
}
