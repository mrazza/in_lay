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
    /// Label that displays specific meta data for a current track
    /// </summary>
    public sealed class trackDataLabel : inlayLabel
    {
        #region Members
        /// <summary>
        /// Player State Changed Event Handler
        /// </summary>
        private EventHandler<stateChangedEventArgs> ePlayerStateChanged;

        /// <summary>
        /// Is the current text track text?
        /// </summary>
        private bool bCurrentIsTrackText;
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
        /// Initializes a new instance of the <see cref="trackDataLabel"/> class.
        /// </summary>
        public trackDataLabel()
            : base()
        {
            ePlayerStateChanged = null;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Called when [initialize complete].
        /// </summary>
        /// <remarks>When overriding this function, you must call base.onGooeyInitializationComplete AFTER any new code.</remarks>
        public override void onGooeyInitializationComplete()
        {
            _nPlayer.eStateChanged += (ePlayerStateChanged = new EventHandler<stateChangedEventArgs>(_nPlayer_eStateChanged));
            _nPlayer_eStateChanged(null, new stateChangedEventArgs(_nPlayer.pState)); //Make sure things display correctly on load
            base.onGooeyInitializationComplete();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the eStateChanged event of the _nPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netAudio.core.events.stateChangedEventArgs"/> instance containing the event data.</param>
        private void _nPlayer_eStateChanged(object sender, stateChangedEventArgs e)
        {
            _gSystem.invokeOnLocalThread((Action)(() =>
            {
                switch (e.pState)
                {
                    case playerState.playing:
                    case playerState.paused:
                    case playerState.opening:
                        displayTrackText();
                        break;

                    default:
                        displayNullText();
                        break;
                }
            }), true);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Displays the track text.
        /// </summary>
        private void displayTrackText()
        {
            if (bCurrentIsTrackText && _isInitializationComplete)
                return;

            Content = formatText(OnTrackText);

            bCurrentIsTrackText = true;
        }

        /// <summary>
        /// Displays the null text.
        /// </summary>
        private void displayNullText()
        {
            if (!bCurrentIsTrackText && _isInitializationComplete)
                return;

            Content = OnNullText;

            bCurrentIsTrackText = false;
        }

        /// <summary>
        /// Formats the text.
        /// </summary>
        /// <param name="sText">The text to format.</param>
        /// <returns></returns>
        private string formatText(string sText)
        {
            metaData mData = _nPlayer.mTrackData;
            return String.Format(sText, mData.ToArray());
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (ePlayerStateChanged != null && _nPlayer != null)
                _nPlayer.eStateChanged -= ePlayerStateChanged;

            base.Dispose();
        }
        #endregion
    }
}
