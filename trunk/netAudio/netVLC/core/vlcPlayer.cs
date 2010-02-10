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
using System.Runtime.InteropServices;
using netAudio.core;

namespace netAudio.netVLC.core
{
    /// <summary>
    /// vlcPlayer Instance to play/manage media
    /// Controls the decoding and playback of media but not the output
    /// </summary>
    public sealed class vlcPlayer : IVLCCoreUser, IDisposable
    {
        #region State Enum
        /// <summary>
        /// VLC Player State
        /// </summary>
        public enum vlcPlayerState : int
        {
            /// <summary>
            /// Nothing Special/No Valid State
            /// </summary>
            nothingSpecial,

            /// <summary>
            /// Opening Media
            /// </summary>
            opening,

            /// <summary>
            /// Buffering Media
            /// </summary>
            buffering,

            /// <summary>
            /// Playing Media
            /// </summary>
            playing,

            /// <summary>
            /// Media Paused
            /// </summary>
            paused,

            /// <summary>
            /// Media Stopped
            /// </summary>
            stopped,

            /// <summary>
            /// Media Ended
            /// </summary>
            ended,

            /// <summary>
            /// Error
            /// </summary>
            error
        }
        #endregion

        #region Members
        /// <summary>
        /// Handle to the player object
        /// </summary>
        private vlcPlayerHandle _vHandle;

        /// <summary>
        /// VLC base Core
        /// </summary>
        private vlcCore _vCore;
        #endregion

        #region Properties
        /// <summary>
        /// Player Handle
        /// </summary>
        internal vlcPlayerHandle vHandle
        {
            get
            {
                return _vHandle;
            }
        }

        /// <summary>
        /// vCore this media is from
        /// </summary>
        public vlcCore vCore
        {
            get
            {
                return _vCore;
            }
        }

        /// <summary>
        /// Position in the track (milliseconds)
        /// -1 if error
        /// </summary>
        public long lPosition
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return -1;

                // We can't get time if there is no track playing
                if (pState != vlcPlayerState.playing && pState != vlcPlayerState.paused)
                    return 0;

                long ret = libvlc_media_player_get_time(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
            set
            {
                if (_vHandle.IsInvalid)
                    return;

                if (bCanSeek)
                {
                    libvlc_media_player_set_time(_vHandle, value, ref _vCore.vException.vExStruct);
                    _vCore.handleError();
                }
            }
        }

        /// <summary>
        /// Current position in the track (%)
        /// -1.0f if error
        /// </summary>
        public float fPosition
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return -1.0f;

                // We can't get time if there is no track playing
                if (pState != vlcPlayerState.playing && pState != vlcPlayerState.paused)
                    return 0.0f;

                float ret = libvlc_media_player_get_position(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
            set
            {
                if (_vHandle.IsInvalid)
                    return;

                if (bCanSeek)
                {
                    libvlc_media_player_set_position(_vHandle, value, ref _vCore.vException.vExStruct);
                    _vCore.handleError();
                }
            }
        }

        /// <summary>
        /// Length of the current track (milliseconds)
        /// -1 if error
        /// </summary>
        public long lLength
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return -1;

