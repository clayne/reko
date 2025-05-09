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

using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Reko.Core.Serialization
{
    /// <summary>
    /// Abstract base class for serialized procedures.
    /// </summary>
    public abstract class ProcedureBase_v1 
    {
        /// <summary>
        /// The default value for the Ordinal property, indicating that
        /// there is no ordinal number for the procedure.
        /// </summary>
        public const int NoOrdinal = -1;

        /// <summary>
        /// The name of a procedure.
        /// </summary>
        [XmlAttribute("name")]
        public string? Name;

        /// <summary>
        /// Ordinal of a procedure -- if it makes sense
        /// </summary>
        [XmlAttribute("ordinal")]
        [DefaultValue(NoOrdinal)]
        public int Ordinal = NoOrdinal;

        /// <summary>
        /// Procedure signature. If non-null, the user has specified a
        /// signature. If null, the signature is unknown.
        /// </summary>
        [XmlElement("signature")]
        public SerializedSignature? Signature;

        /// <summary>
        /// Procedure characteristics. If non-null, the user has specified
        /// one or more procedure characteristic.
        /// </summary>
        [XmlElement("characteristics")]
        public ProcedureCharacteristics? Characteristics;
    }

    /// <summary>
    /// Serialized user procedure.
    /// </summary>
    public class Procedure_v1 : ProcedureBase_v1
    {
        /// <summary>
        /// Address of the procedure.
        /// </summary>
        [XmlElement("address")]
        public string? Address;

        /// <summary>
        /// Indicates whether the procedure body is to be decompiled 
        /// or not. If false, it is recommended that the <see cref="ProcedureBase_v1.Signature"/>
        /// property be set.
        /// </summary>
        [XmlElement("decompile")]
        [DefaultValue(true)]
        public bool Decompile = true;

        /// <summary>
        /// The registers in Assume are assumed to have specific values. These
        /// are considered oracular and will override anything Reko has deduced
        /// through analyses.
        /// </summary>
        [XmlElement("assume")]
        public RegisterValue_v2[]? Assume;

        /// <summary>
        /// The signature of the function as specified by the user. It is
        /// written in C syntax, with the [[reko::arg(register,{name})]] 
        /// extension for specifying values passed or returned in parameters.
        /// </summary>
        [XmlElement("CSignature")]
        public string? CSignature;

        /// <summary>
        /// Project-relative path into which this procedure should be written.
        /// If no path is specified, the procedure is written into a default
        /// output file.
        /// </summary>
        [XmlElement("OutputFile")]
        public string? OutputFile;
    }

    /// <summary>
    /// Serialized representation of a register value.
    /// </summary>
    public class RegisterValue_v2
    {
        /// <summary>
        /// Optional address; if not specified, use parent context
        /// (e.g. address of user-provided procedure)
        /// </summary>
        [XmlAttribute("addr")]
        public string? Address;

        /// <summary>
        /// Register name.
        /// </summary>
        [XmlAttribute("reg")]
        public string? Register;

        /// <summary>
        /// Register value, represented as a hexadecimal string.
        /// </summary>
        [XmlAttribute("value")]
        public string? Value;
    }
}
