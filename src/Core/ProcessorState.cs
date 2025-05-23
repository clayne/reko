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

using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Memory;
using Reko.Core.Operators;
using Reko.Core.Types;
using System;
using System.Collections.Generic;

namespace Reko.Core
{
    /// <summary>
    /// ProcessorState simulates the state of the processor and a part of the
    /// stack during scanning.
    /// </summary>
    public abstract class ProcessorState : EvaluationContext
    {
        private readonly Dictionary<RegisterStorage, Expression> linearDerived;
        private readonly SortedList<int, Expression> stackState;

#nullable disable
        /// <summary>
        /// Initializes a <see cref="ProcessorState"/> instance.
        /// </summary>
        public ProcessorState()
        {
            this.linearDerived = new Dictionary<RegisterStorage, Expression>();
            this.stackState = new SortedList<int, Expression>();
        }

        /// <summary>
        /// Initializes a <see cref="ProcessorState"/> instance from another.
        /// </summary>
        public ProcessorState(ProcessorState orig)
        {
            this.linearDerived = new Dictionary<RegisterStorage, Expression>(orig.linearDerived);
            this.stackState = new SortedList<int, Expression>(orig.stackState);
            this.ErrorListener = orig.ErrorListener;
        }
#nullable enable

        /// <summary>
        /// Endianness of the processor state.
        /// </summary>
        public EndianServices Endianness => Architecture.Endianness;

        /// <summary>
        /// Memory granularity, in bits.
        /// </summary>
        public int MemoryGranularity => Architecture.MemoryGranularity;

        /// <summary>
        /// Method to call if an error occurs within the processor state object (such as stack over/underflows).
        /// </summary>
        public Action<string>? ErrorListener { get; set; }

        /// <summary>
        /// Processor architecture.
        /// </summary>
        public abstract IProcessorArchitecture Architecture { get; }

        /// <summary>
        /// Current value of the instruction pointer.
        /// </summary>
        public virtual Address InstructionPointer { get; set; }

        /// <summary>
        /// Create an identical copy of this <see cref="ProcessorState"/> instance.
        /// </summary>
        /// <returns>A copy of the state.</returns>
        public abstract ProcessorState Clone();

        /// <summary>
        /// Create a new <see cref="ProcessorState"/> instance with a new
        /// architecture.
        /// </summary>
        /// <param name="arch">Architecture to inject into the new instance.</param>
        /// <returns>A new processor state with a new architecture.
        /// </returns>
        public virtual ProcessorState WithArchitecture(IProcessorArchitecture arch)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get the value of a register.
        /// </summary>
        /// <param name="r">Register whose value we are reading.</param>
        /// <returns>A <see cref="Constant"/>, or <see cref="InvalidConstant"/>
        /// if no value is defined.
        /// </returns>
        public abstract Constant GetRegister(RegisterStorage r);

        /// <summary>
        /// Sets the value of a register.
        /// </summary>
        /// <param name="r">Register being written.</param>
        /// <param name="v">Value to assign to register.</param>
        public abstract void SetRegister(RegisterStorage r, Constant v);

        /// <summary>
        /// Some registers need to be updated when a procedure is entered.
        /// </summary>
        /// <param name="addr"></param>
        public abstract void OnProcedureEntered(Address addr);

        /// <summary>
        /// Called when a procedure is leaving. This is used to update the
        /// registers when a procedure is left.
        /// </summary>
        /// <param name="procedureSignature">Signature of the function that was left.
        /// </param>
        public abstract void OnProcedureLeft(FunctionType procedureSignature);

        /// <summary>
        /// Captures the the processor's state before calling a procedure.
        /// </summary>
        /// <param name="stackReg">An identifier for the stack register of the processor.</param>
        /// <param name="returnAddressSize">The size of the return address on stack.</param>
        /// <returns>A CallSite object that abstracts the processor state right before the call.</returns>
        public abstract CallSite OnBeforeCall(Identifier stackReg, int returnAddressSize);

