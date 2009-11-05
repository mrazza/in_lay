/*******************************************************************
 * This file is part of the in_lay Player Shared Library.
 * 
 * in_lay source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * in_lay Player is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using netDiscographer.core;

namespace inlayShared.core
{
    /// <summary>
    /// Database Search Complete Event Arguments
    /// </summary>
    public sealed class databaseSearchCompleteEventArgs : EventArgs
    {
        #region Members
        /// <summary>
        /// Array of returned media entries
        /// </summary>
        private mediaEntry[] _mReturnedEntries;
        #endregion

        #region Properties
        /// <summary>
        /// Array of returned media entries
        /// </summary>
        public mediaEntry[] mReturnedEntries
        {
            get
            {
                return _mReturnedEntries;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="databaseSearchCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="mReturnedEntries">The m returned entries.</param>
        public databaseSearchCompleteEventArgs(mediaEntry[] mReturnedEntries)
        {
            _mReturnedEntries = mReturnedEntries;
        }
        #endregion
    }
}
