#region License
/* 
 * Copyright (C) 1999-2025 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Reko.Core
{
    /// <summary>
    /// Platform-specific heuristics.
    /// </summary>
    public class PlatformHeuristics
    {
        /// <summary>
        /// Gets <see cref="MaskedPattern"/>s for bit patterns that might be
        /// procedure prologs.
        /// </summary>
        public MaskedPattern[]? ProcedurePrologs;
    }

    /// <summary>
    /// Represents a pattern of bytes and a mask that can be used to match patterns.
    /// </summary>
    public class MaskedPattern
    {
        /// <summary>
        /// The pattern of bytes to match.
        /// </summary>
        public byte[]? Bytes { get; init; }

        /// <summary>
        /// The mask to apply to the pattern. A byte of 0x00 means "don't care".
        /// </summary>
        public byte[]? Mask { get; init; }

        /// <summary>
        /// Endianness to apply to the bytes. This is used when matching the pattern.
        /// </summary>
        public EndianServices Endianness = EndianServices.Big;

        /// <summary>
        /// Load a <see cref="MaskedPattern"/> from a string of hexadecimal digits.
        /// </summary>
        /// <param name="sBytes">Byte pattern expressed as hexadecimal digits.</param>
        /// <param name="sMask">Mask pattern expressed as hexadecimal digits.</param>
        /// <param name="sEndianness">Endianness to apply to the bytes.</param>
        /// <returns>A <see cref="MaskedPattern"/> instance; or null if the data is 
        /// incorrect.
        /// </returns>
        public static MaskedPattern? Load(string? sBytes, string? sMask, string? sEndianness = null)
        {
            if (sBytes is null)
                return null;
            var endianness = (string.IsNullOrEmpty(sEndianness) || char.ToLowerInvariant(sEndianness[0]) == 'b')
                ? EndianServices.Big
                : EndianServices.Little;
            if (sMask is null)
            {
                var bytes = new List<byte>();
                var mask = new List<byte>();
                int shift = 4;
                int bb = 0;
                int mm = 0;
                for (int i = 0; i < sBytes.Length; ++i)
                {
                    char c = sBytes[i];
                    if (BytePattern.TryParseHexDigit(c, out byte b))
                    {
                        bb |= (b << shift);
                        mm |= (0x0F << shift);
                        shift -= 4;
                        if (shift < 0)
                        {
                            bytes.Add((byte) bb);
                            mask.Add((byte) mm);
                            shift = 4;
                            bb = mm = 0;
                        }
                    }
                    else if (c == '?' || c == '.')
                    {
                        shift -= 4;
                        if (shift < 0)
                        {
                            bytes.Add((byte) bb);
                            mask.Add((byte) mm);
                            shift = 4;
                            bb = mm = 0;
                        }
                    }
                }
                Debug.Assert(bytes.Count == mask.Count);
                if (bytes.Count == 0)
                    return null;
                return new MaskedPattern
                {
                    Bytes = bytes.ToArray(),
                    Mask = mask.ToArray(),
                    Endianness = endianness,
                };
            }
            else
            {
                var bytes = BytePattern.FromHexBytes(sBytes);
                var mask = BytePattern.FromHexBytes(sMask);
                if (bytes.Length == 0)
                    return null;
                return new MaskedPattern
                {
                    Bytes = bytes.ToArray(),
                    Mask = mask.ToArray(),
                    Endianness = endianness,
                };
            }
        }
    }
}
