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
using inlayShared.ui.controls.core;
using netAudio.core;
using netAudio.core.events;

namespace inlayShared.ui.controls.playback
{
    /// <summary>
    /// Slider control that displays and can change track position
    /// </summary>
    public sealed class seekerSlider : inlaySlider
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
        /// Last set position
        /// </summary>
        /// <remarks>Used to prevent skipping; track moves forward, and then moves back to the previous position due to event overlap</remarks>
        private int iLastPos;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="seekerSlider"/> class.
        /// </summary>
        public seekerSlider()
            : base()
        {
            _ePositionChanged = null;
            _eStateChanged = null;
            Minimum = 0.0d;
            Maximum = 100.0d;
            iLastPos = 0;
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the ePositionChanged event of the _iSystem.nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.positionChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_ePositionChanged(object sender, positionChangedEventArgs e)
        {
            _gSystem.invokeOnLocalThread((Action)(() =>
            {
                iLastPos = (int)(e.fPosition * 100);
                Value = iLastPos;
            }));
        }

        /// <summary>
        /// Handles the eStateChanged event of the _iSystem.nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.stateChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_eStateChanged(object sender, stateChangedEventArgs e)
        {
            switch (e.pState)
            {
                case playerState.playing:
                case playerState.paused:
                    return;

                default:
                    _gSystem.invokeOnLocalThread((Action)(() =>
                    {
                        Value = 0;
                        iLastPos = 0;
                    }));
                    break;
            }
        }

        /// <summary>
        /// Called when [Value Changes].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected override void onValueChanged(object oSender, RoutedEventArgs rArgs)
        {
            if (iLastPos == (int)Value)
                return;

            iLastPos = (int)Value;
            _iSystem.nPlayer.fPosition = (float)(Value / 100);
            rArgs.Handled = true;
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            _iSystem.nPlayer.ePositionChanged += (_ePositionChanged = new EventHandler<positionChangedEventArgs>(_nPlayer_ePositionChanged));
            _iSystem.nPlayer.eStateChanged += (_eStateChanged = new EventHandler<stateChangedEventArgs>(_nPlayer_eStateChanged));
            _nPlayer_ePositionChanged(null, new positionChangedEventArgs(_iSystem.nPlayer.lPosition, _iSystem.nPlayer.fPosition));
            _nPlayer_eStateChanged(null, new stateChangedEventArgs(_iSystem.nPlayer.pState));
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
            if (_ePositionChanged != null && _iSystem.nPlayer != null)
                _iSystem.nPlayer.ePositionChanged -= _ePositionChanged;

            if (_eStateChanged != null && _iSystem.nPlayer != null)
                _iSystem.nPlayer.eStateChanged -= _eStateChanged;

            base.Dispose();
        }
        #endregion
    }
}
