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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using netAudio.core;
using netAudio.core.events;
using netAudio.netVLC.events;

namespace netAudio.netVLC.core
{
    /// <summary>
    /// Manages events with libVLC
    /// </summary>
    public sealed class vlcEventManager : IVLCCoreUser, IDisposable
    {
        #region Function Callback
        /// <summary>
        /// Delegate for the libVLC Event System
        /// </summary>
        /// <param name="vCallbackArgs">Callback Argument Struct</param>
        /// <param name="pUserData">Ignore</param>
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        private delegate void vlcEventCallback(ref vlcCallbackArgs vCallbackArgs, IntPtr pUserData);
        #endregion

        #region Members
        /// <summary>
        /// Called when the player state changes
        /// </summary>
        private event EventHandler<stateChangedEventArgs> _eStateChanged;

        /// <summary>
        /// Called when the player volume changes
        /// </summary>
        private event EventHandler<volumeChangedEventArgs> _eVolumeChanged;

        /// <summary>
        /// Called when the player progress/position changes
        /// </summary>
        private event EventHandler<positionChangedEventArgs> _ePositionChanged;

        /// <summary>
        /// Called when the player speed is changed
        /// </summary>
        private event EventHandler<speedChangedEventArgs> _eSpeedChanged;

        /// <summary>
        /// Currently Bound Event Types
        /// </summary>
        private Dictionary<eventTypes, int> _hBoundEvents;

        /// <summary>
        /// Handle to the event manager
        /// </summary>
        private vlcEventManagerHandle _vHandle;

        /// <summary>
        /// vPlayer this handler is for
        /// </summary>
        private vlcPlayer _vPlayer;

        /// <summary>
        /// Core VLC Object
        /// </summary>
        private vlcCore _vCore;

        /// <summary>
        /// netVLC Player (if we're using one)
        /// </summary>
        private netVLCPlayer _netVLCPlayer;

        #region Callback Delegates
        /// <summary>
        /// Callback Delegate for onStateChanged
        /// </summary>
        private vlcEventCallback _vOnStateChanged;

        /// <summary>
        /// Callback Delegate for onPositionChanged
        /// </summary>
        private vlcEventCallback _vOnPositionChanged;
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Called when the player state changes
        /// It is recommended that you use BeginInvoke() in the delegate function
        /// </summary>
        public event EventHandler<stateChangedEventArgs> eStateChanged
        {
            add
            {
                establishPlayerEventCallbacks(eventTypes.stateChanged);
                _eStateChanged += value;
            }
            remove
            {
                _eStateChanged -= value;
                removePlayerEventCallbacks(eventTypes.stateChanged);
            }
        }

        /// <summary>
        /// Called when the player volume changes
        /// It is recommended that you use BeginInvoke() in the delegate function
        /// </summary>
        public event EventHandler<volumeChangedEventArgs> eVolumeChanged
        {
            add
            {
                _eVolumeChanged += value;
            }
            remove
            {
                _eVolumeChanged -= value;
            }
        }

        /// <summary>
        /// Called when the player progress/position changes
        /// It is recommended that you use BeginInvoke() in the delegate function
        /// </summary>
        public event EventHandler<positionChangedEventArgs> ePositionChanged
        {
            add
            {
                establishPlayerEventCallbacks(eventTypes.positionChanged);
                _ePositionChanged += value;
            }
            remove
            {
                _ePositionChanged -= value;
                removePlayerEventCallbacks(eventTypes.positionChanged);
            }
        }

        /// <summary>
        /// Called when the player speed is changed
        /// It is recommended that you use BeginInvoke() in the delegate function
        /// </summary>
        public event EventHandler<speedChangedEventArgs> eSpeedChanged
        {
            add
            {
                _eSpeedChanged += value;
            }
            remove
            {
                _eSpeedChanged -= value;
            }
        }

        /// <summary>
        /// vPlayer this handler is for
        /// </summary>
        public vlcPlayer vPlayer
        {
            get
            {
                return _vPlayer;
            }
        }

        /// <summary>
        /// Core VLC Object
        /// </summary>
        public vlcCore vCore
        {
            get
            {
                return _vCore;
            }
        }

