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
    /// Grid grouping used to toggle between what is displayed when a track is playing or when it is paused/stopped
    /// </summary>
    public sealed class playPauseStopToggle : inlayGrid
    {
        #region Members
        /// <summary>
        /// Player State Changed Event Handler
        /// </summary>
        private EventHandler<stateChangedEventArgs> _ePlayerStateChanged;
        #endregion

        #region Dependency Properties
        #region OnPlayShow
        /// <summary>
        /// OnPauseShow XAML Property
        /// </summary>
        public static readonly DependencyProperty onPlayShowProperty = DependencyProperty.RegisterAttached("OnPlayShow", typeof(bool), typeof(playPauseStopToggle), new PropertyMetadata(false));
        
        /// <summary>
        /// Sets the on play show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <param name="bValue">if set to <c>true</c> [bValue].</param>
        public static void SetOnPlayShow(UIElement cControl, Boolean bValue)
        {
            cControl.SetValue(onPlayShowProperty, bValue);
        }

        /// <summary>
        /// Gets the on play show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <returns>Value</returns>
        public static Boolean GetOnPlayShow(UIElement cControl)
        {
            return (Boolean)cControl.GetValue(onPlayShowProperty);
        }
        #endregion

        #region OnPauseShow
        /// <summary>
        /// OnPauseShow XAML Property
        /// </summary>
        public static readonly DependencyProperty onPauseShowProperty = DependencyProperty.RegisterAttached("OnPauseShow", typeof(bool), typeof(playPauseStopToggle), new PropertyMetadata(false));

        /// <summary>
        /// Sets the on pause show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <param name="bValue">if set to <c>true</c> [bValue].</param>
        public static void SetOnPauseShow(UIElement cControl, Boolean bValue)
        {
            cControl.SetValue(onPauseShowProperty, bValue);
        }

        /// <summary>
        /// Gets the on pause show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <returns>Value</returns>
        public static Boolean GetOnPauseShow(UIElement cControl)
        {
            return (Boolean)cControl.GetValue(onPauseShowProperty);
        }
        #endregion

        #region OnStopShow
        /// <summary>
        /// OnStopShow XAML Property
        /// </summary>
        public static readonly DependencyProperty onStopShowProperty = DependencyProperty.RegisterAttached("OnStopShow", typeof(bool), typeof(playPauseStopToggle), new PropertyMetadata(false));

        /// <summary>
        /// Sets the on stop show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <param name="bValue">if set to <c>true</c> [bValue].</param>
        public static void SetOnStopShow(UIElement cControl, Boolean bValue)
        {
            cControl.SetValue(onStopShowProperty, bValue);
        }

        /// <summary>
        /// Gets the on stop show.
        /// </summary>
        /// <param name="cControl">The control.</param>
        /// <returns>Value</returns>
        public static Boolean GetOnStopShow(UIElement cControl)
        {
            return (Boolean)cControl.GetValue(onStopShowProperty);
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="playPauseStopToggle"/> class.
        /// </summary>
        public playPauseStopToggle()
            : base()
        {
            _ePlayerStateChanged = null;
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the eStateChanged event of the _iSystem.nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.stateChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_eStateChanged(object sender, stateChangedEventArgs e)
        {
            _gSystem.invokeOnLocalThread((Action)(() =>
            {
                foreach (UIElement oCurr in this.InternalChildren)
                {
                    switch (e.pState)
                    {
                        case playerState.playing:
                            if (GetOnPlayShow(oCurr))
                                oCurr.Visibility = Visibility.Visible;
                            else
                                oCurr.Visibility = Visibility.Collapsed;
                            break;

                        case playerState.paused:
                            if (GetOnPauseShow(oCurr))
                                oCurr.Visibility = Visibility.Visible;
                            else
                                oCurr.Visibility = Visibility.Collapsed;
                            break;

                        default:
                            if (GetOnStopShow(oCurr))
                                oCurr.Visibility = Visibility.Visible;
                            else
                                oCurr.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }));
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            _iSystem.nPlayer.eStateChanged += (_ePlayerStateChanged = new EventHandler<stateChangedEventArgs>(_nPlayer_eStateChanged));
            _nPlayer_eStateChanged(null, new stateChangedEventArgs(playerState.ready)); //Make sure things display correctly on load
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
            if (_ePlayerStateChanged != null && _iSystem.nPlayer != null)
                _iSystem.nPlayer.eStateChanged -= _ePlayerStateChanged;

            base.Dispose();
        }
        #endregion
    }
}
