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
using netAudio.core.events;

namespace netAudio.core
{
    /// <summary>
    /// Base netAudio class (for all basic interactions)
    /// </summary>
    public abstract class netAudioPlayer : IDisposable
    {
        #region Events
        /// <summary>
        /// Called when the player state changes
        /// </summary>
        public abstract event EventHandler<stateChangedEventArgs> eStateChanged;

        /// <summary>
        /// Called when the player volume changes/the player is muted
        /// </summary>
        public abstract event EventHandler<volumeChangedEventArgs> eVolumeChanged;

        /// <summary>
        /// Called when the player position changes
        /// </summary>
        public abstract event EventHandler<positionChangedEventArgs> ePositionChanged;

        /// <summary>
        /// Called when the player speed changes
        /// </summary>
        public abstract event EventHandler<speedChangedEventArgs> eSpeedChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Position in the track (milliseconds)
        /// </summary>
        public abstract long lPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Position in the track (%)
        /// </summary>
        public abstract float fPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Length of the track (milliseconds)
        /// </summary>
        public abstract long lLength
        {
            get;
        }

        /// <summary>
        /// Current volume (% of actual)
        /// </summary>
        public abstract int iVolume
        {
            get;
            set;
        }

        /// <summary>
        /// Currently muted?
        /// </summary>
        public abstract bool bMute
        {
            get;
            set;
        }

        /// <summary>
        /// Current media playback rate
        /// </summary>
        public abstract float fPlaybackRate
        {
            get;
            set;
        }

        /// <summary>
        /// Current state of the player
        /// </summary>
        public abstract playerState pState
        {
            get;
        }

        /// <summary>
        /// Path to the currently loaded media
        /// </summary>
        public abstract string sMediaPath
        {
            get;
            set;
        }

        /// <summary>
        /// Tracks meta data
        /// </summary>
        public abstract metaData mTrackData
        {
            get;
            set;
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~netAudioPlayer()
        {
            Dispose();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Plays the current media asset
        /// </summary>
        public abstract void playMedia();

        /// <summary>
        /// Plays the media in sPath
        /// </summary>
        /// <param name="sPath">Media to play</param>
        public abstract void playMedia(string sPath);

        /// <summary>
        /// Pauses the current media asset
        /// </summary>
        public abstract void pauseMedia();

        /// <summary>
        /// Stops the current media asset
        /// </summary>
        public abstract void stopMedia();
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}
