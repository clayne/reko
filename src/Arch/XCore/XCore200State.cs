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
using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Types;

namespace Reko.Arch.XCore
{
    public class XCore200State : ProcessorState
    {
        private readonly XCore200Architecture arch;

        public XCore200State(XCore200Architecture arch)
        {
            this.arch = arch;
        }

        public override IProcessorArchitecture Architecture => arch;

        public override ProcessorState Clone()
        {
            return new XCore200State(this.arch);
        }

        public override Constant GetRegister(RegisterStorage r)
        {
            throw new System.NotImplementedException();
        }

        public override void OnAfterCall(FunctionType? sigCallee)
        {
            throw new System.NotImplementedException();
        }

        public override CallSite OnBeforeCall(Identifier stackReg, int returnAddressSize)
        {
            throw new System.NotImplementedException();
        }

        public override void OnProcedureEntered(Address addr)
        {
            throw new System.NotImplementedException();
        }

        public override void OnProcedureLeft(FunctionType procedureSignature)
        {
            throw new System.NotImplementedException();
        }

        public override void SetRegister(RegisterStorage r, Constant v)
        {
            throw new System.NotImplementedException();
        }
    }
}