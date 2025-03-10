using DataEncoding.DER;
using DataEncoding.PEM;

namespace Tests;

public class PemTests
{
    [Fact]
    public void Serialization()
    {
        DERSequence der = new([
            new DERGeneric(0, true, DataType.Integer, [0,0,0,1]),
            new DERGeneric(0, true, DataType.Boolean, [1]),
            new DERGeneric(0, true, DataType.Null, []),
        ]);

        PEMBlockDER input = new("TEST", der);

        string encoded = input.Encode();

        string reference = "-----BEGIN TEST-----\nMAsCBAAAAAEBAQEFAA==\n-----END TEST-----\n";

        Assert.Equal(reference, encoded, ignoreLineEndingDifferences: true);
    }


    [Fact]
    public void Deserialization()
    {
        DERSequence reference = new([
            new DERGeneric(0, true, DataType.Integer, [0,0,0,1]),
            new DERGeneric(0, true, DataType.Boolean, [1]),
            new DERGeneric(0, true, DataType.Null, []),
        ]);

        string input = "-----BEGIN TEST-----\nMAsCBAAAAAEBAQEFAA==\n-----END TEST-----\n";
        PEMBlock decoded = PEMBlock.FromEncoded(input, 0, out int endPem);
        Assert.Equal(60, endPem);

        Assert.Equal("TEST", decoded.BlockLabel);

        DERBase der = DERBase.FromEncoded(decoded.Content, 0, out int endDer);
        Assert.Equal(13, endDer);

        DERSequence sequence = Assert.IsType<DERSequence>(der);

        Utils.DerSequencesEqual(reference, sequence);
    }
}
