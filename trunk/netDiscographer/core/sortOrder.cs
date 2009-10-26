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

namespace netDiscographer.core
{
    /// <summary>
    /// Sort Order
    /// </summary>
    public enum sortOrder : ushort
    {
        /// <summary>
        /// No sort order
        /// </summary>
        none = 0,

        /// <summary>
        /// Ascending order
        /// 1, 2, 3
        /// </summary>
        ascending = 1,

        /// <summary>
        /// Decending order
        /// 9, 8, 7
        /// </summary>
        descending = 2
    }
}