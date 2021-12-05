using System;
using System.Collections.Generic;

namespace DataEncoding.PEM
{
    public class PEMSuperBlockDER : PEMSuperBlock
    {
        public PEMSuperBlockDER()
        {
            base.Blocks = null;
        }

        public PEMSuperBlockDER(List<PEMBlockDER> blocks) : this()
        {
            Blocks = blocks;
        }

        /// <summary>
        /// Gets or sets the list of blocks in the current <see cref="PEMSuperBlockDER"/>.
        /// </summary>
        new public List<PEMBlockDER> Blocks { get; set; } = new List<PEMBlockDER>();

        public override string Encode()
        {
            string result = "";

            foreach (PEMBlockDER block in Blocks)
            {
                result += block.Encode() + Environment.NewLine;
            }

            return result.Remove(result.Length - Environment.NewLine.Length, Environment.NewLine.Length);
        }

        public override int Decode(string data, int startIndex)
        {
            int blockStart;
            int blockEnd = startIndex;
            PEMBlockDER currentBlock;

            while (true)
            {
                blockStart = data.IndexOf("-----BEGIN", blockEnd);
                if (blockStart == -1)
                {
                    blockStart = data.IndexOf("----- BEGIN", blockEnd);
                    if (blockStart == -1)
                        break;
                }

                currentBlock = new PEMBlockDER();
                blockEnd = currentBlock.Decode(data, blockStart);

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
        new public int IndexOf(string label)
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
        /// Returns a <see cref="PEMBlockDER"/> with the <see cref="PEMBlock.BlockLabel"/> matching the given label from the <see cref="Blocks"/> list.
        /// </summary>
        /// <param name="label">The <see cref="PEMBlock.BlockLabel"/> to search for.</param>
        /// <returns>
        /// A <see cref="PEMBlockDER"/> with the <see cref="PEMBlock.BlockLabel"/> matching the given label.<br />
        /// null of no matching <see cref="PEMBlockDER"/> was found.
        /// </returns>
        new public PEMBlockDER FindBlock(string label)
        {
            int index = IndexOf(label);

            return index != -1 ? Blocks[index] : null;
        }
    }
}
