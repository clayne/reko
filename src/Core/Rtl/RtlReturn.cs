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

using System.IO;

namespace Reko.Core.Rtl
{
    /// <summary>
    /// Models an RTL return instruction.
    /// </summary>
    public sealed class RtlReturn : RtlInstruction
    {
        /// <summary>
        /// Constructs an <see cref="RtlReturn"/> instruction.
        /// </summary>
        /// <param name="returnAddressBytes">Number of bytes the return address occupies
        /// on the stack.
        /// </param>
        /// <param name="extraBytesPopped">Any extra bytes removed from the stack, excluding
        /// the return address.
        /// </param>
        /// <param name="rtlClass"><see cref="InstrClass"/> of this instruction.
        /// </param>
        public RtlReturn(int returnAddressBytes, int extraBytesPopped, InstrClass rtlClass)
        {
            this.ReturnAddressBytes = returnAddressBytes;
            this.ExtraBytesPopped = extraBytesPopped;
            this.Class = rtlClass;
        }

        /// <summary>
        /// The stack size of the return address in bytes.
        /// </summary>
        /// <remarks>
        /// Architectures where the return address is not passed on the stack should specify 0 as the
        /// size of this property.
        /// </remarks>
        public int ReturnAddressBytes { get; }

        /// <summary>
        /// Number of bytes (or allocation unites) removed from the
        /// return stack in addition to the <see cref="ReturnAddressBytes"/>.
        /// </summary>
        /// <remarks>
        /// Most architectures will set this to 0. The x86 `ret NNNN` instruction.
        /// however, requires the extra recovered space to be modeled.
        /// </remarks>
        public int ExtraBytesPopped { get; }

        /// <inheritdoc/>
        public override T Accept<T>(RtlInstructionVisitor<T> visitor)
        {
            return visitor.VisitReturn(this);
        }

        /// <inheritdoc/>
        public override T Accept<T, C>(IRtlInstructionVisitor<T, C> visitor, C context)
        {
            return visitor.VisitReturn(this, context);
        }

        /// <inheritdoc/>
        protected override void WriteInner(TextWriter writer)
        {
            writer.Write("return ({0},{1})", ReturnAddressBytes, ExtraBytesPopped);
        }
    }
}
