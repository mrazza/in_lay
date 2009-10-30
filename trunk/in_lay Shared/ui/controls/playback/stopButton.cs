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

namespace in_lay_Shared.ui.controls.playback
{
    /// <summary>
    /// Stops playback of any tracks
    /// </summary>
    public sealed class stopButton : inlayButton
    {
        #region Events
        /// <summary>
        /// Called when [click].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected override void onClick(object oSender, RoutedEventArgs rArgs)
        {
            _nPlayer.stopMedia();
            rArgs.Handled = true;
        }
        #endregion
    }
}
