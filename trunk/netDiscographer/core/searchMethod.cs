/*******************************************************************
 * This file is part of the netDiscographer library.
 * 
 * netDiscographer source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netDiscographer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netDiscographer.core
{
    /// <summary>
    /// Valid Search Methods
    /// </summary>
    [Flags]
    public enum searchMethod : ushort
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        none = 0x0,

        /// <summary>
        /// Normal Search (Generally Expected Search Type)
        /// Matches any field with all params (All params must exist in any of the given fields)
        /// </summary>
        normal = 0x6,

        /// <summary>
        /// Matches any params (default behavior)
        /// </summary>
        matchAnyParam = 0x1,

        /// <summary>
        /// Matches ALL params
        /// </summary>
        matchAllParams = 0x2,

        /// <summary>
        /// Matches any fields (default behavior)
        /// </summary>
        matchAnyField = 0x4,

        /// <summary>
        /// Matches ALL fields
        /// </summary>
        matchAllFields = 0x8,

        /// <summary>
        /// Matches any fields and any params
        /// </summary>
        matchAny = 0x5,

        /// <summary>
        /// Matches all fields and all params
        /// </summary>
        matchAll = 0xA,

        /// <summary>
        /// Last Search Type
        /// </summary>
        last = 0xA
    }
}