                long ret = libvlc_media_player_get_length(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// Is the current track seekable?
        /// </summary>
        public bool bCanSeek
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return false;

                bool ret = libvlc_media_player_is_seekable(_vHandle, ref _vCore.vException.vExStruct) != 0;
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// Can we pause the current track?
        /// </summary>
        public bool bCanPause
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return false;

                bool ret = libvlc_media_player_can_pause(_vHandle, ref _vCore.vException.vExStruct) != 0;
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// Current state of the player
        /// </summary>
        public vlcPlayerState pState
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return vlcPlayerState.error;

                vlcPlayerState ret = libvlc_media_player_get_state(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// vlcMedia currently in the player
        /// </summary>
        public vlcMedia vMedia
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return null;

                vlcMedia ret = new vlcMedia(_vCore, libvlc_media_player_get_media(_vHandle, ref _vCore.vException.vExStruct));
                _vCore.handleError();
                return ret;
            }
            set
            {
                if (_vHandle.IsInvalid)
                    return;

                libvlc_media_player_set_media(_vHandle, value.vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
            }
        }

        /// <summary>
        /// Current media playback rate
        /// -1.0f if error
        /// </summary>
        public float fPlaybackRate
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return -1.0f;

                float ret = libvlc_media_player_get_rate(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
            set
            {
                if (_vHandle.IsInvalid)
                    return;

                libvlc_media_player_set_rate(_vHandle, value, ref _vCore.vException.vExStruct);
                _vCore.handleError();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of the player from the vlcCore
        /// </summary>
        /// <param name="vCore">Base core to create from</param>
        public vlcPlayer(vlcCore vCore)
        {
            _vCore = vCore;
            _vHandle = libvlc_media_player_new(_vCore.vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }

        /// <summary>
        /// Creates an instance of the player from a vlcMedia instance
        /// </summary>
        /// <param name="vMedia">VLC Media to create from</param>
        public vlcPlayer(vlcMedia vMedia)
        {
            _vCore = vMedia.vCore;
            _vHandle = libvlc_media_player_new_from_media(vMedia.vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcPlayer()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Starts the player
        /// </summary>
        public void playMedia()
        {
            if (_vHandle.IsInvalid)
                return;

            libvlc_media_player_play(_vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }

        /// <summary>
        /// Sets the media and starts the player
        /// </summary>
        /// <param name="media">Media to play</param>
        public void playMedia(vlcMedia media)
        {
            vMedia = media;
            playMedia();
        }

        /// <summary>
        /// Pauses the player
        /// </summary>
        public void pauseMedia()
        {
            if (_vHandle.IsInvalid)
                return;

            if (!bCanPause)
                return;

            libvlc_media_player_pause(_vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }

        /// <summary>
        /// Stops the player
        /// </summary>
        public void stopMedia()
        {
            if (_vHandle.IsInvalid)
                return;

            libvlc_media_player_stop(_vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }

        /// <summary>
        /// Converts a vlcPlayerState to a netAudio.playerState
        /// </summary>
        /// <param name="state">vlcPlayerState to convert</param>
        /// <returns>netAudio Equivelent state</returns>
        public static playerState vlcToNetAudioPlayerState(vlcPlayerState state)
        {
            switch (state)
            {
                case vlcPlayerState.nothingSpecial:
                    return playerState.waiting;

                case vlcPlayerState.buffering:
                    return playerState.buffering;

                case vlcPlayerState.ended:
                    return playerState.ended;

                case vlcPlayerState.opening:
                    return playerState.opening;

                case vlcPlayerState.paused:
                    return playerState.paused;

                case vlcPlayerState.playing:
                    return playerState.playing;

                case vlcPlayerState.stopped:
                    return playerState.stopped;

                case vlcPlayerState.error:
                    return playerState.error;
            }

            throw new NotImplementedException("Invalid/Unsupported State");
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public void Dispose()
        {
            _vHandle.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Creates an instance of the player from the core handle
        /// </summary>
        /// <param name="vHandle">Core handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>A new player handle</returns>
        [DllImport("libvlc")]
        private static extern vlcPlayerHandle libvlc_media_player_new(vlcCoreHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Creates an instance of the player from a media handle
        /// </summary>
        /// <param name="vHandle">Media handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>A new player handle</returns>
        [DllImport("libvlc")]
        private static extern vlcPlayerHandle libvlc_media_player_new_from_media(vlcMediaHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Plays media from a player
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_play(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Pauses media from a player
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_pause(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Stops the media in the player
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_stop(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Get the position in the track
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <return>Position in the track in milliseconds</return>
        [DllImport("libvlc")]
        private static extern long libvlc_media_player_get_time(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the position in the track
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="lTime">Time/Distance into the track</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_set_time(vlcPlayerHandle vHandle, long lTime, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the current positon in the track (%)
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>% into the track</returns>
        [DllImport("libvlc")]
        private static extern float libvlc_media_player_get_position(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the current position in the track
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="fPosition">% into the track</param>
        /// <param name="vException">Exception track</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_set_position(vlcPlayerHandle vHandle, float fPosition, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the total track length
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Total length of track in milliseconds</returns>
        [DllImport("libvlc")]
        private static extern long libvlc_media_player_get_length(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Checks if VLC can seek this media
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Returns 1 if seekable, otherwise 0</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_media_player_is_seekable(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Checks if VLC can pause this media
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Returns 1 if pausable, otherwise 0</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_media_player_can_pause(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the current state of the player
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Current player state</returns>
        [DllImport("libvlc")]
        private static extern vlcPlayerState libvlc_media_player_get_state(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the current media handle
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Media handle</returns>
        [DllImport("libvlc")]
        private static extern vlcMediaHandle libvlc_media_player_get_media(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the current media handle
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vMedia">Media handle</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_set_media(vlcPlayerHandle vHandle, vlcMediaHandle vMedia, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the current playback rate
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Playback rate (%)</returns>
        [DllImport("libvlc")]
        private static extern float libvlc_media_player_get_rate(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the playback rate
        /// </summary>
        /// <param name="vHandle">Player handle</param>
        /// <param name="fRate">New playback rate (%)</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_set_rate(vlcPlayerHandle vHandle, float fRate, ref vlcException.vlcExceptionStruct vException);
        #endregion
    }
}
