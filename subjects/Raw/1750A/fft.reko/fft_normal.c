// fft_normal.c
// Generated by decompiling fft.hex
// using Reko decompiler version 0.9.3.0.

#include "fft_normal.h"

// 0115: Register word16 fn0115(Register out (ptr16 Eq_34) gp14Out)
// Called from:
//      fn011B
word16 fn0115(struct Eq_34 & gp14Out)
{
	struct Eq_34 * gp14_11;
	word16 gp0_12 = fn011B(0x00, gp1, gp3, gp4, gp14, out gp14_11);
	gp14Out = gp14_11;
	return gp0_12;
}

// 011B: Register cui16 fn011B(Register cui16 gp0, Register word16 gp1, Register word16 gp3, Register word16 gp4, Register (ptr16 Eq_24) gp14, Register out Eq_25 gp14Out)
// Called from:
//      fn0100
//      fn0115
cui16 fn011B(cui16 gp0, word16 gp1, word16 gp3, word16 gp4, struct Eq_24 * gp14, union Eq_25 & gp14Out)
{
	do
	{
		ci16 gp1_6 = -gp1;
		if ((__shift_arithmetic(gp3, gp1_6) & 0x01) == 0x00)
		{
			while (true)
			{
				ci16 gp3_15 = *(ci16 *) 0x8A40;
				cui16 gp0_16 = *(cui16 *) 39489;
				if (gp3_15 > 0x0F)
					break;
				gp14->w0001 = gp3_15;
				fn0115(out gp14);
			}
			gp14Out = <invalid>;
			return gp0_16;
		}
		gp0 |= __shift_logical(gp4, 0x03 - gp1_6);
		gp1 = gp1_6 + 0x01;
	} while (gp1_6 <= 0x02);
	gp14Out.u0 = <invalid>;
	return gp0;
}

// 014E: void fn014E()
void fn014E()
{
	struct Eq_84 * gp14_46 = fp - 0x02;
	ui16 gp9_6 = 0x00;
	do
	{
		struct Eq_90 * gp8_11 = gp9_6 * 0x03;
		real48 gp5_gp6_gp7_14 = gp8_11->r8030;
		real48 gp2_gp3_gp4_15 = gp8_11->r8060;
		gp14_46[1] = (struct Eq_84) gp9_6;
		Eq_105 gp5_gp6_gp7_36 = gp5_gp6_gp7_14 * gp5_gp6_gp7_14 + gp2_gp3_gp4_15 * gp2_gp3_gp4_15;
		word16 gp7_37 = (word16) gp5_gp6_gp7_36;
		gp14_46 = fn0245(gp5_gp6_gp7_36, gp7_37, gp7_37);
		real48 gp0_gp1_gp2_83 = <invalid>;
		*((word16) gp14_46[1].w0002 + 0x0000808E) = (struct Eq_84) gp0_gp1_gp2_83;
		ci16 gp9_70 = gp14_46[2];
		gp9_6 = gp9_70 + 0x01;
	} while (gp9_70 <= 0x0E);
}

// 0226: void fn0226(Register cui16 gp1, Register (ptr16 (ptr16 Eq_134)) gp3)
// Called from:
//      fn0245
void fn0226(cui16 gp1, struct Eq_134 ** gp3)
{
	struct Eq_134 * gp5_5 = *gp3;
	cu16 gp4_13 = gp1 & 0xFF;
	if (Test(NE,(gp1 & 0xFF & 0x0100) == 0x00))
		gp4_13 = gp1 & 0xFF | 0xFF00;
	if (Test(NE,(gp4_13 & 0x8000) == 0x00))
		++gp4_13;
	gp5_5->w0001 = (gp4_13 >> 0x01 & 0xFF) + 0x01 & 0xFF;
	gp5_5->w0002 = 0x00;
	gp5_5->w0000 = 0x4000;
}

// 0245: Register ptr16 fn0245(Sequence Eq_105 gp0_gp1, Register word16 gp2, Register word16 gp7)
// Called from:
//      fn014E
ptr16 fn0245(Eq_105 gp0_gp1, word16 gp2, word16 gp7)
{
	cui16 gp1 = (word16) gp0_gp1;
	if (gp0_gp1 < 0x00)
		return fp - 0x03;
	fn0226(gp1, (struct Eq_134 **) 0x01);
	while (true)
		;
}

// 0273: void fn0273()
void fn0273()
{
}

// 02E1: void fn02E1()
void fn02E1()
{
}

// 02FA: void fn02FA()
void fn02FA()
{
}

