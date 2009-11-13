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
using System.Threading;
using netDiscographer.core.events;

namespace netDiscographer.core
{
    /// <summary>
    /// This class wraps any <see cref="discographerDatabase"/> implementation and provides event and threading based support
    /// </summary>
    /// <remarks>This class does not add any database-related functionality. You can use <see cref="discographerDatabase"/> for the same purpose if you don't need event and threading support.</remarks>
    public class discographerSystem
    {
        #region Members
        /// <summary>
        /// Discographer Database we are wrapping
        /// </summary>
        private discographerDatabase _dDatabase;

        /// <summary>
        /// Occurs when [search complete].
        /// </summary>
        private event EventHandler<onSearchCompleteEventArgs> _eSearchComplete;
        #endregion

        #region Properties
        /// <summary>
        /// Occurs when [search complete].
        /// </summary>
        public event EventHandler<onSearchCompleteEventArgs> eSearchComplete
        {
            add
            {
                _eSearchComplete += value;
            }
            remove
            {
                _eSearchComplete -= value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="discographerSystem"/> class.
        /// </summary>
        /// <param name="dDatabase">The database we are wrapping.</param>
        public discographerSystem(discographerDatabase dDatabase)
        {
            if (_dDatabase == null)
                throw new ArgumentNullException("dDatabase can't be null when creating a new instance of the discographerSystem.");

            _dDatabase = dDatabase;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Searches the database
        /// </summary>
        /// <param name="sRequest">Search request to process</param>
        /// <returns>Entries meeting search requirements</returns>
        public mediaEntry[] searchDatabase(searchRequest sRequest)
        {
            return searchDatabase(sRequest, false);
        }

        /// <summary>
        /// Searches the database
        /// </summary>
        /// <param name="sRequest">Search request to process</param>
        /// <param name="bFireEvent">If false, this function will not trigger any search-related events</param>
        /// <returns>Entries meeting search requirements</returns>
        public mediaEntry[] searchDatabase(searchRequest sRequest, bool bFireEvent)
        {
            DateTime dStart = DateTime.Now;
            mediaEntry[] mRet = _dDatabase.searchDatabase(sRequest);

            if (bFireEvent)
                invokeOnSearchComplete(new onSearchCompleteEventArgs(sRequest, mRet, DateTime.Now - dStart));

            return mRet;
        }

        /// <summary>
        /// Searches the database asyncronously.
        /// </summary>
        /// <param name="sRequest">Search request to process</param>
        public void asyncSearchDatabase(searchRequest sRequest)
        {
            DateTime dStart = DateTime.Now;

            ThreadStart tStartupInfo = new ThreadStart(() =>
                {
                    invokeOnSearchComplete(new onSearchCompleteEventArgs(sRequest, _dDatabase.searchDatabase(sRequest), DateTime.Now - dStart));
                });

            Thread tNewThread = new Thread(tStartupInfo);
            tNewThread.Start();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Invokes the on search complete event.
        /// </summary>
        /// <param name="eArgs">The <see cref="netDiscographer.core.events.onSearchCompleteEventArgs"/> instance containing the event data.</param>
        private void invokeOnSearchComplete(onSearchCompleteEventArgs eArgs)
        {
            _eSearchComplete.Invoke(this, eArgs);
        }
        #endregion
    }
}
