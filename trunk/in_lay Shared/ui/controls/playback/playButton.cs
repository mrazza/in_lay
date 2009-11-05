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
using inlayShared.ui.controls.core;

namespace inlayShared.ui.controls.playback
{
    /// <summary>
    /// Triggers playback of the netAudioObject attached
    /// </summary>
	public sealed class playButton : inlayButton
    {
        #region Events
        /// <summary>
        /// Called when [click].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected override void onClick(object oSender, RoutedEventArgs rArgs)
        {
            _iSystem.nPlayer.playMedia();
            rArgs.Handled = true;
        }
        #endregion
    }
}
