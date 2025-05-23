#region License
/*
 * Copyright (C) 2018-2025 Stefano Moioli <smxdev4@gmail.com>.
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it 
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented;
 *    you must not claim that you wrote the original software.
 *    If you use this software in a product, an acknowledgment
 *    in the product documentation would be appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such,
 *    and must not be misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */
#endregion
using System;

namespace Reko.Core.IO
{
    /// <summary>
    /// Indicates the endianness of a field or structure.
    /// </summary>
    /* http://stackoverflow.com/a/2624377 */
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct, Inherited = true)]
    public class EndianAttribute : Attribute
    {
        /// <summary>
        /// Endianness of the field or structure.
        /// </summary>
        public Endianness Endianness { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="EndianAttribute"/> instance.
        /// </summary>
        /// <param name="endianness">Endiannesss to use.</param>
        public EndianAttribute(Endianness endianness)
        {
            this.Endianness = endianness;
        }
    }

    /// <summary>
    /// Represents the endianness of a field or structure.
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        /// Big-endian byte order.
        /// </summary>
        BigEndian,

        /// <summary>
        /// Little-endian byte order.
        /// </summary>
        LittleEndian
    }
}
