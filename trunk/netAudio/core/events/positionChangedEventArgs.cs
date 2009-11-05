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
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netAudio.core.events
{
    /// <summary>
    /// Base class for position-change event callbacks
    /// </summary>
    public class positionChangedEventArgs : EventArgs
    {
        #region Memebers
        /// <summary>
        /// New player position (milliseconds)
        /// </summary>
        private long _lPosition;

        /// <summary>
        /// New player position (%)
        /// </summary>
        private float _fPosition;
        #endregion

        #region Properties
        /// <summary>
        /// New player volume (milliseconds)
        /// </summary>
        public long lPosition
        {
            get
            {
                return _lPosition;
            }
        }

                /// <summary>
        /// New player volume
        /// </summary>
        public float fPosition
        {
            get
            {
                return _fPosition;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="lNewPos">New player position (milliseconds)</param>
        /// <param name="fNewPos">New player position (%)</param>
        public positionChangedEventArgs(long lNewPos, float fNewPos)
        {
            _lPosition = lNewPos;
            _fPosition = fNewPos;
        }
        #endregion
    }
}