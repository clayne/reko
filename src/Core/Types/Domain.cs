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

using System;

namespace Reko.Core.Types
{
    /// <summary>
    /// A Domain specifies the possible interpretation of a datum.
    /// </summary>
    /// <remarks>
    /// A 32-bit load from memory could mean that the variable could be 
    /// treated as an signed int, unsigned int, floating point number, a 
    /// pointer to something. As the decompiler records how the value is used,
    /// some of these alternatives will be discarded. For instance, if the
    /// 32-bit word is used in a memory access, it is certain that it is a
    /// pointer to (something), and it can't be a float.
    /// </remarks>
	[Flags]
    public enum Domain
    {
        /// <summary>
        /// Domain of the Unit, or 'void' data type. 
        /// </summary>
        None = 0,

        /// <summary>
        /// Boolean data types.
        /// </summary>
        Boolean = 1,                // f
        
        /// <summary>
        /// Domain of character data.
        /// </summary>
        Character = 2,              // c

        /// <summary>
        /// Domain of signed integers.
        /// </summary>
        SignedInt = 4,              // i 

        /// <summary>
        /// Domain of unsigned integers.
        /// </summary>
        UnsignedInt = 8,            // u

        /// <summary>
        /// Either a signed or unsigned integer.
        /// </summary>
        Integer = SignedInt | UnsignedInt,

        /// <summary>
        /// Binary coded decimals.
        /// </summary>
        Bcd = 16,                   // b - Binary coded decimal; a decimal digit stored in each nybble of a byte.

        /// <summary>
        /// Floating point numbers.
        /// </summary>
        Real = 32,                  // r

        /// <summary>
        /// Pointers.
        /// </summary>
        Pointer = 64,               // p

        /// <summary>
        /// Segment-relative offsets.
        /// </summary>
        Offset = 128,               // n - "near pointer" (x86)

        /// <summary>
        /// Segment selectors.
        /// </summary>
        Selector = 256,             // S

        /// <summary>
        /// Segment pointers.
        /// </summary>
        SegPointer = 512,           // P - Segmented pointer (x86-style)

        /// <summary>
        /// Enumerated types.
        /// </summary>
        Enum = 0x400,               // An enumerated type.

        /// <summary>
        /// Composite types are constructed from other types.
        /// </summary>
        Composite = 0x1000,

        /// <summary>
        /// Structure types.
        /// </summary>
        Structure = 0x2000 | Composite, // A product type (T_1 x T_2 x ... x T_n)

        /// <summary>
        /// Arrays.
        /// </summary>
        Array = 0x4000 | Composite,     // An array of values

        /// <summary>
        /// Union types.
        /// </summary>
        Union = 0x6000 | Composite,     // A union type

        /// <summary>
        /// Class types.
        /// </summary>
        Class = 0x8000 | Composite,     // A C++ class, a fancy version of Structure

        /// <summary>
        /// Function types.
        /// </summary>
        Function = 0xA000 | Composite,  // Executable code.

        /// <summary>
        /// All types.
        /// </summary>
        Any = Boolean | Character | SignedInt | UnsignedInt | Bcd | Real | Pointer | Offset | Selector | SegPointer | Enum 
            | Structure | Array | Union | Class | Function
    }
}
