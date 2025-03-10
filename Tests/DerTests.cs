using DataEncoding.DER;

namespace Tests;

public class DerTests
{
    [Fact]
    public void Serialization_Simple()
    {
        DERGeneric number = new(0, true, DataType.Integer, [0, 0, 0, 1]);
        byte[] encodedNumber = number.Encode();

        Assert.Equal([2, 4, 0, 0, 0, 1], encodedNumber);

        DERGeneric end = new(0, true, DataType.EndOfContent, []);
        byte[] encodedEnd = end.Encode();

        Assert.Equal([0, 0], encodedEnd);
    }

    [Fact]
    public void Serialization_Sequence()
    {
        DERSequence sequence = new([
            new DERGeneric(0, true, DataType.Integer, [0,0,0,1]),
            new DERGeneric(0, true, DataType.Boolean, [1]),
            new DERGeneric(0, true, DataType.Null, []),
        ]);
        byte[] sequenceEncoded = sequence.Encode();

        byte[] reference = [(1 << 5) | 16, 11, 2, 4, 0, 0, 0, 1, 1, 1, 1, 5, 0];

        Assert.Equal(reference, sequenceEncoded);
    }


    [Fact]
    public void Deserialization()
    {
        DERSequence reference = new([
            new DERGeneric(0, true, DataType.Integer, [0,0,0,1]),
            new DERGeneric(0, true, DataType.Boolean, [1]),
            new DERGeneric(0, true, DataType.Null, []),
        ]);

        byte[] input = [(1 << 5) | 16, 11, 2, 4, 0, 0, 0, 1, 1, 1, 1, 5, 0];
        DERBase decoded = DERBase.FromEncoded(input, 0, out int end);

        Assert.Equal(13, end);

        DERSequence sequence = Assert.IsType<DERSequence>(decoded);

        Utils.DerSequencesEqual(reference, sequence);
    }
}
