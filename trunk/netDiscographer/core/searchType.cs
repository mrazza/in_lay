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
    /// Valid search types
    /// </summary>
    public enum searchType : ushort
    {
        /// <summary>
        /// No search type set
        /// </summary>
        none = 0,

        /// <summary>
        /// Direct library search
        /// </summary>
        library = 1,

        /// <summary>
        /// Standard playlist search
        /// </summary>
        playlist = 2,
        
        /// <summary>
        /// Dynamic playlist search
        /// </summary>
        dynamic = 3
    }
}