// 0311: Register ptr16 fn0311(Register ci16 gp1, Stack cui16 wArg1A)
// Called from:
//      fn037C
//      fn03A0
ptr16 fn0311(ci16 gp1, cui16 wArg1A)
{
	if (gp1 >= 0x00)
		return fp;
	struct Eq_194 * gp2_25 = wArg1A + 0x07 & ~0x07;
	while (true)
	{
		Eq_202 gp1_gp2_45 = (uint32) (gp1 - gp2_25);
		do
			;
		while (gp1_gp2_45 > 0x07);
		gp2_25 = gp2_25->ptr032C;
	}
}

// 037C: void fn037C(Sequence int32 gp0_gp1, Sequence int32 gp12_gp13, Register word16 gp2, Register cui16 gp3, Register (ptr16 Eq_211) gp11, Stack cui16 wArg18)
void fn037C(int32 gp0_gp1, int32 gp12_gp13, word16 gp2, cui16 gp3, struct Eq_211 * gp11, cui16 wArg18)
{
	cui16 gp1 = (word16) gp0_gp1;
	struct Eq_4 * gp0 = SLICE(gp0_gp1, word16, 16);
	struct Eq_217 * gp12 = SLICE(gp12_gp13, word16, 16);
	int32 gp1_gp2_18 = SEQ(gp1, gp2);
	if (gp0_gp1 >= 0x00)
		fn03E2(gp1_gp2_18, gp0, gp3, gp1, gp2, gp11);
	else
	{
		real48 gp2_gp3_gp4_42 = g_rFFFF8054;
		real48 gp5_gp6_gp7_57 = (SEQ((word32) gp2_gp3_gp4_42, (word16) gp2_gp3_gp4_42) - SEQ(gp0_gp1, gp2)) * *((char *) (&g_rFFFF8054) + 3);
		Eq_235 gp0_gp1_62 = (int32) gp5_gp6_gp7_57;
		if (gp5_gp6_gp7_57 - (real48) gp0_gp1_62 >= 0.0)
		{
			Eq_241 gp0_gp1_82 = gp0_gp1_62 + gp12->r0020;
			fn03F1(gp0_gp1_82, gp12_gp13, gp2, gp11);
		}
		else
		{
			word32 gp0_gp1_110 = gp0_gp1_62 + Mem0[0x805D<p16>:word32];
			if (gp0_gp1 >= 0x00)
				fn03F7(gp0_gp1_110, gp12_gp13, gp1, gp11);
			else
				fn0311(SLICE(gp0_gp1_110 + Mem0[0x805F<p16>:word32], word16, 0), wArg18);
		}
	}
}

// 03A0: void fn03A0(Sequence int32 gp0_gp1, Sequence int32 gp12_gp13, Register word16 gp2, Register word16 gp8, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11, Stack cui16 wArg18)
void fn03A0(int32 gp0_gp1, int32 gp12_gp13, word16 gp2, word16 gp8, cui16 gp9, word16 gp10, struct Eq_288 * gp11, cui16 wArg18)
{
	Eq_241 gp1_gp2_14 = SEQ((word16) gp0_gp1, gp2);
	if (gp0_gp1 >= 0x00)
		fn0404(gp1_gp2_14, gp12_gp13, gp9, gp10, gp11);
	else
	{
		real48 gp2_gp3_gp4_48 = g_rFFFF8061 - SEQ(gp0_gp1, gp2);
		real48 gp5_gp6_gp7_57 = SEQ((word32) gp2_gp3_gp4_48, (word16) gp2_gp3_gp4_48) * *((char *) (&g_rFFFF8061) + 3);
		int32 gp0_gp1_62 = (int32) gp5_gp6_gp7_57;
		real48 gp5_gp6_gp7_71 = gp5_gp6_gp7_57 - (real48) gp0_gp1_62;
		word16 gp7_72 = (word16) gp5_gp6_gp7_71;
		word16 gp5_74 = SLICE(gp5_gp6_gp7_71, word16, 32);
		if (gp5_gp6_gp7_71 >= 0.0)
			fn0412(gp12_gp13, SEQ(gp7_72, gp8), gp5_74, gp9, gp10, gp11);
		else
			fn0311((word16) (*((char *) &g_dwFFFF806A + 2) + (gp0_gp1_62 + g_dwFFFF806A) / 39491), wArg18);
	}
}

