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
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netDiscographer.core.dynamicQueryCore
{
    /// <summary>
    /// Logical Operator Types
    /// </summary>
    [Serializable]
    public enum logicOperators : ushort
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        none = 0,

        /// <summary>
        /// AND Gate
        /// </summary>
        and = 1,

        /// <summary>
        /// OR Gate
        /// </summary>
        or = 2,

        /// <summary>
        /// Order of Operations
        /// </summary>
        parenthesis = 3
    }
}