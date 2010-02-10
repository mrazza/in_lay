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
    public class speedChangedEventArgs : EventArgs
    {
        #region Memebers
        /// <summary>
        /// New player speed
        /// </summary>
        private float _fSpeed;
        #endregion

        #region Properties
        /// <summary>
        /// New player speed
        /// </summary>
        public float fSpeed
        {
            get
            {
                return _fSpeed;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="fSpeed">Change in player speed</param>
        public speedChangedEventArgs(float fSpeed)
        {
            _fSpeed = fSpeed;
        }
        #endregion
    }
}