// 03BF: void fn03BF(Register (ptr16 Eq_4) gp0, Register int16 gp1, Register cui16 gp3, Stack word16 wArgC7)
void fn03BF(struct Eq_4 * gp0, int16 gp1, cui16 gp3, word16 wArgC7)
{
	gp0->t0000.u1 = 0x2020;
	gp0[1] = (struct Eq_4) 0x2020;
	gp0[2] = (struct Eq_4) 0x2020;
	gp0[3] = (struct Eq_4) 0x00;
	cui16 gp13_48 = 0x00;
	while (true)
	{
		word16 gp10_51 = 0x20;
		int16 gp1_53 = gp1;
		if (gp1 < 0x00)
		{
			gp10_51 = 0x2D;
			gp1_53 = -gp1;
		}
		gp1 = gp1_53 / 0x0A;
		gp13_48 |= 0x30;
		if (true)
			break;
		gp0[2] = (struct Eq_4) SEQ(gp0[3], (byte) gp13_48);
	}
	gp0[2] = (struct Eq_4) SEQ((byte) gp13_48, gp0[2]);
	fn03E2(SEQ(gp1, gp1 % 0x0A), gp0, gp3, 0x01, gp10_51, gp0 - 0x01);
}

// 03E2: void fn03E2(Sequence int32 gp1_gp2, Register (ptr16 Eq_4) gp0, Register cui16 gp3, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_211) gp11)
// Called from:
//      fn037C
//      fn03BF
void fn03E2(int32 gp1_gp2, struct Eq_4 * gp0, cui16 gp3, cui16 gp9, word16 gp10, struct Eq_211 * gp11)
{
	if (PZN)
		fn0432(gp1_gp2, gp0, gp3);
	else if ((gp9 & 0x8000) != 0x00)
		gp11[2] = (struct Eq_211) SEQ((byte) gp10, gp11[2]);
	else
		gp11[2] = (struct Eq_211) SEQ(gp11[3], (byte) gp10);
}

// 03ED: void fn03ED(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register (ptr16 Eq_211) gp0)
void fn03ED(Eq_241 gp1_gp2, int32 gp12_gp13, struct Eq_211 * gp0)
{
	fn03F1(gp1_gp2, gp12_gp13, 0x2020, gp0);
}

// 03F1: void fn03F1(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register word16 gp10, Register (ptr16 Eq_211) gp11)
// Called from:
//      fn037C
//      fn03ED
void fn03F1(Eq_241 gp1_gp2, int32 gp12_gp13, word16 gp10, struct Eq_211 * gp11)
{
	fn03F2(gp1_gp2, gp12_gp13, 0x06, gp10, gp11);
}

// 03F2: void fn03F2(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_211) gp11)
// Called from:
//      fn037C
//      fn03F1
void fn03F2(Eq_241 gp1_gp2, int32 gp12_gp13, cui16 gp9, word16 gp10, struct Eq_211 * gp11)
{
	do
	{
		gp11->w0000 = gp10;
		++gp11;
		--gp9;
	} while (gp9 != 0x00);
	fn03F7(gp1_gp2, gp12_gp13, gp9, gp11);
}

// 03F7: void fn03F7(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register cui16 gp9, Register (ptr16 Eq_211) gp11)
// Called from:
//      fn037C
void fn03F7(Eq_241 gp1_gp2, int32 gp12_gp13, cui16 gp9, struct Eq_211 * gp11)
{
	ci16 gp1 = SLICE(gp1_gp2, word16, 16);
	word16 gp2 = (word16) gp1_gp2;
	gp11->wFFFFFFFF = SEQ(gp11[2], (byte) gp9);
	if (gp1 >= 0x00)
		fn0407(gp1_gp2, gp12_gp13, gp9, 0x20, gp11 - 0x01);
	else if (gp1 != 0x8000)
		fn0406(gp1_gp2, gp12_gp13, gp9, 0x2D, gp11 - 0x01);
	else if (gp2 != 0x00)
		fn0406(gp1_gp2, gp12_gp13, gp9, 0x2D, gp11 - 0x01);
	else
		fn0405(gp12_gp13, gp1, gp2, gp9, 0x2D, gp11 - 0x01);
}

// 0404: void fn0404(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn03A0
void fn0404(Eq_241 gp1_gp2, int32 gp12_gp13, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	ci16 gp1 = SLICE(gp1_gp2, word16, 16);
	word16 gp2 = (word16) gp1_gp2;
	if (Z)
		fn0406(gp1_gp2, gp12_gp13, gp9, gp10, gp11);
	else
		fn0405(gp12_gp13, gp1, gp2, gp9, gp10, gp11);
}

// 0405: void fn0405(Sequence int32 gp12_gp13, Register ci16 gp1, Register word16 gp2, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn037C
//      fn0404
void fn0405(int32 gp12_gp13, ci16 gp1, word16 gp2, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	fn0406(SEQ(gp1, gp2 + 0x01), gp12_gp13, gp9, gp10, gp11);
}

