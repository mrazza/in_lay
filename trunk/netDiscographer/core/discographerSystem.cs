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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="discographerSystem"/> class.
        /// </summary>
        /// <param name="dDatabase">The database we are wrapping.</param>
        public discographerSystem(discographerDatabase dDatabase)
        {
            if (dDatabase == null)
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
            DateTime dStart = DateTime.Now;
            mediaEntry[] mRet = _dDatabase.searchDatabase(sRequest);

            return mRet;
        }

        /// <summary>
        /// Searches the database asyncronously.
        /// </summary>
        /// <param name="sRequest">Search request to process</param>
        /// <param name="eTriggerTarget">Function/Event Handler to invoke when completed</param>
        public void asyncSearchDatabase(searchRequest sRequest, EventHandler<onSearchCompleteEventArgs> eTriggerTarget)
        {
            DateTime dStart = DateTime.Now;

            ThreadStart tStartupInfo = new ThreadStart(() =>
                {
                    eTriggerTarget.Invoke(this, new onSearchCompleteEventArgs(sRequest, _dDatabase.searchDatabase(sRequest), DateTime.Now - dStart));
                });

            Thread tNewThread = new Thread(tStartupInfo);
            tNewThread.Start();
        }
        #endregion
    }
}
