using System;
using System.Collections.Generic;

namespace DataEncoding.PEM
{
    /// <summary>
    /// Represents a series of <see cref="PEMBlock"/>.<br />
    /// Provides an easy way of handling encoding and decoding of multiple blocks at once.
    /// </summary>
    public class PEMSuperBlock : PEMBase
    {
        public PEMSuperBlock()
        { }

        public PEMSuperBlock(List<PEMBlock> blocks)
        {
            Blocks = blocks;
        }

        /// <summary>
        /// Gets or sets the list of blocks in the current <see cref="PEMSuperBlock"/>.
        /// </summary>
        public List<PEMBlock> Blocks { get; set; } = new List<PEMBlock>();

        public override string Encode()
        {
            string result = "";

            foreach (PEMBlock block in Blocks)
            {
                result += block.Encode() + Environment.NewLine;
            }

            return result.Remove(result.Length - Environment.NewLine.Length, Environment.NewLine.Length);
        }

        public override int Decode(string data, int startIndex)
        {
            int blockStart;
            int blockEnd = startIndex;
            PEMBlock currentBlock;

            while (true)
            {
                blockStart = data.IndexOf("-----BEGIN", blockEnd);
                if (blockStart == -1)
                {
                    blockStart = data.IndexOf("----- BEGIN", blockEnd);
                    if (blockStart == -1)
                        break;
                }

                try
                {
                    currentBlock = new PEMBlockDER();
                    blockEnd = currentBlock.Decode(data, blockStart);
                }
                catch (Exception)
                {
                    currentBlock = new PEMBlock();
                    blockEnd = currentBlock.Decode(data, blockStart);
                }

                Blocks.Add(currentBlock);
            }

            return blockEnd;
        }

        /// <summary>
        /// Returns the index of a <see cref="PEMBlock"/> with the specified <see cref="PEMBlock.BlockLabel"/> in the <see cref="Blocks"/> list.
        /// </summary>
        /// <param name="label">The <see cref="PEMBlock.BlockLabel"/> to search for.</param>
        /// <returns>
        /// The index of a <see cref="PEMBlock"/> with the specified <see cref="PEMBlock.BlockLabel"/> in the <see cref="Blocks"/> list.<br />
        /// -1 if no <see cref="PEMBlock"/> with a matching <see cref="PEMBlock.BlockLabel"/> was found.
        /// </returns>
        public int IndexOf(string label)
        {
            string labelUpper = label.ToUpper();

            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].BlockLabel == labelUpper)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Returns a <see cref="PEMBlock"/> with the <see cref="PEMBlock.BlockLabel"/> matching the given label from the <see cref="Blocks"/> list.
        /// </summary>
        /// <param name="label">The <see cref="PEMBlock.BlockLabel"/> to search for.</param>
        /// <returns>
        /// A <see cref="PEMBlock"/> with the <see cref="PEMBlock.BlockLabel"/> matching the given label.<br />
        /// null of no matching <see cref="PEMBlock"/> was found.
        /// </returns>
        public PEMBlock FindBlock(string label)
        {
            int index = IndexOf(label);

            return index != -1 ? Blocks[index] : null;
        }

        /// <summary>
        /// Initiates a new instance of <see cref="PEMSuperBlock"/> based on the decoded input data.
        /// </summary>
        /// <param name="data">The PEM data to decode.</param>
        /// <param name="startIndex">The index from which to start decoding.</param>
        /// <param name="end">The index at which the decoding process stopped (the index of the next character after the decoded value).</param>
        /// <returns>The new instance of <see cref="PEMSuperBlock"/> containing the decoded data.</returns>
        /// <exception cref="ArgumentException"/>
        public static PEMSuperBlock FromEncoded(string data, int startIndex, out int end)
        {
            PEMSuperBlock result = new PEMSuperBlock();

            end = result.Decode(data, startIndex);
            return result;
        }
    }
}
