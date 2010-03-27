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
using netAudio.core;
using netAudio.core.events;
using netAudio.netVLC.core;

namespace netAudio.netVLC
{
    /// <summary>
    /// netVLC Streaming Main Class
    /// Interacts with the libVLC helper classes
    /// </summary>
    public sealed class netVLCStreamer : netAudioStreamer, IVLCCoreUser
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

        /// <summary>
        /// Bit rate
        /// </summary>
        private int _iBitRate;

        /// <summary>
        /// Number of channels
        /// </summary>
        private int _iChannels;

        /// <summary>
        /// Sample Rate
        /// </summary>
        private int _iSampleRate;
        
        /// <summary>
        /// Network IP to stream on
        /// </summary>
        private string _sNetworkAddr;

        /// <summary>
        /// Port to stream on
        /// </summary>
        private int _iPort;

        /// <summary>
        /// Has the option been applied?
        /// </summary>
        private bool _bOptionApplied;

        /// <summary>
        /// Has the config changed?
        /// </summary>
        private bool _bConfigChanged;
        #endregion

        #region Properties
        #region Events
        /// <summary>
        /// Called when the stream state changes
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
        /// Called when the stream position changes
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
        #endregion

        #region netAudioStreamer
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
                stopStream(); //Make sure we're not streaming

                // Dispose if we need to
                if (_vPlayer.vMedia != null)
                    _vPlayer.vMedia.Dispose();

                vlcMedia newMedia = new vlcMedia(_vCore, value); //Set the media
                _bOptionApplied = false;
                _vPlayer.vMedia = newMedia;
                _mReader.sFile = sMediaPath;
            }
        }

        /// <summary>
        /// Tracks meta data
        /// </summary>
        /// <value>Track meta data</value>
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

        /// <summary>
        /// Output bitrate
        /// </summary>
        public override int iBitRate
        {
            get
            {
                return _iBitRate;
            }
            set
            {
                _iBitRate = value;
                _bConfigChanged = true;
            }
        }

        /// <summary>
        /// Number of channels to transcode
        /// </summary>
        public override int iChannels
        {
            get
            {
                return _iChannels;
            }
            set
            {
                _iChannels = value;
                _bConfigChanged = true;
            }
        }

        /// <summary>
        /// Transcoding sampling rate
        /// </summary>
        public override int iSampleRate
        {
            get
            {
                return _iSampleRate;
            }
            set
            {
                _iSampleRate = value;
                _bConfigChanged = true;
            }
        }

        /// <summary>
        /// IP we're streaming on
        /// </summary>
        public override string sNetworkAddr
        {
            get
            {
                return _sNetworkAddr;
            }
            set
            {
                _sNetworkAddr = value;
                _bConfigChanged = true;
            }
        }

        /// <summary>
        /// Port we're streaming on
        /// </summary>
        public override int iPort
        {
            get
            {
                return _iPort;
            }
            set
            {
                _iPort = value;
                _bConfigChanged = true;
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
        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public netVLCStreamer()
            : this(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory + "plugins\\", new string[] { "-I", "dummy", "--ignore-config", "--verbose=0", "--no-media-library", "--no-video", "--vout", "dummy", "--control", "" }) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sArgs">Arguments</param>
        public netVLCStreamer(string[] sArgs)
            : this(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory + "plugins\\", sArgs) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sPluginPath">Plugin path</param>
        /// <param name="sArgs">Arguments</param>
        public netVLCStreamer(string sPluginPath, string[] sArgs)
            : this(AppDomain.CurrentDomain.BaseDirectory, sPluginPath, sArgs) { }

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="sVLCPath">Path to VLC</param>
        /// <param name="sPluginPath">Path to plugins</param>
        /// <param name="sArgs">Arguments</param>
        public netVLCStreamer(string sVLCPath, string sPluginPath, string[] sArgs)
        {
            _iBitRate = 128;
            _iChannels = 2;
            _iSampleRate = 44100;
            _iPort = 8080;
            _bConfigChanged = false;
            _bOptionApplied = false;
            initVLC(sVLCPath, sPluginPath, sArgs);
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Starts streaming the current media asset
        /// </summary>
        public override void startStream()
        {
            applyOption();
            _vPlayer.playMedia();
        }

        /// <summary>
        /// Pauses the stream
        /// </summary>
        public override void pauseStream()
        {
            _vPlayer.pauseMedia();
        }

        /// <summary>
        /// Stops the stream
        /// </summary>
        public override void stopStream()
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
            _vEventMan = new vlcEventManager(this._vPlayer);
            _mReader = new metaDataManager(null);
        }

        /// <summary>
        /// Apply encoding options
        /// </summary>
        private void applyOption()
        {
            if (_bOptionApplied && _bConfigChanged)
                sMediaPath = sMediaPath;

            _vPlayer.vMedia.addOption(":sout=#transcode{acodec=mp3,ab=" + _iBitRate + ",channels=" + _iChannels + ",samplerate=" + _iSampleRate + "}:std{access=http,mux=raw,dst=" + _sNetworkAddr + ":" + _iPort + "}");
            _bOptionApplied = true;
            _bConfigChanged = false;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public override void Dispose()
        {
            _vEventMan.clearEventHandlers();
            stopStream();
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
