using System.Xml.Linq;
using DataEncoding.XML;

namespace Tests
{
    public class XmlTests
    {
        [Fact]
        public void SerializeSimple()
        {
            XMLElement element = new("top");
            element.Content.Add(new XMLString("555"));
            element.Attributes.Add(new XMLAttribute("one", "1"));
            element.Attributes.Add(new XMLAttribute("two", "2"));

            string encoded = element.Encode();

            Assert.Equal(@"<top one=""1"" two=""2"">555</top>", encoded);
        }

        [Fact]
        public void SerializeTree()
        {
            XMLElement top = new("top");
            top.Attributes.Add(new XMLAttribute("one", "1"));
            top.Attributes.Add(new XMLAttribute("two", "2"));

            XMLElement first = new("first");

            XMLElement firstfirst = new("first");
            firstfirst.Attributes.Add(new XMLAttribute("aa", "bb"));
            firstfirst.Content.Add(new XMLString("aaa"));

            first.Content.Add(firstfirst);

            XMLElement second = new("second");

            top.Content.Add(first);
            top.Content.Add(second);

            string encoded = top.Encode();

            Assert.Equal(@"<top one=""1"" two=""2""><first><first aa=""bb"">aaa</first></first><second/></top>", encoded);
        }

        [Fact]
        public void DeserializeSimple()
        {
            string encoded = @"<top one=""1"" two=  ""2"" > 555 </top>  ";

            XMLElement element = new();
            element.Decode(encoded);

            Assert.Equal("top", element.Name);

            Assert.Equal(2, element.Attributes.Count);
            Assert.Equal("1", element.Attributes.FindValue("one"));
            Assert.Equal("2", element.Attributes.FindValue("two"));

            Assert.Equal(1, element.Content.Count);

            XMLString str = element.Content.GetXMLString();
            Assert.NotNull(str);

            Assert.Equal(" 555 ", str.Content);
        }

        [Fact]
        public void DeserializeTree()
        {
            string encoded = @"<top one=""1"" two=""2""><first><second aa=""bb"">aaa</second></first><second/></top>";

            XMLElement top = new();
            top.Decode(encoded);

            Assert.Equal("top", top.Name);

            Assert.Equal(2, top.Attributes.Count);
            Assert.Equal("1", top.Attributes.FindValue("one"));
            Assert.Equal("2", top.Attributes.FindValue("two"));

            Assert.Equal(2, top.Content.Count);

            var first = top.Content.Find("first");
            Assert.NotNull(first);

            Assert.Equal(0, first.Attributes.Count);
            Assert.Equal(1, first.Content.Count);

            var firstSecond = first.Content.Find("second");
            Assert.NotNull(firstSecond);

            Assert.Equal(1, firstSecond.Attributes.Count);
            Assert.Equal("bb", firstSecond.Attributes.FindValue("aa"));

            var str = firstSecond.Content.GetXMLString();
            Assert.NotNull(str);
            Assert.Equal("aaa", str.Content);

            var second = top.Content.Find("second");
            Assert.NotNull(second);

            Assert.Equal(0, second.Content.Count);
            Assert.Equal(0, second.Attributes.Count);
        }
    }
}
