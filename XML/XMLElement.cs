using System;
using System.Collections.Generic;

namespace DataEncoding.XML
{
    /// <summary>
    /// Represents one XML element (surrounded by tags).
    /// </summary>
    public class XMLElement : XMLBase
    {
        /// <summary>
        /// Represents all contents of the <see cref="XMLElement"/>.
        /// </summary>
        public List<XMLBase> Content { get; set; } = new List<XMLBase>();

        public XMLElement()
        { }

        public XMLElement(string name)
        {
            Name = name;
        }

        public XMLElement(string name, XMLBase content) : this(name)
        {
            Content.Add(content);
        }

        public XMLElement(string name, IEnumerable<XMLBase> content) : this(name)
        {
            Content.AddRange(content);
        }

        public override string Encode()
        {
            string result = "<" + XMLString.Encode(Name);

            foreach (XMLAttribute attribute in Attributes)
            {
                result += " " + attribute.Encode();
            }

            if (Content.Count > 0)
            {
                result += ">";

                foreach (XMLBase elem in Content)
                {
                    result += elem.Encode();
                }

                result += String.Format("</{0}>", XMLString.Encode(Name));
            }
            else
                result += "/>";

            return result;
        }

        public override int Decode(string xml, int start)
        {
            string contentStr = CommonDecoder(xml, start, out int end);

            if (contentStr != null)
            {
                contentStr = Minify(XMLString.RemoveComments(contentStr));

                int lastEnd = 0, elemStart;
                while (true)
                {
                    elemStart = contentStr.IndexOf('<', lastEnd);

                    if (elemStart != -1)
                    {
                        if (elemStart == lastEnd)
                        {
                            Content.Add(XMLElement.FromEncoded(contentStr, elemStart, out lastEnd));
                        }
                        else
                        {
                            Content.Add(XMLString.FromEncoded(contentStr.Substring(lastEnd, elemStart - lastEnd)));

                            lastEnd = elemStart;
                        }
                    }
                    else
                    {
                        if (lastEnd != contentStr.Length - 1)
                        {
                            Content.Add(XMLString.FromEncoded(contentStr.Substring(lastEnd, contentStr.Length - lastEnd)));
                        }

                        break;
                    }
                }
            }

            return end;
        }

        /// <summary>
        /// Creates a new instance of <see cref="XMLElement"/> and sets it's values based on the decoded data.
        /// </summary>
        /// <param name="xml">The data to decode.</param>
        /// <param name="start">The index at which to start decoding.</param>
        /// <param name="end">The index at which the decoding stopped (the index of the first character after the decoded value).</param>
        /// <returns>The new instance of <see cref="XMLElement"/> based on values from the decoded data.</returns>
        public static XMLElement FromEncoded(string xml, int start, out int end)
        {
            XMLElement elem = new XMLElement();

            end = elem.Decode(xml, start);
            return elem;
        }
    }
}
