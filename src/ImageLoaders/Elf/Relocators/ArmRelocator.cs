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
using Reko.Core.Configuration;
using Reko.Core.Loading;
using Reko.Core.Services;
using Reko.Core.Types;
using System.Collections.Generic;

namespace Reko.ImageLoaders.Elf.Relocators
{
    public class ArmRelocator : ElfRelocator32
    {
        IProcessorArchitecture archThumb;
        private uint currentTlsSlotOffset;

        public ArmRelocator(ElfLoader32 loader, SortedList<Address, ImageSymbol> imageSymbols) : base(loader, imageSymbols)
        {
            this.currentTlsSlotOffset = 0x0000000;
            this.archThumb = null!;
        }

        public override Address AdjustAddress(Address address)
        {
            if ((address.ToLinear() & 1) == 0)
                return address;
            return address - 1;
        }

        public override ImageSymbol AdjustImageSymbol(ImageSymbol sym)
        {
            if (sym.Type != SymbolType.Code &&
                sym.Type != SymbolType.ExternalProcedure &&
                sym.Type != SymbolType.Procedure)
                return sym;
            if ((sym.Address!.ToLinear() & 1) == 0)
                return sym;
            if (archThumb is null)
            {
                var cfgSvc = loader.Services.RequireService<IConfigurationService>();
                this.archThumb = cfgSvc.GetArchitecture("arm-thumb")!;
            }
            var addrNew = sym.Address - 1;
            var symNew = ImageSymbol.Create(
                sym.Type,
                archThumb,
                addrNew,
                sym.Name,
                sym.DataType,
                !sym.NoDecompile);
            symNew.ProcessorState = sym.ProcessorState?.WithArchitecture(archThumb);
            return symNew;
        }

