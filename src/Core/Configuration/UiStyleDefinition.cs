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
using System.Configuration;
using System.Linq;
using System.Text;

namespace Reko.Core.Configuration
{
    public class UiStyleDefinition
    {
        public string? Name { get; set; }

        public string? FontName { get; set; }

        public double? FontSize { get; set; }

        public string? ForeColor { get; set; }

        public string? BackColor { get; set; }

        public string? Cursor { get; set; }

        public string? Width { get; set; }

        public string? TextAlign { get; set; }

        public string? PaddingTop { get; set; }

        public string? PaddingLeft { get; set; }

        public string? PaddingBottom { get; set; }

        public string? PaddingRight { get; set; }
    }
}
