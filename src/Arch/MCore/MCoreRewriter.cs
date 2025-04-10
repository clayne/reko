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
using Reko.Core.Memory;
using Reko.Core.Rtl;
using Reko.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reko.Arch.MCore
{
    public class MCoreRewriter : IEnumerable<RtlInstructionCluster>
    {
        private readonly MCoreArchitecture arch;
        private readonly EndianImageReader rdr;
        private readonly ProcessorState state;
        private readonly IStorageBinder binder;
        private readonly IRewriterHost host;
        private readonly IEnumerator<MCoreInstruction> dasm;
        private readonly List<RtlInstruction> rtls;
        private readonly RtlEmitter m;
        private MCoreInstruction instr;
        private InstrClass iclass;

        public MCoreRewriter(MCoreArchitecture arch, EndianImageReader rdr, ProcessorState state, IStorageBinder binder, IRewriterHost host)
        {
            this.arch = arch;
            this.rdr = rdr;
            this.state = state;
            this.binder = binder;
            this.host = host;
            this.dasm = new MCoreDisassembler(arch, rdr).GetEnumerator();
            this.rtls = new List<RtlInstruction>();
            this.m = new RtlEmitter(rtls);
            this.instr = default!;
        }

        public IEnumerator<RtlInstructionCluster> GetEnumerator()
        {
            while (dasm.MoveNext())
            {
                this.instr = dasm.Current;
                this.iclass = instr.InstructionClass;
                switch (instr.Mnemonic)
                {
                default:
                    host.Warn(instr.Address, "MCore instruction {0} not supported yet.", this.instr);
                    EmitUnitTest();
                    goto case Mnemonic.invalid;
                case Mnemonic.invalid: m.Invalid(); iclass = InstrClass.Invalid; break;
                }
                yield return m.MakeCluster(instr.Address, instr.Length, this.iclass);
                this.rtls.Clear();
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Conditional("DEBUG")]
        private void EmitUnitTest()
        {
            var testGenSvc = arch.Services.GetService<ITestGenerationService>();
            testGenSvc?.ReportMissingRewriter("MCoreRw", instr, instr.Mnemonic.ToString(), rdr, "");
        }
    }
}