        public override (Address?, ElfSymbol?) RelocateEntry(RelocationContext ctx, ElfRelocation rela, ElfSymbol symbol)
        {
            /*
            S (when used on its own) is the address of the symbol
            A is the addend for the relocation.
            P is the address of the place being relocated (derived from r_offset).
            * Pa is the adjusted address of the place being reloc
            ated, defined as (P & 0xFFFFFFFC). 
            * T is 1 if the target symbol S has type STT_FUNC and the symbol addresses a Thumb instruction; it is 0 
            otherwise.
            * B(S) is the addressing origin of the output segment defining the symbol S. The origin is not required to be 
            the base address of the segment. This value must always be word-aligned.

            * GOT_ORG is the addressing origin of the Global Offset Table (the indirection table for imported data 
            addresses).  This value must always be word-aligned.  See §4.6.1.8, Proxy generating relocations. 
            * GOT(S) is the address of the GOT entry for the symbol S.
            Table 
            0   R_ARM_NONE      Static      Miscellaneous
            1   R_ARM_PC24      Deprecated  ARM             ((S + A) | T) - P
            2   R_ARM_ABS32     Static      Data            ((S + A) | T)
            3   R_ARM_REL32     Static      Data            ((S + A) | T) – P
            4   R_ARM_LDR_PC_G0 Static      ARM             S + A – P
            5   R_ARM_ABS16     Static      Data            S + A
            6   R_ARM_ABS12     Static      ARM             S + A
            7   R_ARM_THM_ABS5  Static      Thumb16         S + A
            8   R_ARM_ABS8      Static      Data            S + A
            9   R_ARM_SBREL32   Static      Data            ((S + A) | T) – B(S)
            20  R_ARM_COPY                                  S
            21  R_ARM_GLOB_DAT Dynamic      Data            (S + A) | T 
            22  R_ARM_JUMP_SLOT Dynamic     Data            (S + A) | T 
            23  R_ARM_RELATIVE Dynamic      Data            B(S) + A  [Note: see Table 4-18]
            */
            var addr = ctx.CreateAddress(ctx.P);
            var rt = (Arm32Rt) (rela.Info & 0xFF);
            switch (rt)
            {
            case Arm32Rt.R_ARM_NONE:
                return (addr, null);
            case Arm32Rt.R_ARM_COPY:
                return (addr, null);
            case Arm32Rt.R_ARM_ABS32:
            case Arm32Rt.R_ARM_GLOB_DAT:
            case Arm32Rt.R_ARM_JUMP_SLOT:       // S + A
                // Add sym + rel.a
                ctx.WriteUInt32(addr, ctx.S + ctx.A);
                return (addr, null);
            case Arm32Rt.R_ARM_RELATIVE:
                // From the docs:
                //
                // (S ≠ 0) B(S) resolves to the difference between the address
                // at which the segment defining the symbol S was loaded and
                // the address at which it was linked. 
                // (S = 0) B(S) resolves to the difference between the address
                // at which the segment being relocated was loaded and the
                // address at which it was linked.
                //
                // Reko always loads objects at their specified physical address, 
                // so this relocation is a no-op;
                return (addr, null);
            case Arm32Rt.R_ARM_TLS_TPOFF32: // S + A - tp
                // Allocates a 32 bit TLS slot
                //$REVIEW: the documentation is unreadable, but this is a
                // guess.
                uint tlsSlotOffset = AllocateTlsSlot(ctx.PointerType);
                ctx.WriteUInt32(addr, ctx.S + ctx.A - tlsSlotOffset);
                return (addr, null);
            case Arm32Rt.R_ARM_TLS_DTPMOD32:
                //$REVIEW: this seems to refer to the modules
                // used when creating the binary. My guess is
                // that it wont be necessary for a fruitful
                // decompilation -jkl
                return (addr, null);
            case Arm32Rt.R_ARM_TLS_DTPOFF32:
                //$NYI
                return (addr, null);
            case Arm32Rt.R_ARM_CALL:
            case Arm32Rt.R_ARM_PC24:
            case Arm32Rt.R_ARM_JUMP24:
            {
                if ((symbol.Value & 0b11) != 0)
                {
                    var secName = ctx.ReferringSection?.Name ?? "<null>";
                    ctx.Warn(addr, $"Section {secName}: unsupported interworking call (ARM -> Thumb)");
                    return (addr, null);
                }
                if (!ctx.TryReadUInt32(addr, out uint uInstr))
                    return (addr, null);
                var offset = uInstr;
                offset = (offset & 0x00ffffff) << 2;
                if ((offset & 0x02000000) != 0)
                    offset -= 0x04000000;

                offset += (uint) symbol.Value - addr.ToUInt32() + (uint)ctx.B;

#if NOT_YET
                /*
                 * Route through a PLT entry if 'offset' exceeds the
                 * supported range. Note that 'offset + loc + 8'
                 * contains the absolute jump target, i.e.,
                 * @sym + addend, corrected for the +8 PC bias.
                 */
                if (IS_ENABLED(CONFIG_ARM_MODULE_PLTS) &&
                    (offset <= (s32) 0xfe000000 ||
                     offset >= (s32) 0x02000000))
                    offset = get_module_plt(module, loc,
                                offset + loc + 8)
                         - loc - 8;
#endif

                if ((int) offset <= -0x02000000 || (int) offset >= 0x02000000)
                {
                    var secName = ctx.ReferringSection?.Name ?? "<null>";
                    ctx.Warn(addr, $"section {secName} relocation at {addr} out of range.");
                    return (addr, null);
                }

                offset >>= 2;
                offset &= 0x00ffffff;

                uInstr &= 0xFF000000;
                uInstr |= offset;

                ctx.WriteUInt32(addr, uInstr);

                return (addr, null);
            }
            case Arm32Rt.R_ARM_MOVW_ABS_NC:
            case Arm32Rt.R_ARM_MOVT_ABS:
            {
                //var instr = program.CreateDisassembler(program.Architecture, addr).First();
                if (!ctx.TryReadUInt32(addr, out uint uInstr))
                    return (addr, null);
                var offset = uInstr;
                offset = ((offset & 0xf0000) >> 4) | (offset & 0xfff);
                offset = (offset ^ 0x8000) - 0x8000;

                offset += (uint) symbol.Value;
                if (rt == Arm32Rt.R_ARM_MOVT_ABS)
                    offset >>= 16;

                var tmp = uInstr & 0xfff0f000;
                tmp |= ((offset & 0xf000) << 4) |
                    (offset & 0x0fff);

                ctx.WriteUInt32(addr, uInstr);
                return (addr, null);
            }
            default:
                ctx.Warn(addr, $"AArch32 relocation type {rt} is not implemented yet.");
                return (null, null);
            }
        }

        private uint AllocateTlsSlot(PrimitiveType dtTlsSlot)
        {
            var tlsSlotOffset = this.currentTlsSlotOffset;
            this.currentTlsSlotOffset += (uint) dtTlsSlot.Size;
            return tlsSlotOffset;
        }

        public override string RelocationTypeToString(uint type)
        {
            return ((Arm32Rt)type).ToString();
        }
    }

    // http://infocenter.arm.com/help/topic/com.arm.doc.ihi0044f/IHI0044F_aaelf.pdf
    public enum Arm32Rt
    {
        R_ARM_NONE = 0,
        R_ARM_PC24 = 1,
        R_ARM_ABS32 = 2,
        R_ARM_REL32 = 3,
        R_ARM_LDR_PC_G0 = 4,
        R_ARM_ABS16 = 5,
        R_ARM_ABS12 = 6,
        R_ARM_THM_ABS5 = 7,
        R_ARM_ABS8 = 8,
        R_ARM_SBREL32 = 9,

        R_ARM_TLS_DTPMOD32 = 17,
        R_ARM_TLS_DTPOFF32 = 18,
        R_ARM_TLS_TPOFF32 = 19,
        R_ARM_COPY = 20,
        R_ARM_GLOB_DAT = 21,
        R_ARM_JUMP_SLOT = 22,
        R_ARM_RELATIVE = 23,

        R_ARM_CALL = 28,
        R_ARM_JUMP24 = 29,

        R_ARM_MOVW_ABS_NC = 43,
        R_ARM_MOVT_ABS = 44,
    }
}
