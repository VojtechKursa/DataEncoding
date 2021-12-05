﻿using System;

namespace DataEncoding.DER
{
    /// <summary>
    /// Represents the base of all DER objects in the DataEncoding library.
    /// </summary>
    public abstract class DERBase
    {
        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the DER object.
        /// </summary>
        public DERDataType Type { protected set; get; }

        /// <summary>
        /// Gets or sets the <see cref="Length"/> of the DER object's value.
        /// </summary>
        public int Length { protected set; get; }

        /// <summary>
        /// Gets or sets the <see cref="Encoded"/> value. (Obtained after calling either <see cref="Encode()"/> or <see cref="Decode(byte[])"/> method).
        /// </summary>
        public byte[] Encoded { protected set; get; }

        /// <summary>
        /// Encodes the value currently stored inside the DER object into a DER byte array.<br />
        /// Also sets the <see cref="Encoded"/> value of the object.
        /// </summary>
        /// <returns>The encoded byte array.</returns>
        public abstract byte[] Encode();

        /// <summary>
        /// Decodes the given byte array into a value and sets it as Content to the given object.<br />
        /// Also sets the <see cref="Encoded"/>, <see cref="Length"/> and <see cref="Type"/> values.
        /// </summary>
        /// <param name="encoded">The byte array to decode.</param>
        /// <param name="start">An index in the encoded array from which the object starts.</param>
        /// <returns>The amount of bytes used from the input during the decoding process (i.e. the total length of the T-L-V structure).</returns>
        public abstract int Decode(byte[] encoded, int start);

        /// <summary>
        /// Encodes the given length into a byte array that represents that length according to the DER standard.
        /// </summary>
        /// <param name="length">The length to encode.</param>
        /// <returns>The byte array that represents the given length according to the DER standard.</returns>
        protected static byte[] EncodeLength(int length)
        {
            if (length < 0)
                throw new ArgumentException(nameof(length) + " was less than 0. Expected a value >= 0.");
            else
            {
                if (length < 128)
                {
                    return new byte[] { (byte)length };
                }
                else
                {
                    byte bytesNeeded;

                    if (length < 256)
                        bytesNeeded = 1;
                    else if (length < 65536)
                        bytesNeeded = 2;
                    else if (length < 16777216)
                        bytesNeeded = 3;
                    else
                        bytesNeeded = 4;

                    byte[] result = new byte[1 + bytesNeeded];

                    for (int i = 1; i < result.Length; i++)
                    {
                        result[result.Length - i] = (byte)((length >> ((i - 1) * 8)) & 0xFF);
                    }

                    result[0] = (byte)(bytesNeeded | 0x80);

                    return result;
                }
            }
        }

        /// <summary>
        /// Decodes the length of a the value field from the given encoded data.<br />
        /// Note: The entire encoded byte array that's relevant to the given object must be passed to the method.<br />
        /// Throws a <see cref="NotSupportedException"/> when the length of the length field takes more than 5 bytes. (i.e. The value field takes up more than 2^32 bytes)
        /// </summary>
        /// <param name="encoded">The encoded byte array that represents the DER encoded object.</param>
        /// <param name="lengthStart">The index in encoded array at which the length field starts.</param>
        /// <returns>
        /// An array of <see cref="int"/>:<br />
        /// Index 0 - The length of the value field.<br />
        /// Index 1 - The total length of the length field.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="NotSupportedException"/>
        protected static int[] DecodeLength(byte[] encoded, int lengthStart)
        {
            if (encoded == null)
                throw new ArgumentNullException(nameof(encoded) + " was null.");
            else if (encoded[lengthStart] < 128)
                return new int[] { encoded[lengthStart], 1 };
            else
            {
                int lengthArrayLength = encoded[lengthStart] & 0x7F;

                if (lengthArrayLength == 0)
                    throw new NotSupportedException("Indefinite DER structures are not supported by this library.");
                else if (lengthArrayLength > 4)
                    throw new NotSupportedException("Length field requires more than 32 bits, such lengths are currently unsupported by this library.");
                else
                {
                    int length = 0;

                    for (int i = 0; i < lengthArrayLength; i++)
                    {
                        length += encoded[lengthStart + 1 + i] << ((lengthArrayLength - 1 - i) * 8);
                    }

                    return new int[] { length, lengthArrayLength + 1 };
                }
            }
        }
    }
}
