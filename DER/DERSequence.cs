using System;
using System.Collections.Generic;

namespace DataEncoding.DER
{
    /// <summary>
    /// Represents a DER Sequence.<br />
    /// Note: The DER sequence does not have the <see cref="DERBase.Length"/> value available until either of <see cref="Encode()"/> or <see cref="Decode(byte[], int)"/> method is called.
    /// </summary>
    public class DERSequence : DERBase
    {
        public DERSequence()
        {
            Type = new DERDataType(0, false, DataType.Sequence);
        }

        public DERSequence(List<DERBase> content) : this()
        {
            Content = content;
        }

        public List<DERBase> Content { protected set; get; } = new List<DERBase>();

        public override byte[] Encode()
        {
            List<byte[]> encodedValues = new List<byte[]>();
            byte[] currentValue;
            int valuesLength = 0;

            foreach (DERBase value in Content)
            {
                currentValue = value.Encode();
                encodedValues.Add(currentValue);
                valuesLength += currentValue.Length;
            }

            byte[] lengthEncoded = EncodeLength(valuesLength);

            byte[] result = new byte[Type.TypeBytes.Length + lengthEncoded.Length + valuesLength];

            Array.Copy(Type.TypeBytes, 0, result, 0, Type.TypeBytes.Length);
            Array.Copy(lengthEncoded, 0, result, Type.TypeBytes.Length, lengthEncoded.Length);

            int currentIndex = Type.TypeBytes.Length + lengthEncoded.Length;
            foreach (byte[] encodedValue in encodedValues)
            {
                for (int i = 0; i < encodedValue.Length; i++)
                {
                    result[currentIndex++] = encodedValue[i];
                }
            }

            Length = valuesLength;

            return result;
        }

        public override int Decode(byte[] encoded, int start)
        {
            Type = new DERDataType(encoded, start);

            if (Type.DataType != DataType.Sequence)
                throw new ArgumentException("The input data don't represent a " + nameof(DERSequence) + " therefore can't be decoded using the Decoder for it.");
            else
            {
                int[] length = DecodeLength(encoded, start + Type.TypeBytes.Length);
                Length = length[0];
                int lengthArrayLength = length[1];

                int valueStartIndex = start + Type.TypeBytes.Length + lengthArrayLength;

                int lastEnd = valueStartIndex;
                while (lastEnd < valueStartIndex + Length)
                {
                    Content.Add(FromEncoded(encoded, lastEnd, out lastEnd));
                }

                return lastEnd;
            }
        }
    }
}
