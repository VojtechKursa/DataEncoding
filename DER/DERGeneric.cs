using System;

namespace DataEncoding.DER
{
    /// <summary>
    /// Represents a generic DER object.
    /// </summary>
    public class DERGeneric : DERBase
    {
        public DERGeneric()
        { }

        public DERGeneric(byte tagClass, bool primitive, int type, byte[] content)
        {
            Type = new DERDataType(tagClass, primitive, type);
            Content = content;
        }

        public DERGeneric(byte tagClass, bool primitive, DataType type, byte[] content) : this(tagClass, primitive, (int)type, content)
        { }

        private byte[] content;
        /// <summary>
        /// Gets or sets the <see cref="Content"/> of the DER object. When the <see cref="Content"/> is set, the <see cref="DERBase.Length"/> is automatically adjusted.
        /// </summary>
        public byte[] Content
        {
            get => content;
            set
            {
                content = value;
                Length = value.Length;
            }
        }

        public override byte[] Encode()
        {
            byte[] lengthEncoded = EncodeLength(Length);
            byte[] result = new byte[Type.TypeBytes.Length + lengthEncoded.Length + Length];

            Array.Copy(Type.TypeBytes, 0, result, 0, Type.TypeBytes.Length);
            Array.Copy(lengthEncoded, 0, result, Type.TypeBytes.Length, lengthEncoded.Length);
            Array.Copy(content, 0, result, Type.TypeBytes.Length + lengthEncoded.Length, content.Length);

            return result;
        }

        public override int Decode(byte[] encoded, int start)
        {
            Type = new DERDataType(encoded, start);

            int[] decodeLengthResult = DecodeLength(encoded, start + Type.TypeBytes.Length);
            Length = decodeLengthResult[0];

            int lengthArrayLength = decodeLengthResult[1];

            content = new byte[Length];
            int valueStart = start + Type.TypeBytes.Length + lengthArrayLength;

            Array.Copy(encoded, valueStart, content, 0, Length);

            return valueStart + Length;
        }
    }
}
