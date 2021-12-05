using System;

namespace DataEncoding.PEM
{
    /// <summary>
    /// Represent the base for all PEM objects in the DataEncoding library.
    /// </summary>
    public abstract class PEMBase
    {
        /// <summary>
        /// Gets or sets the encoded string.
        /// </summary>
        public string Encoded { get; protected set; }

        /// <summary>
        /// Encodes the data into the PEM format and sets it to the <see cref="Encoded"/> variable.
        /// </summary>
        /// <returns>The encoded string.</returns>
        public abstract string Encode();

        /// <summary>
        /// Decodes the given PEM data and sets object's properties based on the decoded data.
        /// </summary>
        /// <param name="data">The PEM data to decode.</param>
        /// <param name="startIndex">The index from which to start decoding.</param>
        /// <returns>The index at which the decoding process stopped.</returns>
        /// <exception cref="ArgumentException"/>
        public abstract int Decode(string data, int startIndex);
    }
}
