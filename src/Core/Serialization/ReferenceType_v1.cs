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

using System.Text;

namespace Reko.Core.Serialization
{
    /// <summary>
    /// Serialization format of a C++ reference type.
    /// </summary>
    public class ReferenceType_v1 : SerializedType
    {
        /// <summary>
        /// The type of the referent. This is the type that is referred to by
        /// the reference.
        /// </summary>
        public SerializedType? Referent;

        /// <summary>
        /// The implementation's size of the reference, in storage units.
        /// </summary>
        public int Size;

        /// <inheritdoc/>
        public override T Accept<T>(ISerializedTypeVisitor<T> visitor)
        {
            return visitor.VisitReference(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ref({0}", Referent);
            WriteQualifier(Qualifier, sb);
            sb.Append(")");
            return sb.ToString();
        }

    }
}
