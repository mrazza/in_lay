/*******************************************************************
 * This file is part of the netAudio library.
 * 
 * netAudio source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netAudio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netAudio.core.events
{
    /// <summary>
    /// Base class for state-change event callbacks
    /// </summary>
    public class stateChangedEventArgs : EventArgs
    {
        #region Memebers
        /// <summary>
        /// New player state
        /// </summary>
        private playerState _pState;
        #endregion

        #region Properties
        /// <summary>
        /// New player state
        /// </summary>
        public playerState pState
        {
            get
            {
                return _pState;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="newState">Change in player state</param>
        public stateChangedEventArgs(playerState newState)
        {
            _pState = newState;
        }
        #endregion
    }
}
