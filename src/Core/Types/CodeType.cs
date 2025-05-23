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

using System.Collections.Generic;

namespace Reko.Core.Types
{
    /// <summary>
    /// Denotes a blob of executable code. The targets of gotos and functions 
    /// point to this. The types of BasicBlocks are this.
    /// </summary>
    public class CodeType : DataType
    {
        /// <summary>
        /// Constructs an instance of <see cref="CodeType"/>.
        /// </summary>
        public CodeType() : base(Domain.Function)
        {
        }

        /// <inheritdoc/>
        public override int Size { get; set; }

        /// <inheritdoc/>
        public override void Accept(IDataTypeVisitor v)
        {
            v.VisitCode(this);
        }

        /// <inheritdoc/>
        public override T Accept<T>(IDataTypeVisitor<T> v)
        {
            return v.VisitCode(this);
        }

        /// <inheritdoc/>
        public override DataType Clone(IDictionary<DataType, DataType>? clonedTypes)
        {
            return new CodeType
            {
                Size = this.Size,
                Qualifier = this.Qualifier
            };
        }
    }
}
