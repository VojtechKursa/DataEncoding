﻿using System;
using DataEncoding.Interfaces;

namespace DataEncoding.PEM
{
    /// <summary>
    /// Represent the base for all PEM objects in the DataEncoding library.
    /// </summary>
    public abstract class PEMBase : ISupportsEncode<string>, ISupportsDecode<string>
    {
        /// <summary>
        /// Encodes the data into the PEM format.
        /// </summary>
        /// <returns>The encoded string.</returns>
        public abstract string Encode();

        /// <summary>
        /// Decodes the given PEM data and sets object's properties based on the decoded data.
        /// </summary>
        /// <param name="data">The PEM data to decode.</param>
        /// <returns>The index at which the decoding process stopped (the index of the next character after the decoded value).</returns>
        /// <exception cref="ArgumentException"/>
        public int Decode(string data) => Decode(data, 0);

        /// <inheritdoc cref="Decode(string)"/>
        /// <param name="startIndex">The index from which to start decoding.</param>
        public abstract int Decode(string data, int startIndex);
    }
}