        /// <summary>
        /// Perform any adjustments to the processor's state after returning from a procedure call with the
        /// specified signature.
        /// </summary>
        /// <param name="sigCallee">The signature of the called procedure.</param>
        public abstract void OnAfterCall(FunctionType? sigCallee);

        private bool IsStackRegister(Expression ea)
        {
            return
                ea is Identifier id &&
                id.Storage is RegisterStorage reg &&
                reg == Architecture.StackRegister;
        }

        private bool GetStackOffset(Expression ea, out int offset)
        {
            if (IsStackRegister(ea))
            {
                offset = 0;
                return true;
            }

            if (ea is BinaryExpression bin && (bin.Operator.Type == OperatorType.IAdd || bin.Operator.Type == OperatorType.ISub) && IsStackRegister(bin.Left))
            {
                if (bin.Right is Constant cOffset)
                {
                    offset = cOffset.ToInt32();
                    if (bin.Operator.Type == OperatorType.ISub)
                        offset = -offset;
                    return true;
                }
            }
            offset = 0;
            return false;
        }

        /// <inheritdoc/>
        public Expression GetValue(Identifier id)
        {
            if (id.Storage is TemporaryStorage)
                return InvalidConstant.Create(id.DataType);
            if (id.Storage is not RegisterStorage reg)
                return InvalidConstant.Create(id.DataType);
            return GetValue(reg);
        }

        /// <inheritdoc/>
        public Expression GetValue(RegisterStorage reg)
        {
            Expression? exp = GetRegister(reg);
            if (exp is not InvalidConstant)
                return exp;
            if (linearDerived.TryGetValue(reg, out exp))
                return exp;
            return InvalidConstant.Create(reg.DataType);
        }

        /// <inheritdoc/>
        public Expression GetValue(MemoryAccess access, IMemory memory)
        {
            if (access.EffectiveAddress is SegmentedPointer segptr)
            {
                if (GetStackOffset(segptr.Offset, out var segStackOffset))
                {
                    if (stackState.TryGetValue(segStackOffset, out var value) &&
                        value.DataType.BitSize == access.DataType.BitSize)
                        return value;
                }
                return InvalidConstant.Create(access.DataType);
            }
            if (access.EffectiveAddress is Constant constAddr)
            {
                // This can only happen on linear architectures.
                if (constAddr is InvalidConstant)
                    return constAddr;
                var ea = Architecture.MakeAddressFromConstant(constAddr, false)!;
                return GetMemoryValue(ea, access.DataType, memory);
            }
            if (access.EffectiveAddress is Address addr)
            {
                return GetMemoryValue(addr, access.DataType, memory);
            }
            if (GetStackOffset(access.EffectiveAddress, out var stackOffset))
            {
                if (stackState.TryGetValue(stackOffset, out var value))
                {
                    if (value.DataType.BitSize == access.DataType.BitSize)
                        return value;
                }
            }
            return InvalidConstant.Create(access.DataType);
        }

        /// <inheritdoc/>
        public Expression GetMemoryValue(Address addr, DataType dt, IMemory memory)
        {
            if (dt is not PrimitiveType pt)
                return InvalidConstant.Create(dt);
            else if (pt.Domain == Domain.Real && pt.BitSize > 80)
            {
                //$TODO: we can't represent 96- or 128-bit floats quite yet.
                return InvalidConstant.Create(dt);
            }
            else if (pt.BitSize > 64)
            {
                //$TODO: we can't represent integer constants larger than 64 bits yet.
                return InvalidConstant.Create(dt);
            }
            if (!memory.IsWriteable(addr) && Architecture.TryRead(memory, addr, pt, out Constant? c))
                return c;
                
            return InvalidConstant.Create(dt);
        }

        /// <inheritdoc/>
        //$TODO: needs the data type of the value being fetched.
        public Expression GetStackValue(int offset)
        {
            if (stackState.TryGetValue(offset, out var value))
                return value;
            return InvalidConstant.Create(PrimitiveType.Word32);    //$BUG: should be data type of access
        }

        /// <inheritdoc/>
        public Expression GetValue(Application appl)
        {
            return InvalidConstant.Create(appl.DataType);
        }

