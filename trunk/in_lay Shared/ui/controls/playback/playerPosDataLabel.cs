/*******************************************************************
 * This file is part of the in_lay Player Shared Library.
 * 
 * in_lay source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * in_lay Player is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Windows;
using in_lay_Shared.ui.controls.core;
using netAudio.core;
using netAudio.core.events;

namespace in_lay_Shared.ui.controls.playback
{
    /// <summary>
    /// Label that displays playback position related data for the player
    /// </summary>
    public sealed class playerPosDataLabel : inlayLabel
    {
        #region Members
        /// <summary>
        /// Player Position Changed Event Handler
        /// </summary>
        private EventHandler<positionChangedEventArgs> _ePositionChanged;

        /// <summary>
        /// Player State Changed Event Args
        /// </summary>
        private EventHandler<stateChangedEventArgs> _eStateChanged;

        /// <summary>
        /// Is the current text track text?
        /// </summary>
        private bool _bCurrentIsTrackText;
        #endregion

        #region Dependency Properties
        #region OnTrackText
        /// <summary>
        /// Text to display when there is a track loaded
        /// </summary>
        public static readonly DependencyProperty onTrackTextProperty = DependencyProperty.Register("OnTrackText", typeof(string), typeof(trackDataLabel), new PropertyMetadata(null));

        /// <summary>
        /// Text to display when there is a track loaded
        /// </summary>
        public string OnTrackText
        {
            get
            {
                return (string)GetValue(onTrackTextProperty);
            }
            set
            {
                SetValue(onTrackTextProperty, value);
            }
        }
        #endregion

        #region OnNullText
        /// <summary>
        /// Text to display when there is no track loaded
        /// </summary>
        public static readonly DependencyProperty onNullTextProperty = DependencyProperty.Register("OnNullText", typeof(string), typeof(trackDataLabel), new PropertyMetadata(null));

        /// <summary>
        /// Text to display when there is no track loaded
        /// </summary>
        public string OnNullText
        {
            get
            {
                return (string)GetValue(onNullTextProperty);
            }
            set
            {
                SetValue(onNullTextProperty, value);
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="playerPosDataLabel"/> class.
        /// </summary>
        public playerPosDataLabel()
            : base()
        {
            _ePositionChanged = null;
            _eStateChanged = null;
            _bCurrentIsTrackText = false;
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the ePositionChanged event of the _nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.positionChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_ePositionChanged(object sender, positionChangedEventArgs e)
        {
            _gSystem.invokeOnLocalThread((Action)(() =>
            {
                Content = string.Format(OnTrackText, new TimeSpan(e.lPosition * 1000), e.fPosition);
                _bCurrentIsTrackText = true;
            }));
        }

        /// <summary>
        /// Handles the eStateChanged event of the _nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.stateChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_eStateChanged(object sender, stateChangedEventArgs e)
        {
            switch (e.pState)
            {
                case playerState.playing:
                case playerState.paused:
                case playerState.opening:
                    return;

                default:
                    if (!_bCurrentIsTrackText && _isInitializationComplete)
                        return;

                    _gSystem.invokeOnLocalThread((Action)(() =>
                    {
                        Content = OnNullText;
                    }));

                    _bCurrentIsTrackText = false;
                    break;
            }
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            _nPlayer.ePositionChanged += (_ePositionChanged = new EventHandler<positionChangedEventArgs>(_nPlayer_ePositionChanged));
            _nPlayer.eStateChanged += (_eStateChanged = new EventHandler<stateChangedEventArgs>(_nPlayer_eStateChanged));
            _nPlayer_eStateChanged(null, new stateChangedEventArgs(_nPlayer.pState));
            base.completeInitialization();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (_ePositionChanged != null && _nPlayer != null)
                _nPlayer.ePositionChanged -= _ePositionChanged;

            if (_eStateChanged != null && _nPlayer != null)
                _nPlayer.eStateChanged -= _eStateChanged;

            base.Dispose();
        }
        #endregion
    }
}
