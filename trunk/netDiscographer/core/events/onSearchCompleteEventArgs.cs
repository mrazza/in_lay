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

namespace netDiscographer.core.events
{
    /// <summary>
    /// Event arguments for the onSearchComplete Event
    /// </summary>
    public sealed class onSearchCompleteEventArgs : EventArgs
    {
        #region Members
        /// <summary>
        /// Search request that was exectuted
        /// </summary>
        private searchRequest _sRequest;

        /// <summary>
        /// Data returned from the search
        /// </summary>
        private mediaEntry[] _mEntries;
        #endregion

        #region Properties
        /// <summary>
        /// Search request that was exectuted
        /// </summary>
        public searchRequest sRequest
        {
            get
            {
                return _sRequest;
            }
        }

        /// <summary>
        /// Data returned
        /// </summary>
        public mediaEntry[] mEntries
        {
            get
            {
                return _mEntries;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="onSearchCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="sRequest">The search request.</param>
        /// <param name="mEntries">The media entries returned.</param>
        public onSearchCompleteEventArgs(searchRequest sRequest, mediaEntry[] mEntries)
        {
            if (sRequest == null)
                throw new NullReferenceException("sRequest can not be null when creating an instance of onSearchCompleteEventArgs.");

            _sRequest = sRequest;
            _mEntries = mEntries;
        }
        #endregion
    }
}
