using System;

namespace DataEncoding.DER
{
    /// <summary>
    /// Represents the base of all DER objects in the DataEncoding library.
    /// </summary>
    public abstract class DERBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the DER object.
        /// </summary>
        public DERDataType Type { protected set; get; }

        /// <summary>
        /// Gets or sets the <see cref="Length"/> of the DER object's value.
        /// </summary>
        public int Length { protected set; get; }

        #endregion

        #region Methods
        #region Abstract methods

        /// <summary>
        /// Encodes the value currently stored inside the DER object into a DER byte array.
        /// </summary>
        /// <returns>The encoded byte array.</returns>
        public abstract byte[] Encode();

        /// <summary>
        /// Decodes the given byte array into a value and sets it as Content to the given object.<br />
        /// Also sets the <see cref="Length"/> and <see cref="Type"/> values.
        /// </summary>
        /// <param name="encoded">The byte array to decode.</param>
        /// <param name="start">An index in the encoded array from which the object starts.</param>
        /// <returns>The index at which the decoding process stopped (the index of the next byte after the decoded value).</returns>
        public abstract int Decode(byte[] encoded, int start);

        #endregion

        #region Public static methods

        /// <summary>
        /// Creates a new instance of <see cref="DERBase"/> based on the input data.<br />
        /// </summary>
        /// <param name="encoded">The encoded data that contain the desired object.</param>
        /// <param name="start">The index from which to start decoding (should be the index of the first byte of the type field).</param>
        /// <param name="end">The index at which the decoding process stopped (the index of the next byte after the decoded value).</param>
        /// <returns>A new instance of <see cref="DERBase"/> with the decoded value inside. (Retyping to the correct datatype is required to access the value).</returns>
        public static DERBase FromEncoded(byte[] encoded, int start, out int end)
        {
            DERBase result = GetType(encoded, start);

            end = result.Decode(encoded, start);
            return result;
        }

        #endregion

        #region Support methods

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

        protected static DERBase GetType(byte[] encoded, int start)
        {
            DERDataType type = new DERDataType(encoded, start);
            DERBase result;

            switch (type.DataType)
            {
                case DataType.Sequence: result = new DERSequence(); break;
                default: result = new DERGeneric(); break;
            }

            return result;
        }

        #endregion
        #endregion
    }
}
