using DataEncoding.DER;

namespace DataEncoding.PEM
{
    /// <summary>
    /// Represents a PEM block with DER data inside.
    /// </summary>
    public class PEMBlockDER : PEMBlock
    {
        public PEMBlockDER()
        { }

        public PEMBlockDER(PEMBlock block) : base(block.BlockLabel, block.Content)
        {
            DERSequence sequence = new DERSequence();
            sequence.Decode(Content, 0);

            ContentDER = sequence;
        }

        public PEMBlockDER(string label, DERSequence der)
        {
            BlockLabel = label;
            ContentDER = der;
        }

        /// <summary>
        /// Gets or sets the DER content of the <see cref="PEMBlockDER"/>.
        /// </summary>
        public DERSequence ContentDER
        {
            get
            {
                if (contentDER == null)
                {
                    contentDER = new DERSequence();
                    contentDER.Decode(Content, 0);
                }

                return contentDER;
            }
            set => contentDER = value;
        }
        private DERSequence contentDER;

        public override string Encode()
        {
            Content = ContentDER.Encode();

            return base.Encode();
        }

        public override int Decode(string data, int startIndex)
        {
            int endIndex = base.Decode(data, startIndex);

            DERSequence der = new DERSequence();
            der.Decode(Content, 0);

            ContentDER = der;

            return endIndex;
        }
    }
}
