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
using System.Collections.Generic;

namespace netDiscographer.core.dynamicQueryCore
{
    /// <summary>
    /// Base search object
    /// </summary>
    public interface ISearchBase
    {
        /// <summary>
        /// Returns the next set of queries
        /// </summary>
        /// <returns>Next set of queries</returns>
        ISearchBase[] getNextQueries();
    }
}