// 0406: void fn0406(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn037C
//      fn0404
void fn0406(Eq_241 gp1_gp2, int32 gp12_gp13, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	Eq_241 gp1_gp2_6 = -gp1_gp2;
	fn0407(gp1_gp2_6, gp12_gp13, gp9, gp10, gp11);
}

// 0407: void fn0407(Sequence Eq_241 gp1_gp2, Sequence int32 gp12_gp13, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn037C
void fn0407(Eq_241 gp1_gp2, int32 gp12_gp13, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	word16 gp5_26 = (word16) (gp1_gp2 - (gp1_gp2 / 0x0A) * 0x0A);
	if (Test(EQ,(gp9 + 0x01 & 0x8000) == 0x00))
		fn0417(gp12_gp13, 0x0A, gp5_26 + 0x30, gp9 + 0x01, gp10, gp11);
	else
		fn0413(gp12_gp13, 0x0A, gp5_26 + 0x30, gp9 + 0x01, gp10, gp11);
}

// 0412: void fn0412(Sequence int32 gp12_gp13, Sequence int32 gp7_gp8, Register word16 gp5, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn03A0
void fn0412(int32 gp12_gp13, int32 gp7_gp8, word16 gp5, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	if (Z)
		fn0417(gp12_gp13, gp7_gp8, gp5, gp9, gp10, gp11);
	else
		fn0413(gp12_gp13, gp7_gp8, gp5, gp9, gp10, gp11);
}

// 0413: void fn0413(Sequence int32 gp12_gp13, Sequence int32 gp7_gp8, Register word16 gp5, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn037C
//      fn0412
void fn0413(int32 gp12_gp13, int32 gp7_gp8, word16 gp5, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	gp11->t0000 = SEQ((byte) gp5, gp11->t0000);
	fn0417(gp12_gp13, gp7_gp8, gp5, gp9, gp10, gp11 - 0x01);
}

// 0417: void fn0417(Sequence int32 gp12_gp13, Sequence int32 gp7_gp8, Register word16 gp5, Register cui16 gp9, Register word16 gp10, Register (ptr16 Eq_288) gp11)
// Called from:
//      fn037C
//      fn0412
void fn0417(int32 gp12_gp13, int32 gp7_gp8, word16 gp5, cui16 gp9, word16 gp10, struct Eq_288 * gp11)
{
	gp11->t0000 = SEQ(gp11->b0001, (byte) gp5);
	if (gp12_gp13 / gp7_gp8 != 0x00)
		fn046A();
	else if (Test(EQ,(gp9 + 0x01 & 0x8000) == 0x00))
		gp11->t0000 = SEQ(gp11->b0001, (byte) gp10);
	else
		gp11->t0000 = SEQ((byte) gp10, gp11->t0000);
}

// 0426: Register (ptr16 Eq_4) fn0426(Sequence word32 gp1_gp2, Register (ptr16 Eq_4) gp0)
// Called from:
//      fn04F6
struct Eq_4 * fn0426(word32 gp1_gp2, struct Eq_4 * gp0)
{
	cui16 gp2 = (word16) gp1_gp2;
	struct Eq_4 * gp1 = SLICE(gp1_gp2, word16, 16);
	gp0[7] = (struct Eq_4) 0x00;
	fn043E(0x00, 0x06, gp0, gp1, gp2, 0x00);
	return gp1;
}

// 0432: void fn0432(Sequence int32 gp1_gp2, Register (ptr16 Eq_4) gp0, Register cui16 gp3)
// Called from:
//      fn037C
void fn0432(int32 gp1_gp2, struct Eq_4 * gp0, cui16 gp3)
{
	cui16 gp2 = (word16) gp1_gp2;
	struct Eq_4 * gp1 = SLICE(gp1_gp2, word16, 16);
	gp0[0x0A] = (struct Eq_4) 0x00;
	fn043E(0x00, 11, gp0, gp1, gp2, gp3);
}

