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

using Reko.Core.NativeInterface;

namespace Reko.Core.Expressions
{
    /// <summary>
    /// Represents a low-level condition.
    /// </summary>
    [NativeInterop]
    public enum ConditionCode
    {
        None,
        UGT,	// Unsigned >
        ULE,	// Unsigned <=
        ULT,	// Unsigned <
        GT,		// >
        GE,		// >=
        LT,		// <
        LE,		// <=
        UGE,	// Unsigned >=
        NO,		// No overflow
        NS,		// >= 0
        NE,		// != 
        OV,		// Overflow
        SG,		// < 0
        EQ,		// ==	
        PE,     // Parity even
        PO,     // parity odd

        ALWAYS, // Some architectures have this.
        NEVER,

        IS_NAN,     // comparison discovered an floating point NaN
        NOT_NAN,    // comparison didn't discover a NaN
    }

    public static class ConditionCodeEx
    {
        private static readonly ConditionCode[] invertMap = new[]
        {
            ConditionCode.None,
            ConditionCode.ULE,
            ConditionCode.UGT,
            ConditionCode.UGE,
            ConditionCode.LE,
            ConditionCode.LT,
            ConditionCode.GE,
            ConditionCode.GT,
            ConditionCode.ULT,
            ConditionCode.OV,
            ConditionCode.SG,
            ConditionCode.EQ,
            ConditionCode.NO,
            ConditionCode.NS,
            ConditionCode.NE,
            ConditionCode.PO,
            ConditionCode.PE,
            ConditionCode.NEVER,
            ConditionCode.ALWAYS,
            ConditionCode.NOT_NAN,
            ConditionCode.IS_NAN
        };


        /// <summary>
        /// Returns the condition code that results upon logical negation.
        /// </summary>
        /// <remarks>
        /// E.g. inverting UGE results in ULT.</remarks>
        /// <param name="cc">Condition code to invert.</param>
        /// <returns>The negated condition code.</returns>
        public static ConditionCode Invert(this ConditionCode cc)
        {
            return invertMap[(int)cc];
        }

        private static readonly ConditionCode[] mirrorMap = new[]
        {
            ConditionCode.None,
            ConditionCode.ULT,
            ConditionCode.UGE,
            ConditionCode.UGT,
            ConditionCode.LT,
            ConditionCode.LE,
            ConditionCode.GT,
            ConditionCode.GE,
            ConditionCode.ULE,
            ConditionCode.NO,
            ConditionCode.NS,
            ConditionCode.NE,
            ConditionCode.OV,
            ConditionCode.SG,
            ConditionCode.EQ, 
            ConditionCode.PE, 
            ConditionCode.PO, 

            ConditionCode.ALWAYS,
            ConditionCode.NEVER,

            ConditionCode.IS_NAN,
            ConditionCode.NOT_NAN,
        };

        /// <summary>
        /// Returns the condition code that results upon swapping
        /// the sides of a comparison.
        /// </summary>
        /// <param name="cc">The condition code to mirror.</param>
        /// <returns>The mirrored condition code.</returns>
        public static ConditionCode Mirror(this ConditionCode cc)
        {
            return mirrorMap[(int) cc];
        }
    }
}
