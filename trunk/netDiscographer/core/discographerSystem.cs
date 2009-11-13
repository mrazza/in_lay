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

        #region Properties
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
        #endregion
    }
}
