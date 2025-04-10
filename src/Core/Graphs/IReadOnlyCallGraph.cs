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
using System.IO;

namespace Reko.Core.Graphs
{
    /// <summary>
    /// Read-only view of the <see cref="CallGraph"/> of a <see cref="Program"/>.
    /// </summary>
    public interface IReadOnlyCallGraph
    {
        /// <summary>
        /// Given a procedure, find all the statements that call it.
        /// </summary>
        IEnumerable<Statement> FindCallerStatements(ProcedureBase procedure);

        //$BUG: mutable state. Used in ValuePropagator when 
        // new procedure constants are discovered. Need to
        // change this so that mutations of the call graph
        // are done safely.
        void AddEdge(Statement stmCur, ProcedureBase procCallee);

        //$BUG: mutable state. Only used in SsaTransform
        // when unreachable code is discovered. Need to
        // change this so that mutations of the call graph
        // are done safely.
        void RemoveCaller(Statement stm);
        void Write(TextWriter writer);
    }
}
