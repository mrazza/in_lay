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

using System.Windows;
using in_lay_Shared.ui.controls.core;
using netAudio.core.events;
using netAudio.core;
using System;

namespace in_lay_Shared.ui.controls.main
{
    /// <summary>
    /// Grid grouping used to toggle between what is displayed when a track is playing or when it is paused/stopped
    /// </summary>
    public sealed class playPauseStopToggle : inlayGrid
    {
        #region Dependency Properties
        #region OnPlayShow
        /// <summary>
        /// OnPauseShow XAML Property
        /// </summary>
        public static readonly DependencyProperty onPlayShowProperty = DependencyProperty.RegisterAttached("OnPlayShow", typeof(bool), typeof(playPauseStopToggle));
        
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
        public static readonly DependencyProperty onPauseShowProperty = DependencyProperty.RegisterAttached("OnPauseShow", typeof(bool), typeof(playPauseStopToggle));

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
        public static readonly DependencyProperty onStopShowProperty = DependencyProperty.RegisterAttached("OnStopShow", typeof(bool), typeof(playPauseStopToggle));

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

        #region Public Members
        /// <summary>
        /// Called when [initialize complete].
        /// </summary>
        /// <remarks>When overriding this function, you must call base.onGooeyInitializationComplete AFTER any new code.</remarks>
        public override void onGooeyInitializationComplete()
        {
            _nPlayer.eStateChanged += new System.EventHandler<stateChangedEventArgs>(_nPlayer_eStateChanged);
            _nPlayer_eStateChanged(null, new stateChangedEventArgs(playerState.ready)); //Make sure things display correctly on load
            base.onGooeyInitializationComplete();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the eStateChanged event of the _nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.stateChangedEventArgs"/> instance containing the event data.</param>
        void _nPlayer_eStateChanged(object sender, stateChangedEventArgs e)
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
    }
}
