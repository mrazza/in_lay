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
    /// Search Comparison Operators
    /// </summary>
    [Serializable]
    public enum comparisonOperators : ushort
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        none = 0,

        /// <summary>
        /// A LIKE comparision
        /// </summary>
        like = 1,

        /// <summary>
        /// Equals
        /// </summary>
        equals = 2,

        /// <summary>
        /// Not Equals
        /// </summary>
        notEquals = 3,

        /// <summary>
        /// Greater Than
        /// </summary>
        greaterThan = 4,

        /// <summary>
        /// Less Than
        /// </summary>
        lessThan = 5,

        /// <summary>
        /// Greater Than or Equal To
        /// </summary>
        greaterThanEquals = 6,

        /// <summary>
        /// Less Than or Equal To
        /// </summary>
        lessThanEquals = 7
    }
}