        /// <summary>
        /// Internal handle
        /// </summary>
        internal vlcEventManagerHandle vHandle
        {
            get
            {
                return _vHandle;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// netVLC Player Constructor
        /// </summary>
        /// <param name="vlcPlayer">netVLC Player</param>
        public vlcEventManager(netVLCPlayer vlcPlayer)
            : this(vlcPlayer.vPlayer)
        {
            _netVLCPlayer = vlcPlayer;
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="vPlayer">vlcPlayer we are handling for</param>
        public vlcEventManager(vlcPlayer vPlayer)
        {
            _netVLCPlayer = null; //Make sure this defaults to null
            _vPlayer = vPlayer;
            _vCore = vPlayer.vCore;
            _vHandle = libvlc_media_player_event_manager(_vPlayer.vHandle, ref _vCore.vException.vExStruct);
            _vCore.handleError();
            _vPlayer.vHandle.RetainHandle();

            // Set up event tracking
            _hBoundEvents = new Dictionary<eventTypes, int>();
            _hBoundEvents.Add(eventTypes.positionChanged, 0);
            _hBoundEvents.Add(eventTypes.stateChanged, 0);

            // Create the event delegates
            _vOnStateChanged = new vlcEventCallback(onStateChanged);
            _vOnPositionChanged = new vlcEventCallback(onPositionChanged);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcEventManager()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Invokes the state changed event
        /// </summary>
        /// <param name="arguments">Arguments to pass</param>
        public void invokeStateChanged(stateChangedEventArgs arguments)
        {
            if (_eStateChanged == null)
                return;

            if (_netVLCPlayer != null)
                _eStateChanged.Invoke(_netVLCPlayer, arguments);
            else
                _eStateChanged.Invoke(this, arguments);
        }

        /// <summary>
        /// Invokes the volume changed event
        /// </summary>
        /// <param name="arguments">Arguments to pass</param>
        public void invokeVolumeChanged(volumeChangedEventArgs arguments)
        {
            if (_eVolumeChanged == null)
                return;

            if (_netVLCPlayer != null)
                _eVolumeChanged.Invoke(_netVLCPlayer, arguments);
            else
                _eVolumeChanged.Invoke(this, arguments);
        }

        /// <summary>
        /// Invokes the position changed event
        /// </summary>
        /// <param name="arguments">Arguments to pass</param>
        public void invokePositionChanged(positionChangedEventArgs arguments)
        {
            if (_ePositionChanged == null)
                return;

            if (_netVLCPlayer != null)
                _ePositionChanged.Invoke(_netVLCPlayer, arguments);
            else
                _ePositionChanged.Invoke(this, arguments);
        }

        /// <summary>
        /// Invokes the speed changed event
        /// </summary>
        /// <param name="arguments">Arguments to pass</param>
        public void invokeSpeedChanged(speedChangedEventArgs arguments)
        {
            if (_eSpeedChanged == null)
                return;

            if (_netVLCPlayer != null)
                _eSpeedChanged.Invoke(_netVLCPlayer, arguments);
            else
                _eSpeedChanged.Invoke(this, arguments);
        }

        /// <summary>
        /// Clears the event handlers
        /// </summary>
        public void clearEventHandlers()
        {
            // Set to null
            _eStateChanged = null;
            _eVolumeChanged = null;
            _ePositionChanged = null;
            _eSpeedChanged = null;
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Delegate method to be called when the state changes
        /// </summary>
        /// <param name="vCallbackArgs">Callback Argument Struct</param>
        /// <param name="pUserData">User Supplied Data</param>
        private void onStateChanged(ref vlcCallbackArgs vCallbackArgs, IntPtr pUserData)
        {
            invokeStateChanged(new stateChangedEventArgs((playerState)pUserData));
        }

        /// <summary>
        /// Delegate method to be called when the position changes
        /// </summary>
        /// <param name="vCallbackArgs">Callback Argument Struct</param>
        /// <param name="pUserData">User Supplied Data</param>
        private void onPositionChanged(ref vlcCallbackArgs vCallbackArgs, IntPtr pUserData)
        {
            //invokePositionChanged(new positionChangedEventArgs(_vPlayer.lPosition, vCallbackArgs.vPlayerPositionChanged.fPosition)); //Replaced because the callback args were returning null
            invokePositionChanged(new positionChangedEventArgs(_vPlayer.lPosition, _vPlayer.fPosition));
        }

        /// <summary>
        /// Establishes the player event callbacks between netVLC and libVLC
        /// </summary>
        private void establishPlayerEventCallbacks(eventTypes newEvent)
        {
            if (_vHandle.IsInvalid)
                return;

            // We're going to attach the callbacks and check for errors each time
            switch (newEvent)
            {
                case eventTypes.positionChanged:
                    if (_hBoundEvents[eventTypes.positionChanged]++ == 0)
                    {
                        libvlc_event_attach(_vHandle, vlcEventType.playerPositionChanged, _vOnPositionChanged, IntPtr.Zero, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                    }
                    break;

                case eventTypes.stateChanged:
                    if (_hBoundEvents[eventTypes.stateChanged]++ == 0)
                    {
                        libvlc_event_attach(_vHandle, vlcEventType.playerPlaying, _vOnStateChanged, (IntPtr)playerState.playing, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerPaused, _vOnStateChanged, (IntPtr)playerState.paused, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerStopped, _vOnStateChanged, (IntPtr)playerState.stopped, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerOpening, _vOnStateChanged, (IntPtr)playerState.opening, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerBuffering, _vOnStateChanged, (IntPtr)playerState.buffering, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerEndReached, _vOnStateChanged, (IntPtr)playerState.ended, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerEncounteredError, _vOnStateChanged, (IntPtr)playerState.error, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_attach(_vHandle, vlcEventType.playerNothingSpecial, _vOnStateChanged, (IntPtr)playerState.waiting, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                    }
                    break;
            }
        }

        /// <summary>
        /// Detaches all the events
        /// </summary>
        private void removePlayerEventCallbacks()
        {
            if (_vHandle.IsInvalid)
                return;

            // We're going to detach all the callbacks and check for errors each time
            if (_hBoundEvents[eventTypes.positionChanged] != 0)
            {
                libvlc_event_detach(_vHandle, vlcEventType.playerPositionChanged, _vOnPositionChanged, IntPtr.Zero, ref _vCore.vException.vExStruct);
                _vCore.handleError();

                _hBoundEvents[eventTypes.positionChanged] = 0;
            }

            if (_hBoundEvents[eventTypes.stateChanged] != 0)
            {
                libvlc_event_detach(_vHandle, vlcEventType.playerPlaying, _vOnStateChanged, (IntPtr)playerState.playing, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerPaused, _vOnStateChanged, (IntPtr)playerState.paused, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerStopped, _vOnStateChanged, (IntPtr)playerState.stopped, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerOpening, _vOnStateChanged, (IntPtr)playerState.opening, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerBuffering, _vOnStateChanged, (IntPtr)playerState.buffering, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerEndReached, _vOnStateChanged, (IntPtr)playerState.ended, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                libvlc_event_detach(_vHandle, vlcEventType.playerNothingSpecial, _vOnStateChanged, (IntPtr)playerState.waiting, ref _vCore.vException.vExStruct);
                _vCore.handleError();

                _hBoundEvents[eventTypes.stateChanged] = 0;
            }
        }

        /// <summary>
        /// Detaches the type of event
        /// </summary>
        private void removePlayerEventCallbacks(eventTypes newEvent)
        {
            if (_vHandle.IsInvalid)
                return;

            // We're going to detach all the callbacks and check for errors each time
            switch (newEvent)
            {
                case eventTypes.positionChanged:
                    if (--_hBoundEvents[eventTypes.positionChanged] == 0)
                    {
                        libvlc_event_detach(_vHandle, vlcEventType.playerPositionChanged, _vOnPositionChanged, IntPtr.Zero, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                    }
                    break;

                case eventTypes.stateChanged:
                    if (--_hBoundEvents[eventTypes.stateChanged] == 0)
                    {
                        libvlc_event_detach(_vHandle, vlcEventType.playerPlaying, _vOnStateChanged, (IntPtr)playerState.playing, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerPaused, _vOnStateChanged, (IntPtr)playerState.paused, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerStopped, _vOnStateChanged, (IntPtr)playerState.stopped, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerOpening, _vOnStateChanged, (IntPtr)playerState.opening, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerBuffering, _vOnStateChanged, (IntPtr)playerState.buffering, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerEndReached, _vOnStateChanged, (IntPtr)playerState.ended, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerStopped, _vOnStateChanged, (IntPtr)playerState.stopped, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                        libvlc_event_detach(_vHandle, vlcEventType.playerNothingSpecial, _vOnStateChanged, (IntPtr)playerState.waiting, ref _vCore.vException.vExStruct);
                        _vCore.handleError();
                    }
                    break;
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public void Dispose()
        {
            removePlayerEventCallbacks();
            _vHandle.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Creates a new instance of the event manager
        /// </summary>
        /// <param name="vHandle">Player</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern vlcEventManagerHandle libvlc_media_player_event_manager(vlcPlayerHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Creates a new instance of the event manager
        /// </summary>
        /// <param name="vHandle">Media</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern vlcEventManagerHandle libvlc_media_event_manager(vlcMediaHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Attaches an event to the system
        /// </summary>
        /// <param name="vHandle">Event System Handle</param>
        /// <param name="vEventType">Event Type</param>
        /// <param name="vCallback">Callback function</param>
        /// <param name="pUserData">User data (Can be Empty Pointer)</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_event_attach(vlcEventManagerHandle vHandle, vlcEventType vEventType, vlcEventCallback vCallback, IntPtr pUserData, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Detaches an event from the system
        /// </summary>
        /// <param name="vHandle">Event System Handle</param>
        /// <param name="vEventType">Event Type</param>
        /// <param name="vCallback">Callback function</param>
        /// <param name="pUserData">User data (Can be Empty Pointer)</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_event_detach(vlcEventManagerHandle vHandle, vlcEventType vEventType, vlcEventCallback vCallback, IntPtr pUserData, ref vlcException.vlcExceptionStruct vException);
        #endregion
    }
}
