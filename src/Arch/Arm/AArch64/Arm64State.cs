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
using System.Collections.Generic;
using System.Linq;
using Reko.Core;
using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Types;

namespace Reko.Arch.Arm.AArch64
{
    public class Arm64State : ProcessorState
    {
        private readonly Arm64Architecture arch;
        private readonly Dictionary<StorageDomain, Constant> values;

        public Arm64State(Arm64Architecture arch)
        {
            this.arch = arch;
            this.values = new Dictionary<StorageDomain, Constant>();
        }

        public Arm64State(Arm64State that)
        {
            this.arch = that.arch;
            this.values = that.values.ToDictionary(k => k.Key, v => v.Value);
        }

        public override IProcessorArchitecture Architecture
        {
            get { return arch; }
        }

        public override ProcessorState Clone()
        {
            return new Arm64State(this.arch);
        }

        public override Constant GetRegister(RegisterStorage r)
        {
            if (!values.TryGetValue(r.Domain, out var value))
                return InvalidConstant.Create(r.DataType);
            if (value is InvalidConstant)
            {
                return InvalidConstant.Create(r.DataType);
            }
            if ((int)r.BitSize > value.DataType.BitSize)
            {
                return InvalidConstant.Create(r.DataType);
            }
            else if ((int)r.BitSize < value.DataType.BitSize)
            {
                return value.Slice(r.DataType, 0);
            }
            return value;
        }

        public override void OnAfterCall(FunctionType? sigCallee)
        {
        }

        public override CallSite OnBeforeCall(Identifier stackReg, int returnAddressSize)
        {
            return new CallSite(0, 0);
        }

        public override void OnProcedureEntered(Address addr)
        {
        }

        public override void OnProcedureLeft(FunctionType procedureSignature)
        {
        }

        public override void SetRegister(RegisterStorage r, Constant v)
        {
            if (v is not InvalidConstant)
            {
                if (!values.TryGetValue(r.Domain, out var oldValue))
                {
                    values[r.Domain] = v;
                }
                else
                {
                    if (v.DataType.BitSize < oldValue.DataType.BitSize)
                    {
                        // Overwriting the low part of oldValue.
                        v = oldValue.DepositBits(v, 0);
                    }
                }
            }
            values[r.Domain] = v;
        }
    }
} 