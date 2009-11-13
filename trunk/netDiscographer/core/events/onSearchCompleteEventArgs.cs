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

        /// <summary>
        /// The time it took to complete the search
        /// </summary>
        private TimeSpan _tSearchTime;
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

        /// <summary>
        /// The time it took to complete the search
        /// </summary>
        public TimeSpan tSearchTime
        {
            get
            {
                return _tSearchTime;
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
            : this(sRequest, mEntries, TimeSpan.Zero) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="onSearchCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="sRequest">The search request.</param>
        /// <param name="mEntries">The media entries returned.</param>
        /// <param name="tSearchTime">Time it took to execute the search</param>
        public onSearchCompleteEventArgs(searchRequest sRequest, mediaEntry[] mEntries, TimeSpan tSearchTime)
        {
            if (sRequest == null)
                throw new NullReferenceException("sRequest can not be null when creating an instance of onSearchCompleteEventArgs.");

            _sRequest = sRequest;
            _mEntries = mEntries;
            _tSearchTime = tSearchTime;
        }
        #endregion
    }
}
