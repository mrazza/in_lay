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
using netAudio.core.events;

namespace in_lay_Shared.ui.controls.main
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
        private EventHandler<volumeChangedEventArgs> ePlayerVolumeChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="volumeSlider"/> class.
        /// </summary>
        public volumeSlider()
            : base()
        {
            ePlayerVolumeChanged = null;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Called when [initialize complete].
        /// </summary>
        /// <remarks>When overriding this function, you must call base.onGooeyInitializationComplete AFTER any new code.</remarks>
        public override void onGooeyInitializationComplete()
        {
            _nPlayer.eVolumeChanged += (ePlayerVolumeChanged = new EventHandler<volumeChangedEventArgs>(_nPlayer_eVolumeChanged));
            _nPlayer_eVolumeChanged(null, new volumeChangedEventArgs(_nPlayer.iVolume, _nPlayer.bMute)); //Make sure things display correctly on load
            base.onGooeyInitializationComplete();
        }
        #endregion

        #region Events
        /// <summary>
        /// Called when [Value Changes].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        public override void onValueChanged(object oSender, RoutedEventArgs rArgs)
        {
            _nPlayer.iVolume = (int)Value;
            rArgs.Handled = true;
        }

        /// <summary>
        /// Handles the eVolumeChanged event of the _nPlayer control.
        /// </summary>
        /// <remarks>Must be blocking, otherwise event trigger loop</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.volumeChangedEventArgs"/> instance containing the event data.</param>
        void _nPlayer_eVolumeChanged(object sender, volumeChangedEventArgs e)
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

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (ePlayerVolumeChanged != null && _nPlayer != null)
                _nPlayer.eVolumeChanged -= ePlayerVolumeChanged;

            base.Dispose();
        }
        #endregion
    }
}
