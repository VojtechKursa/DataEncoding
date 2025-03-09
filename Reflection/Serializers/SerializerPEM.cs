using DataEncoding.PEM;

namespace DataEncoding.Reflection.Serializers
{
    public class SerializerPEM : Serializer<string>
    {
        private readonly SerializerDER der = new SerializerDER();

        public override string Serialize(object obj) => new PEMBlockDER(GetStructureName(obj), der.GetSequence(obj)).Encode();
    }
}
