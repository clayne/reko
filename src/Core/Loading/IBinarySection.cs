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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reko.Core.Loading;

/// <summary>
/// Represents a segment of the executable file.
/// </summary>
/// <remarks>
/// Some file formats distinguish between segments and sections
/// (e.g. ELF and MachO) while others conflate the two
/// concepts (e.g. PE). In general, sections are smaller, or
/// contained within segments.
/// </remarks>

public interface IBinarySection
{
    int Index { get; }
    string Name { get; }
    ulong Size { get; }

    Address VirtualAddress { get; }

    ulong FileOffset { get; }

    ulong Alignment { get; }

    ulong Flags { get; }

    AccessMode AccessMode { get; }
}