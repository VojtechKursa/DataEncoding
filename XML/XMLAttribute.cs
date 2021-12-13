using System;

namespace DataEncoding.XML
{
    /// <summary>
    /// Represents an attribute of an XML tag.
    /// </summary>
    public class XMLAttribute
    {
        #region Properties

        /// <summary>
        /// The name of the attribute.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The value of the attribute
        /// </summary>
        public string Value { get; set; } = null;

        #endregion

        #region Constructors

        public XMLAttribute()
        { }

        public XMLAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Encodes the <see cref="XMLAttribute"/> based on it's current <see cref="Name"/> and <see cref="Value"/>.
        /// </summary>
        /// <returns>The encoded <see cref="XMLAttribute"/></returns>
        public string Encode()
        {
            return String.Format("{0}=\"{1}\"", XMLString.Encode(Name), XMLString.Encode(Value));
        }

        /// <summary>
        /// Decodes the given text and sets the <see cref="Name"/> and <see cref="Value"/> of the <see cref="XMLAttribute"/> object accordingly.
        /// </summary>
        /// <param name="attributeText">The text to decode.</param>
        public void Decode(string attributeText)
        {
            int equalsIndex = attributeText.IndexOf('=');

            if (equalsIndex != -1)
            {
                Name = XMLString.Decode(attributeText.Substring(0, equalsIndex).Trim());

                int valueStart = attributeText.IndexOf('\"', equalsIndex + 1);
                if (valueStart != -1)
                {
                    int valueEnd = attributeText.IndexOf('\"', valueStart + 1);

                    if (valueEnd != -1)
                    {
                        Value = XMLString.Decode(attributeText.Substring(valueStart + 1, valueEnd - (valueStart + 1)));
                    }
                    else
                        throw new ArgumentException("No end of a value found in " + nameof(attributeText) + ".");
                }
                else
                    throw new ArgumentException("No start of a value found in " + nameof(attributeText) + ".");
            }
            else
                throw new ArgumentException("No '=' found in " + nameof(attributeText) + ".");
        }

        /// <summary>
        /// Creates a new instance of <see cref="XMLAttribute"/> based on information decoded from the given text.
        /// </summary>
        /// <param name="attributeText">The text to decode.</param>
        /// <returns>The new instance of <see cref="XMLAttribute"/>.</returns>
        public static XMLAttribute FromEncoded(string attributeText)
        {
            XMLAttribute result = new XMLAttribute();
            result.Decode(attributeText);

            return result;
        }

        #endregion
    }
}
