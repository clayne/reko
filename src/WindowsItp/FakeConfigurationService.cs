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

using Reko.Core;
using Reko.Core.Configuration;
using Reko.Core.Serialization;
using Reko.Gui.Services;
using System;
using System.Collections.Generic;

namespace Reko.WindowsItp
{
    public class FakeConfigurationService : IConfigurationService
    {
        public ICollection<ArchitectureDefinition> GetArchitectures()
        {
            throw new NotImplementedException();
        }

        public ICollection<PlatformDefinition> GetEnvironments()
        {
            throw new NotImplementedException();
        }

        public ICollection<LoaderDefinition> GetImageLoaders()
        {
            throw new NotImplementedException();
        }

        public ICollection<RawFileDefinition> GetRawFiles()
        {
            throw new NotImplementedException();
        }

        public ICollection<SignatureFileDefinition> GetSignatureFiles()
        {
            throw new NotImplementedException();
        }

        public IProcessorArchitecture GetArchitecture(string sArch)
        {
            throw new NotImplementedException();
        }

        public IProcessorArchitecture GetArchitecture(string sArch, string modelName)
        {
            throw new NotImplementedException();
        }

        public IProcessorArchitecture GetArchitecture(string sArch, Dictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

        public PlatformDefinition GetEnvironment(string envName)
        {
            throw new NotImplementedException();
        }

        public LoaderDefinition GetImageLoader(string loaderName)
        {
            throw new NotImplementedException();
        }

        public string GetInstallationRelativePath(params string [] pathComponents)
        {
            throw new NotImplementedException();
        }

        public RawFileDefinition GetRawFile(string rawFileFormat)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Core.Configuration.UiStyleDefinition> GetDefaultPreferences()
        {
            return new Core.Configuration.UiStyleDefinition[] {
                    new UiStyleDefinition { Name = UiStyles.MemoryWindow, FontName="Lucida Console, 9pt"},
                    new UiStyleDefinition { Name = UiStyles.MemoryCode, ForeColor = "#000000", BackColor="#FFC0C0", },
                    new UiStyleDefinition { Name = UiStyles.MemoryHeuristic, ForeColor = "#000000", BackColor="#FFE0E0"},
                    new UiStyleDefinition { Name = UiStyles.MemoryData, ForeColor="#000000", BackColor="#C0C0FF" },

                    new UiStyleDefinition { Name = UiStyles.Disassembler,  FontName="Lucida Console, 9pt" },
                    new UiStyleDefinition { Name = UiStyles.DisassemblerOpcode, ForeColor = "#801010" },

                    new UiStyleDefinition { Name = UiStyles.CodeWindow, FontName = "Lucida Console, 9pt"},
                    new UiStyleDefinition { Name = UiStyles.CodeKeyword, ForeColor="#00C0C0" },
                    new UiStyleDefinition { Name = UiStyles.CodeComment, ForeColor="#00C000" },
                };
        }

        public ICollection<SymbolSourceDefinition> GetSymbolSources()
        {
            return new List<SymbolSourceDefinition>
            {
                new SymbolSourceDefinition { Name = "Bobsym", Description="BOB symbol loader", TypeName="BobSymSource,Bob" },
                new SymbolSourceDefinition { Name = "PDB", Description="PDB", TypeName="PDBSymSource,PDBLoader" }
            };
        }

        public MemoryMap_v1 LoadMemoryMap(string memoryMapFile)
        {
            throw new NotImplementedException();
        }
    }
}