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

using NUnit.Framework;
using Reko.Arch.Msp430;
using Reko.Core;
using Reko.Core.Machine;

namespace Reko.UnitTests.Arch.Msp430
{
    public class Msp430XRewriterTests : RewriterTestBase
    {
        private readonly Msp430Architecture arch;
        private readonly Address addr;

        public Msp430XRewriterTests()
        {
            this.arch = new Msp430Architecture(CreateServiceContainer(), "msp430", new()
            {
                { ProcessorOption.InstructionSet, "MSP430X" }
            });
            this.addr = Address.Ptr16(0x0100);
        }

        public override IProcessorArchitecture Architecture => arch;
        public override Address LoadAddress => addr;

        [Test]
        public void Msp430XRw_mov()
        {
            Given_Bytes(0x3C, 0x40, 0xA0, 0xEE);	// mov.w	#EEA0,r12
            AssertCode(
                "0|L--|0100(4): 2 instructions",
                "1|L--|v4 = CONVERT(0xEEA0<16>, word16, word20)",
                "2|L--|r12 = v4");
        }

        [Test]
        public void Msp430XRw_xor()
        {
            Given_Bytes(0xA0, 0xEE, 0x3C, 0x90);	// xor.w	@r14,-6FC4(pc)
            AssertCode(
                "0|L--|0100(4): 5 instructions",
                "1|L--|v4 = Mem0[r14:word16]",
                "2|L--|v5 = Mem0[0x913E<p16>:word16]",
                "3|L--|v5 = v5 ^ v4",
                "4|L--|Mem0[0x913E<p16>:word16] = v5",
                "5|L--|VNZC = cond(v5)");
        }

        [Test]
        public void Msp430XRw_cmp()
        {
            Given_Bytes(0x3C, 0x90, 0xA0, 0xEE);	// cmp.w	#EEA0,r12
            AssertCode(
                "0|L--|0100(4): 1 instructions",
                "1|L--|VNZC = cond(r12 - 0xEEA0<16>)");
        }

        [Test]
        public void Msp430XRw_jz()
        {
            Given_Bytes(0x07, 0x24);	// jz	0118
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(EQ,Z)) branch 0110");
        }

        [Test]
        public void Msp430XRw_call()
        {
            Given_Bytes(0x8D, 0x12);	// call	r13
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|call r13 (2)");
        }

        [Test]
        public void Msp430XRw_sub()
        {
            Given_Bytes(0x3D, 0x80, 0xA0, 0xEE);	// sub.w	#EEA0,r13
            AssertCode(
                "0|L--|0100(4): 4 instructions",
                "1|L--|v4 = SLICE(r13, word16, 0)",
                "2|L--|v5 = v4 - 0xEEA0<16>",
                "3|L--|r13 = CONVERT(v5, word16, word20)",
                "4|L--|VNZC = cond(v5)");
        }

        [Test]
        public void Msp430XRw_rra_b()
        {
            Given_HexString("4411");
            AssertCode(
                "0|L--|0100(2): 3 instructions",
                "1|L--|r4 = r4 >> 1<8>",
                "2|L--|NZC = cond(r4)",
                "3|L--|V = false");
        }

        [Test]
        public void Msp430XRw_rra_w()
        {
            Given_Bytes(0x0D, 0x11);	// rra.w	r13
            AssertCode(
                "0|L--|0100(2): 3 instructions",
                "1|L--|r13 = r13 >> 1<8>",
                "2|L--|NZC = cond(r13)",
                "3|L--|V = false");
        }

        [Test]
        public void Msp430XRw_rrum()
        {
            Given_Bytes(0x5C, 0x03);	// rrum.w	r12
            AssertCode(
                "0|L--|0100(2): 5 instructions",
                "1|L--|v4 = SLICE(r12, byte, 0)",
                "2|L--|v5 = v4 >>u 1<8>",
                "3|L--|r12 = CONVERT(v5, byte, word20)",
                "4|L--|NZC = cond(v5)",
                "5|L--|V = false");
        }

