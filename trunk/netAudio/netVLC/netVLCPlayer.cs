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
using netAudio.core;
using netAudio.core.events;
using netAudio.netVLC.core;

namespace netAudio.netVLC
{
    /// <summary>
    /// netVLC main class
    /// Interacts with the libVLC helper classes
    /// </summary>
    public sealed class netVLCPlayer : netAudioPlayer, IVLCCoreUser
    {
        #region Members
        /// <summary>
        /// VLC Core
        /// </summary>
        private vlcCore _vCore;

        /// <summary>
        /// VLC Player
        /// </summary>
        private vlcPlayer _vPlayer;

        /// <summary>
        /// VLC Event Manager
        /// </summary>
        private vlcEventManager _vEventMan;

        /// <summary>
        /// Meta Data Reader Object
        /// </summary>
        private metaDataManager _mReader;
        #endregion

        #region Properties
        #region Events
        /// <summary>
        /// Called when the player state changes
        /// </summary>
        public override event EventHandler<stateChangedEventArgs> eStateChanged
        {
            add
            {
                _vEventMan.eStateChanged += value;
            }
            remove
            {
                _vEventMan.eStateChanged -= value;
            }
        }

        /// <summary>
        /// Called when the player volume changes/the player is muted
        /// </summary>
        public override event EventHandler<volumeChangedEventArgs> eVolumeChanged
        {
            add
            {
                _vEventMan.eVolumeChanged += value;
            }
            remove
            {
                _vEventMan.eVolumeChanged -= value;
            }
        }

        /// <summary>
        /// Called when the player position changes
        /// </summary>
        public override event EventHandler<positionChangedEventArgs> ePositionChanged
        {
            add
            {
                _vEventMan.ePositionChanged += value;
            }
            remove
            {
                _vEventMan.ePositionChanged -= value;
            }
        }

        /// <summary>
        /// Called when the player speed changes
        /// </summary>
        public override event EventHandler<speedChangedEventArgs> eSpeedChanged
        {
            add
            {
                _vEventMan.eSpeedChanged += value;
            }
            remove
            {
                _vEventMan.eSpeedChanged -= value;
            }
        }
        #endregion

        #region netAudioPlayer
        /// <summary>
        /// Position in the track (milliseconds)
        /// </summary>
        public override long lPosition
        {
            get
            {
                return _vPlayer.lPosition;
            }
            set
            {
                _vPlayer.lPosition = lPosition;
            }
        }

        /// <summary>
        /// Position in the track (%)
        /// </summary>
        public override float fPosition
        {
            get
            {
                return _vPlayer.fPosition;
            }
            set
            {
                _vPlayer.fPosition = value;
            }
        }

        /// <summary>
        /// Length of the track (milliseconds)
        /// </summary>
        public override long lLength
        {
            get
            {
                long len = (long)_mReader.mData.tLength.TotalMilliseconds;

                if (len == 0)
                    return _vPlayer.lLength;

                return len;
            }
        }

        /// <summary>
        /// Current volume (% of actual)
        /// </summary>
        public override int iVolume
        {
            get
            {
                return _vCore.iVolume;
            }
            set
            {
                _vCore.iVolume = value;
                _vEventMan.invokeVolumeChanged(new volumeChangedEventArgs(value, bMute));
            }
        }

        /// <summary>
        /// Currently muted?
        /// </summary>
        public override bool bMute
        {
            get
            {
                return _vCore.bMute;
            }
            set
            {
                _vCore.bMute = value;
                _vEventMan.invokeVolumeChanged(new volumeChangedEventArgs(iVolume, value));
            }
        }

