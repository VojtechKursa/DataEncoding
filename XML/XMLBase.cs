using System;

namespace DataEncoding.XML
{
    /// <summary>
    /// Represents the base class for all objects representing XML data.
    /// </summary>
    public abstract class XMLBase
    {
        #region Proprties

        /// <summary>
        /// The attributes of the XML object (in case of tag)
        /// </summary>
        public XMLAttributeCollection Attributes { get; set; } = new XMLAttributeCollection();

        /// <summary>
        /// The name of the XML object (in case of tag)
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Methods
        #region Abstract methods

        /// <summary>
        /// Encodes the XML object based on it's current values.
        /// </summary>
        /// <returns>A string, representing the encoded XML object.</returns>
        public abstract string Encode();

        /// <summary>
        /// Decodes the given XML encoded text starting at the given index and sets the object's values based on the decoded data.<br/>
        /// Throws an <see cref="ArgumentException"/> if no valid text was found (tag in case of <see cref="XMLElement"/>. <see cref="XMLString"/> decodes anything.)
        /// </summary>
        /// <param name="xml">The XML encoded text to decode.</param>
        /// <param name="start">The index at which to start decoding.</param>
        /// <returns>The index in the xml at which the decoding stopped (the index of the first character after the decoded value).</returns>
        /// <exception cref="ArgumentException"/>
        public abstract int Decode(string xml, int start);

        #endregion

        #region Public static methods

        /// <summary>
        /// Beautifies the given XML string. (Hopefully)
        /// </summary>
        /// <param name="xml">The XML string to beautify.</param>
        /// <returns>The beautified XML string.</returns>
        public static string Beautify(string xml)
        {
            string result = "";
            int depth = 0;
            bool inTag = false, inAttribute = false, openingTag = false;
            char tagStartedBy = '"';

            for (int i = 0; i < xml.Length; i++)
            {
                switch (xml[i])
                {
                    case '<':
                        inTag = true;

                        if (xml[i + 1] != '/')
                        {
                            depth++;
                            openingTag = true;
                        }
                        else
                        {
                            openingTag = false;

                            int tabCount = CountFromEnd(result, '\t');
                            if (tabCount > depth - 1)
                                result = result.Remove(result.Length - (tabCount - (depth - 1)));
                        }

                        if (i - 1 > -1)
                        {
                            if (result[result.Length - 1] != '\t' && result[result.Length - 1] != '\n' && result[result.Length - 1] != '\r')
                                result += Environment.NewLine + MultiplyString("\t", depth - 1);
                        }

                        result += "<";
                        break;
                    case '>':
                        inTag = false;
                        if (xml[i - 1] != '/')
                        {
                            if (!openingTag)
                                depth--;
                        }
                        else
                            depth--;
                        result += ">" + Environment.NewLine + MultiplyString("\t", depth);
                        break;
                    case '\'':
                    case '"':
                        if (inTag)
                        {
                            if (inAttribute)
                            {
                                if (xml[i] == tagStartedBy)
                                    inAttribute = false;
                            }
                            else
                            {
                                tagStartedBy = xml[i];
                                inAttribute = true;
                            }
                        }
                        result += xml[i];
                        break;
                    default:
                        if (inTag)
                        {
                            if (!inAttribute)
                            {
                                if (xml[i - 1] == ' ' && xml[i] == ' ')
                                    continue;
                                else if (xml[i] == '\r' || xml[i] == '\n')
                                    continue;
                                else if (xml[i] == '\t')
                                {
                                    if (xml[i - 1] != ' ')
                                        result += ' ';
                                }
                                else
                                    result += xml[i];
                            }
                            else
                                result += xml[i];
                        }
                        else
                        {
                            if (xml[i] != '\r' && xml[i] != '\n' && xml[i] != '\t')
                                result += xml[i];
                        }
                        break;
                }
            }

            return result.Replace(" />", "/>").Replace("/>", " />");
        }

