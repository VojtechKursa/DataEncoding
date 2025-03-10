using DataEncoding.JSON;

namespace Tests;

public class JsonTests
{
    [Fact]
    public void SerializeNull()
    {
        string encoded = new JSONNull().Encode();
        Assert.Equal("null", encoded);
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void SerializeBool(bool toSerialize, string expected)
    {
        string encoded = new JSONBool(toSerialize).Encode();
        Assert.Equal(expected, encoded);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(5, "5")]
    [InlineData(-5, "-5")]
    [InlineData(1358759, "1358759")]
    [InlineData(3.14, "3.14")]
    [InlineData(-3.14, "-3.14")]
    public void SerializeNumber(double toSerialize, string expected)
    {
        string encoded = new JSONNumber(toSerialize).Encode();
        Assert.Equal(expected, encoded);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("text")]
    [InlineData("文字")]
    [InlineData("\n", "\"\\n\"")]
    [InlineData("\t", "\"\\t\"")]
    [InlineData("    ")]
    public void SerializeString(string toSerialize, string? expected = null)
    {
        string encoded = new JSONString(toSerialize).Encode();
        expected ??= $"\"{toSerialize}\"";

        Assert.Equal(expected, encoded);
    }

    [Fact]
    public void SerializeArray()
    {
        JSONArray arrOuter = new();
        arrOuter.Content.Add(new JSONString("aaa"));
        arrOuter.Content.Add(new JSONNumber(5));
        arrOuter.Content.Add(new JSONNull());

        JSONArray arrInner = new();
        arrInner.Content.Add(new JSONNumber(5));

        JSONObject obj = new();
        obj.Content.Add("aa", new JSONBool(false));
        arrInner.Content.Add(obj);

        arrOuter.Content.Add(arrInner);

        string encoded = JSONFunctions.Minify(arrOuter.Encode());

        Assert.Equal(@"[""aaa"",5,null,[5,{""aa"":false}]]", encoded);
    }

    [Fact]
    public void SerializeObject()
    {
        JSONObject obj = new();
        obj.Content.Add("a", new JSONString("aaa"));
        obj.Content.Add("b", new JSONNumber(5));
        obj.Content.Add("c", new JSONNull());

        JSONArray arr = new();
        arr.Content.Add(new JSONNumber(5));

        JSONObject objInner = new();
        objInner.Content.Add("aa", new JSONBool(false));
        arr.Content.Add(objInner);

        obj.Content.Add("x", arr);

        string encoded = JSONFunctions.Minify(obj.Encode());

        Assert.Equal(@"{""a"":""aaa"",""b"":5,""c"":null,""x"":[5,{""aa"":false}]}", encoded);
    }

    [Fact]
    public void DeserializeNull()
    {
        string encoded = "  null  ";

        var decoded = JSONFunctions.DecodeString(encoded, 0);
        
        Assert.IsType<JSONNull>(Assert.Single(decoded));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void DeserializeBool(string toDeserialize, bool? expected)
    {
        var decoded = JSONFunctions.DecodeString(toDeserialize, 0);

        var jsonBase = Assert.Single(decoded);

        var jsonBool = Assert.IsType<JSONBool>(jsonBase);

        Assert.Equal(expected, jsonBool.Content);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("5", 5)]
    [InlineData("-5", -5)]
    [InlineData("1358759", 1358759)]
    [InlineData("3.14", 3.14, 0.0001)]
    [InlineData("-3.14", -3.14, 0.0001)]
    [InlineData("5e3", 5000)]
    [InlineData("5e-1", 0.5, 0.001)]
    public void DeserializeNumber(string toDeserialize, double expected, double tolerance = 0)
    {
        var decoded = JSONFunctions.DecodeString(toDeserialize, 0);

        var jsonBase = Assert.Single(decoded);

        var number = Assert.IsType<JSONNumber>(jsonBase);

        Assert.Equal(expected, number.Content, tolerance);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("text")]
    [InlineData("文字")]
    [InlineData("\\n", "\n")]
    [InlineData("\\t", "\t")]
    [InlineData("    ")]
    public void DeserializeString(string toDeserialize, string? expected = null)
    {
        expected ??= toDeserialize;
        toDeserialize = $"\"{toDeserialize}\"";

        var decoded = JSONFunctions.DecodeString(toDeserialize, 0);

        var jsonBase = Assert.Single(decoded);

        var str = Assert.IsType<JSONString>(jsonBase);

        Assert.Equal(expected, str.Content);
    }

    [Fact]
    public void DeserializeArray()
    {
        string encoded = @" [""aaa""   , 5,  null,[8,{""a"":false}]] ";

        var decoded = JSONFunctions.DecodeString(encoded, 0);

        var jsonBase = Assert.Single(decoded);

        var arr = Assert.IsType<JSONArray>(jsonBase);

        Assert.Equal(4, arr.Content.Count);

        var first = Assert.IsType<JSONString>(arr.Content[0]);
        Assert.Equal("aaa", first.Content);

        var second = Assert.IsType<JSONNumber>(arr.Content[1]);
        Assert.Equal(5, second.Content);

        Assert.IsType<JSONNull>(arr.Content[2]);



        var fourth = Assert.IsType<JSONArray>(arr.Content[3]);

        Assert.Equal(2, fourth.Content.Count);

        var innerFirst = Assert.IsType<JSONNumber>(fourth.Content[0]);
        Assert.Equal(8, innerFirst.Content);

        var innerSecond = Assert.IsType<JSONObject>(fourth.Content[1]);
        Assert.Equal(1, innerSecond.Content.Count);

        var innerSecondA = Assert.IsType<JSONBool>(innerSecond.Content.FindValue("a"));
        Assert.False(innerSecondA.Content);
    }

    [Fact]
    public void DeserializeObject()
    {
        string encoded = @"{""a"":""aaa"",""b"":5.8,""c"":null, ""x"":[8,{""a"":true}]}";

        var decoded = JSONFunctions.DecodeString(encoded, 0);

        var jsonBase = Assert.Single(decoded);

        var obj = Assert.IsType<JSONObject>(jsonBase);

        Assert.Equal(4, obj.Content.Count);

        var a = Assert.IsType<JSONString>(obj.Content.FindValue("a"));
        Assert.Equal("aaa", a.Content);

        var b = Assert.IsType<JSONNumber>(obj.Content.FindValue("b"));
        Assert.Equal(5.8, b.Content, 0.001);

        Assert.IsType<JSONNull>(obj.Content.FindValue("c"));



        var x = Assert.IsType<JSONArray>(obj.Content.FindValue("x"));
        Assert.Equal(2, x.Content.Count);

        var xFirst = Assert.IsType<JSONNumber>(x.Content[0]);
        Assert.Equal(8, xFirst.Content);

        var xSecond = Assert.IsType<JSONObject>(x.Content[1]);
        Assert.Equal(1, xSecond.Content.Count);

        var xSecondA = Assert.IsType<JSONBool>(xSecond.Content.FindValue("a"));
        Assert.True(xSecondA.Content);
    }
}