        /// <summary>
        /// Current media playback rate
        /// </summary>
        public override float fPlaybackRate
        {
            get
            {
                return _vPlayer.fPlaybackRate;
            }
            set
            {
                _vPlayer.fPlaybackRate = value;
                _vEventMan.invokeSpeedChanged(new speedChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Current state of the player
        /// </summary>
        public override playerState pState
        {
            get
            {
                return vlcPlayer.vlcToNetAudioPlayerState(_vPlayer.pState);
            }
        }

        /// <summary>
        /// Path to the currently loaded media
        /// </summary>
        public override string sMediaPath
        {
            get
            {
                return _vPlayer.vMedia.sPath;
            }
            set
            {
                stopMedia(); //Make sure we're not playing

                // Dispose if we need to
                if (_vPlayer.vMedia != null)
                    _vPlayer.vMedia.Dispose();

                _vPlayer.vMedia = new vlcMedia(_vCore, value); //Ser the media
                _mReader.sFile = sMediaPath;
            }
        }

        /// <summary>
        /// Current Track Meta Data
        /// </summary>
        public override metaData mTrackData
        {
            get
            {
                if (_vPlayer.vMedia == null)
                    return null;

                if (_mReader.mData.bContainsData)
                    return _mReader.mData;

                return _vPlayer.vMedia.getMetaData();
            }
            set
            {
                _mReader.mData = value;
            }
        }
        #endregion

        /// <summary>
        /// vlcCore
        /// </summary>
        public vlcCore vCore
        {
            get
            {
                return _vCore;
            }
        }

        /// <summary>
        /// vlcPlayer
        /// </summary>
        public vlcPlayer vPlayer
        {
            get
            {
                return _vPlayer;
            }
        }

        /// <summary>
        /// vlcEventManager
        /// </summary>
        public vlcEventManager vEventMan
        {
            get
            {
                return _vEventMan;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public netVLCPlayer()
            : this(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory + "plugins\\", new string[] {"-I", "dummy", "--ignore-config"}) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sArgs">Arguments</param>
        public netVLCPlayer(string[] sArgs)
            : this(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory + "plugins\\", sArgs) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sPluginPath">Plugin path</param>
        /// <param name="sArgs">Arguments</param>
        public netVLCPlayer(string sPluginPath, string[] sArgs)
            : this(AppDomain.CurrentDomain.BaseDirectory, sPluginPath, sArgs) { }

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="sVLCPath">Path to VLC</param>
        /// <param name="sPluginPath">Path to plugins</param>
        /// <param name="sArgs">Arguments</param>
        public netVLCPlayer(string sVLCPath, string sPluginPath, string[] sArgs)
        {
            initVLC(sVLCPath, sPluginPath, sArgs);
        }
        #endregion

        #region Public Memebers
        /// <summary>
        /// Plays the current media asset
        /// </summary>
        public override void playMedia()
        {
            _vPlayer.playMedia();
        }

        /// <summary>
        /// Plays the media in sPath
        /// </summary>
        /// <param name="sPath">Media to play</param>
        public override void playMedia(string sPath)
        {
            sMediaPath = sPath;
            playMedia();
        }

        /// <summary>
        /// Pauses the current media asset
        /// </summary>
        public override void pauseMedia()
        {
            _vPlayer.pauseMedia();
        }

        /// <summary>
        /// Stops the current media asset
        /// </summary>
        public override void stopMedia()
        {
            _vPlayer.stopMedia();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Creates the VLC objects
        /// </summary>
        /// <param name="sVLCPath">Path to VLC</param>
        /// <param name="sPluginPath">Path to plugins</param>
        /// <param name="sArgs">Arguments</param>
        private void initVLC(string sVLCPath, string sPluginPath, string[] sArgs)
        {
            // Build the vlcCore arguments
            string[] sArguments = new string[2 + sArgs.Length];
            sArguments[0] = "\"" + sVLCPath + "\"";
            for (int iLoop = 0; iLoop < sArgs.Length; iLoop++)
                sArguments[1 + iLoop] = sArgs[iLoop];
            sArguments[sArguments.Length - 1] = "--plugin-path=\"" + sPluginPath + "\"";

            _vCore = new vlcCore(sArguments);
            _vPlayer = new vlcPlayer(_vCore);
            _vEventMan = new vlcEventManager(this);
            _mReader = new metaDataManager(null);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public override void Dispose()
        {
            _vEventMan.clearEventHandlers();
            stopMedia();
            _vEventMan.Dispose();

            // Dispose if we need to
            if (_vPlayer.vMedia != null)
                _vPlayer.vMedia.Dispose();

            _vPlayer.Dispose();
            _vCore.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
