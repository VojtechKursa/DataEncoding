using System;

namespace DataEncoding.XML
{
    /// <summary>
    /// Represents a string of characters in XML.<br />
    /// Mainly useful for escaping and unescaping of characters.
    /// </summary>
    public class XMLString : XMLBase
    {
        /// <summary>
        /// The string which the <see cref="XMLString"/> represents.
        /// </summary>
        public string Content { get; set; }

        public XMLString()
        { }

        public XMLString(string content)
        {
            Content = content;
        }

        #region Methods
        #region Implemented abstract methods

        public override string Encode()
        {
            return Escape(Content);
        }

        public override int Decode(string xml, int start)
        {
            Content = Unescape(RemoveDeclarations(RemoveComments(xml)));

            return xml.Length;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Encodes the given text to allow it's use in XML document.
        /// </summary>
        /// <param name="input">The text to encode.</param>
        /// <returns>The encoded text.</returns>
        public static string Encode(string input)
        {
            return Escape(input);
        }

        /// <summary>
        /// Decodes the given text, performing operations like removing comments and resolving escape sequences back to their original characters.
        /// </summary>
        /// <param name="input">The text to decode.</param>
        /// <returns>The decoded text</returns>
        public static string Decode(string input)
        {
            return Unescape(RemoveDeclarations(RemoveComments(input)));
        }

        /// <summary>
        /// Creates a new instance of <see cref="XMLString"/> based on the text decoded from the given XML string.
        /// </summary>
        /// <param name="xml">The string to decode.</param>
        /// <returns>A new instance of <see cref="XMLString"/> containing the decoded text from the xml argument.</returns>
        public static XMLString FromEncoded(string xml)
        {
            XMLString str = new XMLString(Decode(xml));

            return str;
        }

        #endregion

        #region Support methods

        /// <summary>
        /// Escapes all characters that need to be escaped in the given input.
        /// </summary>
        /// <param name="input">The string to escape.</param>
        /// <returns>A string in which all characters that need to be escaped are properly escaped.</returns>
        protected static string Escape(string input)
        {
            string result = "";

            foreach (char character in input)
            {
                switch (character)
                {
                    case '<': result += "&lt;"; break;
                    case '>': result += "&gt;"; break;
                    case '\'': result += "&apos;"; break;
                    case '\"': result += "&quot;"; break;
                    case '&': result += "&amp;"; break;
                    default:
                        if (character < 0x20)
                            result += EscapeUnicode(character);
                        else
                            result += character;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Resolves all escaped characters in the given text back to their original characters.
        /// </summary>
        /// <param name="input">The text to unescape.</param>
        /// <returns>A text in which all escape sequences are resolved to their original characters.</returns>
        protected static string Unescape(string input)
        {
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '&')
                {
                    int endOfSequence = input.IndexOf(';', i + 1);
                    if (endOfSequence != -1)
                    {
                        string escapeSequence = input.Substring(i + 1, endOfSequence - (i + 1));

                        if (escapeSequence[0] == '#')
                        {
                            result += UnescapeUnicode("&" + escapeSequence + ";", 0, out int length);
                            i += length - 1;
                        }
                        else
                        {
                            switch (escapeSequence)
                            {
                                case "lt": result += "<"; i += 3; break;
                                case "gt": result += ">"; i += 3; break;
                                case "amp": result += "&"; i += 4; break;
                                case "apos": result += "'"; i += 5; break;
                                case "quot": result += "\""; i += 5; break;
                                default:
                                    throw new ArgumentException("Unknown escape sequence \"" + escapeSequence + "\" in " + nameof(input) + " at index " + nameof(i) + ".");
                            }
                        }
                    }
                    else
                        throw new ArgumentException("Escape sequence detected in " + nameof(input) + ", but no ';' character was found.");
                }
                else
                    result += input[i];
            }

            return result;
        }

        /// <summary>
        /// Creates a Unicode escape sequence for the given character.
        /// </summary>
        /// <param name="character">The character to escape.</param>
        /// <returns>XML hexadecimal escape sequence of the given character.</returns>
        protected static string EscapeUnicode(char character)
        {
            return "&#x" + Convert.ToString((int)character, 16);
        }

        /// <summary>
        /// Resolves an Unicode escape sequence back to it's original character.
        /// </summary>
        /// <param name="input">The text that contains the Unicode escape sequence.</param>
        /// <param name="start">The index at which the escape sequence starts.</param>
        /// <param name="length">The length, in characters, of the escape sequence, that was resolved.</param>
        /// <returns>The original character resolved from the Unicode escape sequence.</returns>
        protected static char UnescapeUnicode(string input, int start, out int length)
        {
            if (start + 2 < input.Length)
            {
                if (input[start] == '&' && input[start + 1] == '#')
                {
                    int endOfSequence = input.IndexOf(';', start + 2);

                    if (endOfSequence != -1)
                    {
                        if (input[start + 2] == 'x')
                        {
                            string hex = input.Substring(start + 3, endOfSequence - (start + 3));

                            length = hex.Length + 4;
                            return (char)Convert.ToInt32(hex, 16);
                        }
                        else
                        {
                            string num = input.Substring(start + 2, endOfSequence - (start + 2));

                            length = num.Length + 3;
                            return (char)Convert.ToInt32(num, 10);
                        }
                    }
                    else
                        throw new ArgumentException(nameof(input) + " doesn't contain escape-ending character ';'");
                }
                else
                    throw new ArgumentException(nameof(input) + " doesn't have a Unicode escape sequence at index " + nameof(start) + ".");
            }
            else
                throw new ArgumentOutOfRangeException("Not enough characters in " + nameof(input) + " to be a Unicode escape sequence");
        }

        #endregion
        #endregion
    }
}
