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

using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Types;

namespace Reko.Arch.M68k.Machine
{
    public class RegisterSetOperand : AbstractMachineOperand
    {
        public RegisterSetOperand(uint bitset, PrimitiveType dt)
            : base(dt)
        {
            BitSet = bitset;
        }

        public uint BitSet { get; }

        public static MachineOperand CreateReversed(ushort vv, PrimitiveType width)
        {
            int v;
            // Swap odd and even bits
            v = vv >> 1 & 0x5555 | (vv & 0x5555) << 1;
            // swap consecutive pairs
            v = v >> 2 & 0x3333 | (v & 0x3333) << 2;
            // swap nibbles
            v = v >> 4 & 0x0F0F | (v & 0x0F0F) << 4;
            // swap bytes
            v = v >> 8 & 0x00FF | (v & 0x00FF) << 8;
            return new RegisterSetOperand((ushort) v, width);
        }

        protected override void DoRender(MachineInstructionRenderer renderer, MachineInstructionRendererOptions options)
        {
            uint bitSet = BitSet;
            WriteRegisterSet(
                bitSet,
                renderer);
        }

        /// <summary>
        /// Write register mask. Bits are:
        ///  15  14  13  12  11  10   9   8   7   6   5   4   3   2   1   0
        ///  d0  d1  d2  d3  d4  d5  d6  d7  a0  a1  a2  a3  a4  a5  a6  a7
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bitPos"></param>
        /// <param name="incr"></param>
        /// <param name="regType"></param>
        /// <param name="renderer"></param>
        public void WriteRegisterSet(uint data, MachineInstructionRenderer renderer)
        {
            string sep = "";
            int maxReg = DataType.IsReal ? 8 : 16;
            int bitPos = maxReg - 1;
            for (int i = 0; i < maxReg; i++, --bitPos)
            {
                if (bit(data, bitPos))
                {
                    int first = i;
                    int run_length = 0;
                    while (i != 7 && i != 15 && bit(data, bitPos - 1))
                    {
                        --bitPos;
                        ++i;
                        ++run_length;
                    }
                    renderer.WriteString(sep);
                    renderer.WriteString(GetRegister(first).ToString());
                    if (run_length > 0)
                        renderer.WriteFormat("-{0}", GetRegister(first + run_length));
                    sep = "/";
                }
            }
        }

        private static bool bit(uint data, int pos) { return (data & 1 << pos) != 0; }

        private RegisterStorage GetRegister(int n)
        {
            if (DataType.IsReal)
                return Registers.GetRegister(n + Registers.fp0.Number);
            else
                return Registers.GetRegister(n);
        }
    }
}
