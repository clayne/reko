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

namespace Reko.Core
{
    /// <summary>
    /// An exception that happened while processing something at a particular address.
    /// </summary>
    public class AddressCorrelatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressCorrelatedException"/> class.
        /// </summary>
        /// <param name="addr">The address at which the problem occurred.</param>
        /// <param name="message"></param>
        public AddressCorrelatedException(Address addr, string message)
            : base(message)
        {
            this.Address = addr;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressCorrelatedException"/> class.
        /// </summary>
        /// <param name="addr">The address at which the problem occurred.</param>
        /// <param name="innerException">Inner exception.</param>
        /// <param name="message"></param>
        public AddressCorrelatedException(Address addr, Exception innerException, string message)
            : base(message, innerException)
        {
            this.Address = addr;
        }

        /// <summary>
        /// The address at which the problem occurred.
        /// </summary>
        public Address Address { get; }
    }
}