        [Test]
        public void Msp430XRw_rrax()
        {
            Given_Bytes(0x4D, 0x18, 0x0C, 0x11);	// rpt #14 rrax.w	r12
            AssertCode(
                "0|L--|0100(4): 3 instructions",
                "1|L--|r12 = r12 >> 0xE<8>",
                "2|L--|NZC = cond(r12)",
                "3|L--|V = false");
        }

        [Test]
        public void Msp430XRw_add()
        {
            Given_Bytes(0x0D, 0x5C);	// add.w	r12,r13
            AssertCode(
                "0|L--|0100(2): 2 instructions",
                "1|L--|r13 = r13 + r12",
                "2|L--|VNZC = cond(r13)");
        }

        [Test]
        public void Msp430XRw_pushm()
        {
            Given_Bytes(0x1A, 0x15);	// pushm.w	#02,r10
            AssertCode(
                "0|L--|0100(2): 4 instructions",
                "1|L--|sp = sp - 4<i32>",
                "2|L--|Mem0[sp:word20] = r10",
                "3|L--|sp = sp - 4<i32>",
                "4|L--|Mem0[sp:word20] = r9");
        }

        [Test]
        public void Msp430XRw_jnz()
        {
            Given_Bytes(0x22, 0x20);	// jnz	0190
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(NE,Z)) branch 0146");
        }

        [Test]
        public void Msp430XRw_jc()
        {
            Given_Bytes(0x0B, 0x2C);	// jc	017A
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(ULT,C)) branch 0118");
        }

        [Test]
        public void Msp430XRw_jnc()
        {
            Given_Bytes(0xF5, 0x2B);	// jnc	0164
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(UGE,C)) branch 00EC");
        }

        [Test]
        public void Msp430XRw_popm_w()
        {
            Given_Bytes(0x19, 0x17);	// popm.w	#02,r9
            AssertCode(
                "0|L--|0100(2): 6 instructions",
                "1|L--|v5 = CONVERT(Mem0[sp:word16], word16, word20)",
                "2|L--|r8 = v5",
                "3|L--|sp = sp + 4<i32>",
                "4|L--|v7 = CONVERT(Mem0[sp:word16], word16, word20)",
                "5|L--|r9 = v7",
                "6|L--|sp = sp + 4<i32>");
        }

        [Test]
        public void Msp430XRw_addc()
        {
            Given_Bytes(0x6A, 0x64);	// addc.b	@r4,r10
            AssertCode(
                "0|L--|0100(2): 5 instructions",
                "1|L--|v5 = Mem0[r4:byte]",
                "2|L--|v7 = SLICE(r10, byte, 0)",
                "3|L--|v8 = v7 + v5 + C",
                "4|L--|r10 = CONVERT(v8, byte, word20)",
                "5|L--|VNZC = cond(v8)");
        }

        [Test]
        public void Msp430XRw_jge()
        {
            Given_Bytes(0x07, 0x34);	// jge	01FC
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(GE,VN)) branch 0110");
        }

        [Test]
        public void Msp430XRw_jmp()
        {
            Given_Bytes(0xF8, 0x3F);	// jmp	01F6
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|goto 00F2");
        }

        [Test]
        public void Msp430XRw_jl()
        {
            Given_Bytes(0x12, 0x38);	// jl	0272
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(LT,VN)) branch 0126");
        }

        [Test]
        public void Msp430XRw_and()
        {
            Given_Bytes(0xFE, 0xFF, 0xF9, 0x3F);	// and.b	@r15+,3FF9(r14)
            AssertCode(
                "0|L--|0100(4): 7 instructions",
                "1|L--|v4 = Mem0[r15:byte]",
                "2|L--|r15 = r15 + 1<i20>",
                "3|L--|v6 = Mem0[r14 + 16377<i20>:byte]",
                "4|L--|v6 = v6 & v4",
                "5|L--|Mem0[r14 + 16377<i20>:byte] = v6",
                "6|L--|NZC = cond(v6)",
                "7|L--|V = false");
        }

        [Test]
        public void Msp430XRw_subc()
        {
            Given_Bytes(0xB1, 0x79, 0x0E, 0x20);	// subc.w	@r9+,200E(sp)
            AssertCode(
                "0|L--|0100(4): 6 instructions",
                "1|L--|v5 = Mem0[r9:word16]",
                "2|L--|r9 = r9 + 2<i20>",
                "3|L--|v7 = Mem0[sp + 8206<i20>:word16]",
                "4|L--|v7 = v7 - v5 - C",
                "5|L--|Mem0[sp + 8206<i20>:word16] = v7",
                "6|L--|VNZC = cond(v7)");
        }

        [Test]
        public void Msp430XRw_bis()
        {
            Given_Bytes(0x0C, 0xDD);	// bis.w	r13,r12
            AssertCode(
                "0|L--|0100(2): 1 instructions",
                "1|L--|r12 = r12 | r13");
        }

        [Test]
        public void Msp430XRw_bit()
        {
            Given_Bytes(0x66, 0xB1);	// bit.b	@sp,r6
            AssertCode(
                "0|L--|0100(2): 6 instructions",
                "1|L--|v5 = Mem0[sp:byte]",
                "2|L--|v6 = CONVERT(v5, byte, word20)",
                "3|L--|v7 = r6 & v6",
                "4|L--|NZ = cond(v7)",
                "5|L--|C = Test(NE,v7)",
                "6|L--|V = false");
        }

        [Test]
        public void Msp430XRw_bic()
        {
            Given_Bytes(0x16, 0xCB, 0x1C, 0x4A);	// bic.w	4A1C(r11),r6
            AssertCode(
                "0|L--|0100(4): 4 instructions",
                "1|L--|v4 = Mem0[r11 + 18972<i20>:word16]",
                "2|L--|v6 = SLICE(r6, word16, 0)",
                "3|L--|v7 = v6 & ~v4",
                "4|L--|r6 = CONVERT(v7, word16, word20)");
        }

        [Test]
        public void Msp430XRw_rrc()
        {
            Given_Bytes(0x04, 0x10);	// rrc.w	pc
            AssertCode(
                "0|L--|0100(2): 3 instructions",
                "1|L--|r4 = __rcr<word20,byte>(r4, 1<8>, C)",
                "2|L--|NZC = cond(r4)",
                "3|L--|V = false");
        }

        [Test]
        public void Msp430XRw_dadd()
        {
            Given_Bytes(0xB0, 0xA4, 0x3E, 0x40);	// dadd.w	@r4+,403E(pc)
            AssertCode(
                "0|L--|0100(4): 6 instructions",
                "1|L--|v4 = Mem0[r4:word16]",
                "2|L--|r4 = r4 + 2<i20>",
                "3|L--|v5 = Mem0[0x4140<p16>:word16]",
                "4|L--|v5 = __dadd<word16>(v5, v4, C)",
                "5|L--|Mem0[0x4140<p16>:word16] = v5",
                "6|L--|NZC = cond(v5)");
        }

        [Test]
        public void Msp430XRw_jn()
        {
            Given_Bytes(0x72, 0x31);	// jn	78E0
            AssertCode(
                "0|T--|0100(2): 1 instructions",
                "1|T--|if (Test(SG,N)) branch 03E6");
        }

        [Test]
        public void Msp430XRw_sxt()
        {
            Given_Bytes(0x8F, 0x11);	// sxt.w	r15
            AssertCode(
                "0|L--|0100(2): 4 instructions",
                "1|L--|v4 = SLICE(r15, byte, 0)",
                "2|L--|r15 = CONVERT(v4, byte, word20)",
                "3|L--|NZC = cond(r15)",
                "4|L--|V = false");
        }

        [Test]
        public void Msp430XRw_swpb()
        {
            Given_Bytes(0x8F, 0x10);	// swpb	r15
            AssertCode(
                "0|L--|0100(2): 3 instructions",
                "1|L--|v4 = SLICE(r15, word16, 0)",
                "2|L--|v5 = CONVERT(__swpb(v4), word16, word20)",
                "3|L--|r15 = v5");
        }

        [Test]
        public void Msp430XRw_swpb_mem()
        {
            Given_HexString("A110"); // A39022DE16323851D8D4877F2AC2");
            AssertCode(
                "0|L--|0100(2): 2 instructions",
                "1|L--|v4 = Mem0[sp:word16]",
                "2|L--|Mem0[sp:word16] = __swpb(v4)");
        }

        [Test]
        public void Msp430XRw_mova()
        {
            Given_Bytes(0x05, 0x04);	// mova.a	@r4,r5
            AssertCode(
                "0|L--|0100(2): 2 instructions",
                "1|L--|v4 = Mem0[r4:word20]",
                "2|L--|r5 = v4");
        }

        [Test]
        public void Msp430XRw_mova_r_ix()
        {
            Given_HexString("7405F2B6");
            AssertCode(
                "0|L--|0100(4): 1 instructions",
                "1|L--|Mem0[r4 + -18702<i20>:word20] = r5");
        }

        [Test]
        public void Msp430XRw_sub_sp()
        {
            Given_Bytes(0x21, 0x83); // sub.w #0002,sp
            AssertCode(
                "0|L--|0100(2): 4 instructions",
                "1|L--|v4 = SLICE(sp, word16, 0)",
                "2|L--|v5 = v4 - 2<16>",
                "3|L--|sp = CONVERT(v5, word16, word20)",
                "4|L--|VNZC = cond(v5)");
        }

        [Test]
        public void Msp430XRw_goto()
        {
            Given_Bytes(0x30, 0x40, 0x4C, 0x41);
            AssertCode(         // "mov.w\t#414C,pc"
                "0|T--|0100(4): 1 instructions",
                "1|T--|goto 414C");
        }

        [Test]
        public void Msp430XRw_ret()
        {
            Given_Bytes(0x30, 0x41);
            AssertCode(         // ret
                "0|R--|0100(2): 1 instructions",
                "1|R--|return (2,0)");
        }

        [Test]
        public void Msp430XRw_add_two_abs()
        {
            Given_HexString("9252 9C57 7877");
            AssertCode(         // add.w\t&579C,&7778
                "0|L--|0100(6): 5 instructions",
                "1|L--|v3 = Mem0[0x579C<p16>:word16]",
                "2|L--|v4 = Mem0[0x7778<p16>:word16]",
                "3|L--|v4 = v4 + v3",
                "4|L--|Mem0[0x7778<p16>:word16] = v4",
                "5|L--|VNZC = cond(v4)");
        }

        [Test]
        public void Msp430XRw_add_b_pcrel_pcrel()
        {
            Given_HexString("D050 6647 40F7");
            AssertCode(         // add.b\t4768(pc),-08BC(pc)
                "0|L--|0100(6): 5 instructions",
                "1|L--|v4 = Mem0[0x4868<p16>:byte]",
                "2|L--|v5 = Mem0[0xF844<p16>:byte]",
                "3|L--|v5 = v5 + v4",
                "4|L--|Mem0[0xF844<p16>:byte] = v5",
                "5|L--|VNZC = cond(v5)");
        }

        [Test]
        public void Msp430XRw_addw()
        {
            Given_HexString("1E53");
            AssertCode(         // add.w #0001,r14
                "0|L--|0100(2): 4 instructions",
                "1|L--|v4 = SLICE(r14, word16, 0)",
                "2|L--|v5 = v4 + 1<16>",
                "3|L--|r14 = CONVERT(v5, word16, word20)",
                "4|L--|VNZC = cond(v5)");
        }
    }
}