// 043E: void fn043E(Register cui16 gp0, Register word16 gp1, Register (ptr16 Eq_4) gp11, Register (ptr16 Eq_4) gp12, Register cui16 gp13, Register cui16 gp14)
// Called from:
//      fn037C
//      fn0426
void fn043E(cui16 gp0, word16 gp1, struct Eq_4 * gp11, struct Eq_4 * gp12, cui16 gp13, cui16 gp14)
{
	while (true)
	{
		real48 gp12_gp13_gp14_300;
		word16 gp0_16;
		if (Test(NE,(gp12 & 0x01) == 0x00))
		{
			if (Test(EQ,(gp12 & 0x02) == 0x00))
			{
				gp0_16 = 0x2D20;
				gp12_gp13_gp14_300 = SEQ(gp12, gp13, gp14) * g_r057F;
				goto l0458;
			}
		}
		else
		{
			gp0 = gp0 | gp12 | gp13 | gp14;
			if (gp0 == 0x00 || Test(NE,(gp12 & 0x02) == 0x00))
			{
				gp0_16 = 0x2B20;
				gp12_gp13_gp14_300 = SEQ(gp12, gp13, gp14);
l0458:
				gp11->t0000.u1 = gp0_16;
				int16 gp2_179 = 0x00;
				real48 gp12_gp13_gp14_178 = gp12_gp13_gp14_300;
				if (Test(NE,(SLICE(gp12_gp13_gp14_300, word16, 16) & 0x0100) == 0x00))
				{
					fn046A();
					return;
				}
				else
				{
					for (; gp12_gp13_gp14_178 >= g_r0579; gp12_gp13_gp14_178 /= g_r0579)
						++gp2_179;
					if (gp1 != 0x06)
						gp11[8] = (struct Eq_4) 17707;
					else
						gp11[5] = (struct Eq_4) 17707;
					int16 gp2_83 = gp2_179 / 0x0A;
					Eq_720 gp2_86 = __xbr(gp2_83) | gp2_83 % 0x0A;
					if (gp1 != 0x06)
						gp11[9] = (struct Eq_4) (gp2_86 | 0x3030);
					else
						gp11[6] = (struct Eq_4) (gp2_86 | 0x3030);
					int32 gp2_gp3_108 = (int32) gp12_gp13_gp14_178;
					gp11->t0000.u1 = SEQ(gp11[1], (byte) ((word16) gp2_gp3_108 + 0x30));
					gp11[1] = (struct Eq_4) SEQ(0x2E, gp11[1]);
					cui16 gp7_155 = 0x00;
					real48 gp12_gp13_gp14_117 = gp12_gp13_gp14_178 - (real48) gp2_gp3_108;
					do
					{
						real48 gp12_gp13_gp14_135 = gp12_gp13_gp14_117 * g_r0579;
						int32 gp2_gp3_144 = (int32) SEQ((word32) gp12_gp13_gp14_135, (word16) gp12_gp13_gp14_135);
						word16 gp3_146 = (word16) gp2_gp3_144;
						gp12_gp13_gp14_117 = gp12_gp13_gp14_135 - (real48) gp2_gp3_144;
						++gp7_155;
						if ((gp7_155 & 0x8000) != 0x00)
							gp11[1] = (struct Eq_4) SEQ((byte) (gp3_146 + 0x30), gp11[1]);
						else
						{
							gp11[1] = (struct Eq_4) SEQ(gp11[2], (byte) (gp3_146 + 0x30));
							++gp11;
						}
						--gp1;
					} while (gp1 != 0x00);
					gp11[1] = (struct Eq_4) 0x2020;
					return;
				}
			}
		}
		gp12 = (struct Eq_4 *) 0x07;
		if (gp1 != 0x06)
			gp12 = (struct Eq_4 *) 0x0A;
		__mov(gp11, 1410);
	}
}

// 0467: void fn0467(Sequence real48 gp12_gp13_gp14)
// Called from:
//      fn046A
void fn0467(real48 gp12_gp13_gp14)
{
	if (gp12_gp13_gp14 >= g_r057C)
		fn04AC();
	else
		fn046A();
}

// 046A: void fn046A()
// Called from:
//      fn037C
//      fn043E
//      fn0467
void fn046A()
{
}

// 04AC: void fn04AC()
// Called from:
//      fn043E
//      fn04AF
//      fn04C0
void fn04AC()
{
}

// 04AF: void fn04AF(Register (ptr16 byte) gp0)
// Called from:
//      fn04F6
void fn04AF(byte * gp0)
{
	if (SEQ(*gp0, 0x00) == 0x00)
		return;
	fn04AC();
}

// 04C0: void fn04C0(Register uint16 gp0)
// Called from:
//      fn04CE
void fn04C0(uint16 gp0)
{
	if ((gp0 & 0x0F) >= 0x09)
		fn04AC();
	else
		fn04AC();
}

// 04CE: void fn04CE(Register cu16 gp0)
void fn04CE(cu16 gp0)
{
	fn04C0(gp0 >> 0x04);
}

// 04F6: void fn04F6(Sequence word32 gp0_gp1)
void fn04F6(word32 gp0_gp1)
{
	fn04AF(fn0426(gp0_gp1, &g_tFFFF806E));
}
