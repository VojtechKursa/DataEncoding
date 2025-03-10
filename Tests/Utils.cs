using DataEncoding.DER;

namespace Tests;

public class Utils
{
    public static void DerSequencesEqual(DERSequence expected, DERSequence actual)
    {
        Assert.Equal(expected.Content.Count, actual.Content.Count);

        for (int i = 0; i < expected.Content.Count; i++)
        {
            DERGeneric value = Assert.IsType<DERGeneric>(actual.Content[i]);
            DERGeneric referenceValue = expected.Content[i] as DERGeneric ?? throw new Exception();

            Assert.Equal(referenceValue.Type.TypeBytes, value.Type.TypeBytes);
            Assert.Equal(referenceValue.Length, value.Length);
            Assert.Equal(referenceValue.Content, value.Content);
        }
    }
}
