using System;

namespace DataEncoding.PEM
{
    /// <summary>
    /// Represents a PEM block with any data inside.
    /// </summary>
    public class PEMBlock : PEMBase
    {
        public PEMBlock()
        { }

        public PEMBlock(string label, byte[] content)
        {
            BlockLabel = label;
            Content = content;
        }

        /// <summary>
        /// Gets or sets the label of the <see cref="PEMBlock"/>.
        /// </summary>
        public string BlockLabel { get; set; }

        /// <summary>
        /// Gets or sets the content of the <see cref="PEMBlock"/>.
        /// </summary>
        public byte[] Content { get; set; }

        public override string Encode()
        {
            return String.Format("-----BEGIN {1}-----{0}{2}{0}-----END {1}-----{0}", Environment.NewLine, BlockLabel.ToUpper(), NormalizeLineLength(Convert.ToBase64String(Content)));
        }

        public override int Decode(string data, int startIndex)
        {
            int index = data.IndexOf("-----BEGIN ", startIndex);
            byte attempt = 1;

            if (index == -1)
            {
                index = data.IndexOf("----- BEGIN ", startIndex);
                attempt = 2;

                if (index == -1)
                    throw new ArgumentException("Beginning of a block not found");
            }

            int labelStart = attempt == 1 ? index + 11 : index + 12;
            int labelEnd = data.IndexOf("-----", labelStart);

            BlockLabel = data.Substring(labelStart, labelEnd - labelStart).Trim(' ');

            int dataStart = labelEnd + 5;
            int dataEnd = data.IndexOf("-----END", dataStart);
            if (dataEnd == -1)
            {
                dataEnd = data.IndexOf("----- END", dataStart);
                if (dataEnd == -1)
                    throw new ArgumentException("End of a block not found");
            }

            string dataBlock = data.Substring(dataStart, dataEnd - dataStart);
            dataBlock = dataBlock.Replace("\n", "").Replace("\r", "");

            try
            {
                Content = Convert.FromBase64String(dataBlock);
            }
            catch (Exception)
            {
                throw new ArgumentException("The extracted data block could not be decoded from Base64.");
            }

            return data.IndexOf("-----", dataEnd + 5) + 5;
        }

        /// <summary>
        /// Aligns the input data into 64 character long lines.
        /// </summary>
        /// <param name="input">The input to align.</param>
        /// <returns>The aligned string.</returns>
        protected static string NormalizeLineLength(string input)
        {
            string result = input;

            int start = result.Length - (result.Length % 64);
            if (result.Length % 64 == 0)
                start -= 64;

            for (int i = start; i > 0; i -= 64)
            {
                result = result.Insert(i, Environment.NewLine);
            }

            return result;
        }

        /// <summary>
        /// Initiates a new instance of <see cref="PEMBlock"/> based on the decoded input data.
        /// </summary>
        /// <param name="data">The PEM data to decode.</param>
        /// <param name="startIndex">The index from which to start decoding.</param>
        /// <param name="end">The index at which the decoding process stopped (the index of the next character after the decoded value).</param>
        /// <returns>The new instance of <see cref="PEMBlock"/> containing the decoded data.</returns>
        /// <exception cref="ArgumentException"/>
        public static PEMBlock FromEncoded(string data, int startIndex, out int end)
        {
            PEMBlock result = new PEMBlock();

            end = result.Decode(data, startIndex);
            return result;
        }
    }
}
