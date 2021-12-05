using System;
using System.Collections.Generic;

namespace DataEncoding.DER
{
    public enum DataType
    {
        EndOfContent = 0,
        Boolean = 1,
        Integer = 2,
        BitString = 3,
        OctetString = 4,
        Null = 5,
        ObjectIdentifier = 6,
        ObjectDescriptor = 7,
        External = 8,
        Float = 9,
        Enumerated = 10,
        EmbeddedPDV = 11,
        UTF8String = 12,
        RelativeOID = 13,
        Time = 14,
        Reserved = 15,
        Sequence = 16,
        Set = 17,
        NumericString = 18,
        PrintableString = 19,
        T61String = 20,
        VideotexString = 21,
        IA5String = 22,
        UTCTime = 23,
        GeneralizedTime = 24,
        GraphicString = 25,
        VisibleString = 26,
        GeneralString = 27,
        UniversalString = 28,
        CharacterString = 29,
        BMPString = 30,
        Date = 31,
        TimeOfDay = 32,
        DateTime = 33,
        Duration = 34,
        OID_IRI = 35,
        RelativeOID_IRI = 36,
        Unknown = 99
    }

    public class DERDataType
    {
        protected DERDataType(byte tagClass, bool primitive)
        {
            TagClass = tagClass;
            Primitive = primitive;
        }

        public DERDataType(byte tagClass, bool primitive, int typeNumber) : this(tagClass, primitive)
        {
            TypeNumber = typeNumber;
        }

        public DERDataType(byte tagClass, bool primitive, DataType dataType) : this(tagClass, primitive)
        {
            DataType = dataType;
        }

        public DERDataType(byte[] encoded, int start)
        {
            TagClass = (byte)(encoded[start] >> 6);
            Primitive = !Convert.ToBoolean((encoded[start] & 0b00100000) >> 5);

            TypeNumber = ToInt(encoded, start);
        }

        public byte TagClass { get; protected set; }
        public bool Primitive { get; protected set; }

        private int typeNumber;
        public int TypeNumber
        {
            get => typeNumber;
            set
            {
                typeNumber = value;

                if (value <= 36)
                    dataType = (DataType)value;
                else
                    dataType = DataType.Unknown;

                typeBytes = ToByte(value);
            }
        }

        private DataType dataType;
        public DataType DataType
        {
            get => dataType;
            set
            {
                dataType = value;

                if (value != DataType.Unknown)
                {
                    typeNumber = (int)value;
                    typeBytes = ToByte(typeNumber);
                }
                else
                {
                    typeNumber = -1;
                    typeBytes = null;
                }
            }
        }

        private byte[] typeBytes;
        public byte[] TypeBytes
        {
            get => typeBytes;
            set
            {
                typeBytes = value;

                typeNumber = ToInt(value, 0);

                if (typeNumber < 37)
                    dataType = (DataType)typeNumber;
                else
                    dataType = DataType.Unknown;
            }
        }

        private static int ToInt(byte[] type, int start)
        {
            if ((type[start] & 0b00011111) == 0b00011111)
            {
                List<byte> bytes = new List<byte>();

                int i = start + 1;
                while ((type[i] & 0x80) != 0)
                {
                    bytes.Add((byte)(type[i++] & 0x7F));
                }
                bytes.Add((byte)(type[i] & 0x7F));

                int typeNum = 0;

                for (int x = 0; x < bytes.Count; x++)
                {
                    typeNum += bytes[bytes.Count - 1 - i] << (i * 7);
                }

                return typeNum;
            }
            else
                return type[start] & 0b00011111;
        }

        private byte[] ToByte(int type)
        {
            if (type > 30)
            {
                List<byte> result = new List<byte>();
                int temp = type;

                while (temp != 0)
                {
                    result.Add((byte)(temp & 0x7F));
                    temp >>= 7;
                }

                byte firstByte = (byte)(TagClass << 6);

                if (!Primitive)
                    firstByte |= 0b00100000;

                firstByte |= 0b00011111;

                result.Reverse();
                byte[] resultArray = new byte[result.Count + 1];

                Array.Copy(result.ToArray(), 0, resultArray, 1, result.Count);
                resultArray[0] = firstByte;

                return resultArray;
            }
            else
                return new byte[] { (byte)(TagClass << 6 | Convert.ToInt32(!Primitive) << 5 | type & 0x1F) };
        }
    }
}
