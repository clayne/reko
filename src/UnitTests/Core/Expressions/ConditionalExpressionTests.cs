#region License
/* 
 * Copyright (C) 1999-2024 John Källén.
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
using Reko.Core.Expressions;
using Reko.Core.Operators;
using Reko.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reko.UnitTests.Core.Expressions
{
    [TestFixture]
    public class ConditionalExpressionTests
    {
        [Test]
        public void CondExp_Invert()
        {
            var a = Identifier.CreateTemporary("a", PrimitiveType.Word16);
            var b = Identifier.CreateTemporary("b", PrimitiveType.Word16);

            var cexp = new ConditionalExpression(
                PrimitiveType.Word16,
                new BinaryExpression(
                    Operator.Ge,
                    PrimitiveType.Bool,
                    a, b),
                a, b);

            var inv = cexp.Invert();
            Assert.AreEqual("a >= b ? !a : !b", inv.ToString());
        }
    }
}