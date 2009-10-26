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
    /// Base class for volume-change event callbacks
    /// </summary>
    public class volumeChangedEventArgs : EventArgs
    {
        #region Memebers
        /// <summary>
        /// New player volume
        /// </summary>
        private int _iVolume;

        /// <summary>
        /// New player muted status
        /// </summary>
        private bool _bMuted;
        #endregion

        #region Properties
        /// <summary>
        /// New player volume
        /// </summary>
        public int iVolume
        {
            get
            {
                return _iVolume;
            }
        }

        /// <summary>
        /// New player muted status
        /// </summary>
        public bool bMuted
        {
            get
            {
                return _bMuted;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="newVolume">New player volume</param>
        /// <param name="bMuted">New player muted status</param>
        public volumeChangedEventArgs(int newVolume, bool bMuted)
        {
            _iVolume = newVolume;
            _bMuted = bMuted;
        }
        #endregion
    }
}