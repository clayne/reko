#region License
/* 
 * Copyright (C) 1999-2020 John Källén.
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
using Reko.Core;
using Reko.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reko.UnitTests.Core.Output
{
    [TestFixture]
    public class SingleFilePolicyTests
    {
        private Program program;

        [SetUp]
        public void Setup()
        {
            var segs = new SegmentMap(Address.Ptr32(0x00100000));
            var platform = new Mocks.FakePlatform(null, new Mocks.FakeArchitecture());
            this.program = new Program(segs, platform.Architecture, platform)
            {
                Name = "myprogram.exe"
            };
        }

        private void Given_Executable(string name, uint uAddr, uint uSize)
        {
            var addr = Address.Ptr32(uAddr);
            var seg = new ImageSegment(
                name,
                new MemoryArea(addr, new byte[uSize]),
                AccessMode.Execute);
            program.SegmentMap.AddSegment(seg);
        }

        private void Given_Item(uint uAddr, uint size)
        {
            var item = new ImageMapItem(size)
            {
                Address = Address.Ptr32(uAddr),
            };
        }

        [Test]
        public void Singlefp_Segment()
        {
            Given_Executable(".text", 0x0010_0000, 0x400);
            Given_Item(0x0010_0010, 0x10);
            Given_Item(0x0010_0020, 0x18);

            var sfp = new SingleFilePolicy(program);
            var placements = sfp.GetItemPlacements(".asm").ToList();
            Assert.AreEqual(1, placements.Count);
            Assert.AreEqual("@@@", placements[0].Key);
            Assert.AreEqual(1, placements[0].Value.Count);
        }
    }
}