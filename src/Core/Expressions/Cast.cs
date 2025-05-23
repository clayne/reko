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

using Reko.Core.Operators;
using Reko.Core.Types;
using System.Collections.Generic;

namespace Reko.Core.Expressions
{
    /// <summary>
    /// Models a C-style cast. The specified expression is cast to the data type <see cref="Expression.DataType" />.
    /// </summary>
	public class Cast : AbstractExpression
	{
        /// <summary>
        /// Constructs a cast expression.
        /// </summary>
        /// <param name="dt">Datatype to cast to.</param>
        /// <param name="expr">Expression to cast.</param>
		public Cast(DataType dt, Expression expr) : base(dt)
		{
			this.Expression = expr;
		}

        /// <summary>
        /// Expression to cast.
        /// </summary>
        public Expression Expression { get; }

        /// <inheritdoc/>
        public override IEnumerable<Expression> Children
        {
            get { yield return Expression; }
        }

        /// <inheritdoc/>
        public override T Accept<T, C>(ExpressionVisitor<T, C> v, C context)
        {
            return v.VisitCast(this, context);
        }

        /// <inheritdoc/>
        public override T Accept<T>(ExpressionVisitor<T> v)
        {
            return v.VisitCast(this);
        }

        /// <inheritdoc/>
		public override void Accept(IExpressionVisitor visit)
		{
			visit.VisitCast(this);
		}

        /// <inheritdoc/>
		public override Expression CloneExpression()
		{
			return new Cast(DataType, Expression.CloneExpression());
		}

        /// <inheritdoc/>
        public override Expression Invert()
        { 
            return new UnaryExpression(Operator.Not, PrimitiveType.Bool, this);
        }
    }
}
