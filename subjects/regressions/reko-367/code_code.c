// code_code.c
// Generated by decompiling code.bin
// using Reko decompiler version 0.12.1.0.

#include "code.h"

// 80000000: void fn80000000()
void fn80000000()
{
	fn800003CC();
}

// 80000132: Register real96 fn80000132(Stack real96 rArg04, Stack real96 rArg10)
// Called from:
//      fn800001F2
//      fn800002AE
//      fn800003CC
real96 fn80000132(real96 rArg04, real96 rArg10)
{
	real96 fp0_n = *(real96 *) 0x800004FC;
	int32 dwLoc14_n = 0x00;
	while (true)
	{
		real96 rLoc10_n = SEQ((word32) fp0_n, SLICE(fp0_n, word32, 32), SLICE(fp0_n, word32, 64));
		if ((real96) dwLoc14_n >= rArg10)
			break;
		fp0_n = rLoc10_n * rArg04;
		++dwLoc14_n;
	}
	return fp0_n;
}

// 8000018E: Register real96 fn8000018E(Stack real96 rArg04)
// Called from:
//      fn800001F2
//      fn800002AE
//      fn800003CC
real96 fn8000018E(real96 rArg04)
{
	real96 fp0_n = *(real96 *) 0x80000508;
	int32 dwLoc14_n = 0x01;
	while (true)
	{
		real96 rLoc10_n = SEQ((word32) fp0_n, SLICE(fp0_n, word32, 32), SLICE(fp0_n, word32, 64));
		if ((real96) dwLoc14_n > rArg04)
			break;
		fp0_n = rLoc10_n *96 (real80) dwLoc14_n;
		++dwLoc14_n;
	}
	return fp0_n;
}

// 800001F2: Register real96 fn800001F2(Stack real96 rArg04)
// Called from:
//      fn8000036C
//      fn800003CC
real96 fn800001F2(real96 rArg04)
{
	real96 rLoc1C_n = *(real96 *) 0x80000514;
	int32 dwLoc20_n = 0x03;
	real96 fp1_n = rArg04;
	while (true)
	{
		real96 rLoc10_n = SEQ((word32) fp1_n, SLICE(fp1_n, word32, 32), SLICE(fp1_n, word32, 64));
		if (dwLoc20_n > 100)
			break;
		real96 fp0_n = (real96) (real80) ((real96) (real80) fn80000132(rArg04, (real96) dwLoc20_n) /96 (real80) fn8000018E((real96) dwLoc20_n)) * rLoc1C_n;
		fp1_n = rLoc10_n + (real80) fp0_n;
		rLoc1C_n = fp0_n;
		dwLoc20_n += 0x02;
	}
	return fp1_n;
}

// 800002AE: Register real96 fn800002AE(Stack real96 rArg04)
// Called from:
//      fn8000036C
//      fn800003CC
real96 fn800002AE(real96 rArg04)
{
	real96 fp0_n = *(real96 *) 0x80000520;
	real96 rLoc1C_n = *(real96 *) 0x8000052C;
	int32 dwLoc20_n = 0x02;
	while (true)
	{
		real96 rLoc10_n = SEQ((word32) fp0_n, SLICE(fp0_n, word32, 32), SLICE(fp0_n, word32, 64));
		if (dwLoc20_n > 100)
			break;
		real96 fp0_n = (real96) (real80) ((real96) (real80) fn80000132(rArg04, (real96) dwLoc20_n) /96 (real80) fn8000018E((real96) dwLoc20_n)) * rLoc1C_n;
		fp0_n = rLoc10_n + (real80) fp0_n;
		rLoc1C_n = fp0_n;
		dwLoc20_n += 0x02;
	}
	return fp0_n;
}

// 8000036C: void fn8000036C(Stack real96 rArg04)
// Called from:
//      fn800003CC
void fn8000036C(real96 rArg04)
{
	fn800001F2(rArg04);
	fn800002AE(rArg04);
}

// 800003CC: void fn800003CC()
// Called from:
//      fn80000000
void fn800003CC()
{
	real96 fp0_n = *(real96 *) 0x80000538;
	fn80000132(fp0_n, fp0_n);
	fn8000018E(fp0_n);
	fn800001F2(fp0_n);
	fn800002AE(fp0_n);
	fn8000036C(fp0_n);
}