        /// <summary>
        /// Minifies the given XML string.
        /// </summary>
        /// <param name="xml">The XML string to minify.</param>
        /// <returns>The minified XML string.</returns>
        public static string Minify(string xml)
        {
            return Beautify(xml).Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" />", "/>");
        }

        #endregion

        #region Support methods

        /// <summary>
        /// Decodes the name and attributes of a tag from the given XML text.<br />
        /// Throws an <see cref="ArgumentException"/> if no tag was found.
        /// </summary>
        /// <param name="xml">The XML text to decode.</param>
        /// <param name="start">The index in the xml from which to start decoding.</param>
        /// <param name="end">The index at which the decoding stopped (the index of the first character after the decoded value).</param>
        /// <returns>The content of the decoded tag.</returns>
        protected virtual string CommonDecoder(string xml, int start, out int end)
        {
            xml = RemoveDeclarations(xml);

            int openingTagStart = xml.IndexOf('<', start);

            if (openingTagStart != -1)
            {
                int openingTagEnd = xml.IndexOf('>', openingTagStart + 1);

                if (openingTagEnd != -1)
                {
                    string openingTag = xml.Substring(openingTagStart + 1, openingTagEnd - (openingTagStart + 1));

                    int index = 0;

                    string name = "";
                    while (openingTag[index] != ' ' && openingTag[index] != '/')
                    {
                        name += openingTag[index];
                        index++;

                        if (index == openingTag.Length)
                            break;
                    }
                    Name = XMLString.Decode(name);

                    int attribEnd = index;
                    int attribValueStart, attribStart;
                    while (true)
                    {
                        attribValueStart = openingTag.IndexOf('"', attribEnd);

                        if (attribValueStart != -1)
                        {
                            attribStart = attribEnd + 1;
                            attribEnd = openingTag.IndexOf('"', attribValueStart + 1) + 1;

                            Attributes.Add(XMLAttribute.FromEncoded(openingTag.Substring(attribStart, attribEnd - attribStart)));
                        }
                        else
                            break;
                    }

                    if (openingTag[openingTag.Length - 1] == '/')
                    {
                        end = openingTagEnd + 1;
                        return null;
                    }
                    else
                    {
                        int endOfContent = xml.IndexOf(String.Format("</{0}", name), openingTagEnd + 1);

                        end = xml.IndexOf(">", endOfContent + 2) + 1;

                        return xml.Substring(openingTagEnd + 1, endOfContent - (openingTagEnd + 1));
                    }
                }
                else
                    throw new ArgumentException("No opening tag end (>) found in " + nameof(xml));
            }
            else
                throw new ArgumentException("No opening tag start (<) found in " + nameof(xml) + ".");
        }

        internal static string RemoveComments(string xml)
        {
            string result = xml;

            int commentStart, commentEnd;
            while (result.Contains("<!--"))
            {
                commentStart = result.IndexOf("<!--");
                commentEnd = result.IndexOf("-->") + 2;

                if (commentEnd != -1)
                {
                    result = result.Remove(commentStart, commentEnd - commentStart + 1);
                }
                else
                    throw new ArgumentException("Found start of a comment without an end.");
            }

            return result;
        }

        internal static string RemoveDeclarations(string xml)
        {
            string result = xml;
            int startIndex, endIndex;

            while (true)
            {
                startIndex = result.IndexOf("<?");

                if (startIndex != -1)
                {
                    endIndex = result.IndexOf(">", startIndex + 2);

                    if (endIndex != -1)
                        result = result.Remove(startIndex, endIndex - startIndex + 1);
                    else
                        throw new ArgumentException("Found an unpaired XML header tag.");
                }
                else
                    break;
            }

            return result;
        }

        private static string MultiplyString(string pattern, int count)
        {
            string result = "";

            for (int i = 0; i < count; i++)
            {
                result += pattern;
            }

            return result;
        }

        private static int CountFromEnd(string text, char character)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[text.Length - 1 - i] != character)
                    return i;
            }

            return text.Length;
        }

        #endregion
        #endregion
    }
}
