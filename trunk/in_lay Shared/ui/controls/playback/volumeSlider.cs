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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Windows;
using inlayShared.ui.controls.core;
using netAudio.core.events;

namespace inlayShared.ui.controls.playback
{
    /// <summary>
    /// Slider that changes the volume of the player
    /// </summary>
    public sealed class volumeSlider : inlaySlider
    {
        #region Members
        /// <summary>
        /// Player Volume Changed Event Handler
        /// </summary>
        private EventHandler<volumeChangedEventArgs> _ePlayerVolumeChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="volumeSlider"/> class.
        /// </summary>
        public volumeSlider()
            : base()
        {
            _ePlayerVolumeChanged = null;
        }
        #endregion

        #region Events
        /// <summary>
        /// Called when [Value Changes].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected override void onValueChanged(object oSender, RoutedEventArgs rArgs)
        {
            _iSystem.nPlayer.iVolume = (int)Value;
            rArgs.Handled = true;
        }

        /// <summary>
        /// Handles the eVolumeChanged event of the _iSystem.nPlayer control.
        /// </summary>
        /// <remarks>Must be blocking, otherwise event trigger loop</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.volumeChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_eVolumeChanged(object sender, volumeChangedEventArgs e)
        {
            _gSystem.invokeOnLocalThread((Action)(() =>
            {
                if (e.bMuted)
                    Value = 0.0d;
                else
                    Value = e.iVolume;
            }), true);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            _iSystem.nPlayer.eVolumeChanged += (_ePlayerVolumeChanged = new EventHandler<volumeChangedEventArgs>(_nPlayer_eVolumeChanged));
            _nPlayer_eVolumeChanged(null, new volumeChangedEventArgs(_iSystem.nPlayer.iVolume, _iSystem.nPlayer.bMute)); //Make sure things display correctly on load
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
            if (_ePlayerVolumeChanged != null && _iSystem.nPlayer != null)
                _iSystem.nPlayer.eVolumeChanged -= _ePlayerVolumeChanged;

            base.Dispose();
        }
        #endregion
    }
}