        /// <inheritdoc/>
        public Expression? GetDefiningExpression(Identifier id)
        {
            return null;
        }

        /// <inheritdoc/>
        public Expression MakeSegmentedAddress(Constant seg, Constant off)
        {
            return Architecture.MakeSegmentedAddress(seg, off);
        }

        /// <inheritdoc/>
        public Constant ReinterpretAsFloat(Constant rawBits)
        {
            return Architecture.ReinterpretAsFloat(rawBits);
        }

        /// <inheritdoc/>
        public void UseExpression(Expression expr)
        {
        }

        /// <inheritdoc/>
        public void RemoveExpressionUse(Expression expr)
        {
        }

        /// <inheritdoc/>
        public void SetValue(Identifier id, Expression value)
        {
            if (id.Storage is TemporaryStorage)
                return;
            if (id.Storage is not RegisterStorage reg)
                return;
            SetValue(reg, value);
        }

        /// <inheritdoc/>
        public Constant SetValue(RegisterStorage reg, Expression value)
        {
            if (value is Address addr && !addr.Selector.HasValue)
            {
                var c = Constant.Create(addr.DataType, addr.ToLinear());
                SetRegister(reg, c);
                linearDerived.Remove(reg);
                return c;
            }
            if (value is Constant constVal)
            {
                SetRegister(reg, constVal);
                linearDerived.Remove(reg);
                return constVal;
            }
            if (value is BinaryExpression binVal)
            {
                if ((binVal.Operator.Type == OperatorType.IAdd || binVal.Operator.Type == OperatorType.ISub) &&
                    binVal.Left is Identifier &&
                    binVal.Right is Constant)
                {
                    var invValue = InvalidConstant.Create(value.DataType);
                    SetRegister(reg, invValue);
                    linearDerived[reg] = binVal;
                    return invValue;
                }
            }
            var invValue2 = InvalidConstant.Create(value.DataType);
            SetRegister(reg, invValue2);
            linearDerived.Remove(reg);
            return invValue2;
        }

        /// <inheritdoc/>
        public void SetValueEa(Expression ea, Expression value)
        {
            if (GetStackOffset(ea, out int stackOffset))
                stackState[stackOffset] = value;
        }

        /// <inheritdoc/>
        public void SetValueEa(Expression basePtr, Expression ea, Expression value)
        {
            if (GetStackOffset(ea, out int stackOffset))
                stackState[stackOffset] = value;
        }

        /// <inheritdoc/>
        public bool IsUsedInPhi(Identifier id)
        {
            return false;
        }
    }

    /// <summary>
    /// This dummy implementation of <see cref="ProcessorState"/> may be used if there is no need to track
    /// the processor's state during scanning.
    /// </summary>
    public class DefaultProcessorState : ProcessorState
    {
        /// <summary>
        /// Creates a default processor state instance.
        /// </summary>
        /// <param name="arch"></param>
        public DefaultProcessorState(IProcessorArchitecture arch)
        {
            this.Architecture = arch;
        }

        /// <inheritdoc/>
        public override IProcessorArchitecture Architecture { get; }

        /// <inheritdoc/>
        public override ProcessorState Clone()
        {
            return new DefaultProcessorState(Architecture);
        }

        /// <inheritdoc/>
        public override Constant GetRegister(RegisterStorage r)
        {
            return InvalidConstant.Create(r.DataType);
        }

        /// <inheritdoc/>
        public override void OnAfterCall(FunctionType? sigCallee)
        {
        }

        /// <inheritdoc/>
        public override CallSite OnBeforeCall(Identifier stackReg, int returnAddressSize)
        {
            return new CallSite(returnAddressSize, 0);
        }

        /// <inheritdoc/>
        public override void OnProcedureEntered(Address _)
        {
        }

        /// <inheritdoc/>
        public override void OnProcedureLeft(FunctionType procedureSignature)
        {
        }

        /// <inheritdoc/>
        public override void SetRegister(RegisterStorage r, Constant v)
        {
        }
    }
}
