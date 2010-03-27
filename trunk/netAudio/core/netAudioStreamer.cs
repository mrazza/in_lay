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
using netAudio.core.events;

namespace netAudio.core
{
    /// <summary>
    /// Base netAudio class for all streaming interactions
    /// </summary>
    public abstract class netAudioStreamer : IDisposable
    {
        #region Events
        /// <summary>
        /// Called when the stream state changes
        /// </summary>
        public abstract event EventHandler<stateChangedEventArgs> eStateChanged;

        /// <summary>
        /// Called when the stream position changes
        /// </summary>
        public abstract event EventHandler<positionChangedEventArgs> ePositionChanged;
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

        /// <summary>
        /// Output bitrate
        /// </summary>
        public abstract int iBitRate
        {
            get;
            set;
        }

        /// <summary>
        /// Number of channels to transcode
        /// </summary>
        public abstract int iChannels
        {
            get;
            set;
        }

        /// <summary>
        /// Transcoding sampling rate
        /// </summary>
        public abstract int iSampleRate
        {
            get;
            set;
        }

        /// <summary>
        /// IP we're streaming on
        /// </summary>
        public abstract string sNetworkAddr
        {
            get;
            set;
        }

        /// <summary>
        /// Port we're streaming on
        /// </summary>
        public abstract int iPort
        {
            get;
            set;
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~netAudioStreamer()
        {
            Dispose();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts streaming the current media asset
        /// </summary>
        public abstract void startStream();

        /// <summary>
        /// Pauses the stream
        /// </summary>
        public abstract void pauseStream();

        /// <summary>
        /// Stops the stream
        /// </summary>
        public abstract void stopStream();
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
        #endregion
}
}
