﻿/*******************************************************************
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
using System.Windows.Controls.Primitives;
using netAudio.core;
using netGooey.controls;

namespace in_lay_Shared.ui.controls.core
{
    /// <summary>
    /// in_lay Slider Control
    /// </summary>
    public abstract class inlaySlider : gooeySlider, IPlayerControl
    {
        #region Members
        /// <summary>
        /// netAudioPlayer the control manages
        /// </summary>
        protected netAudioPlayer _nPlayer;

        /// <summary>
        /// Event hanlder when slider value changes
        /// </summary>
        protected RoutedEventHandler _eOnValueChanged;
        #endregion

        #region Properties
        #region IPlayerControl Members
        /// <summary>
        /// Gets or sets the netAudioPlayer.
        /// </summary>
        /// <value>The netAudioPlayer.</value>
        public virtual netAudioPlayer nPlayer
        {
            get
            {
                return _nPlayer;
            }
            set
            {
                _nPlayer = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>If overridden, base.isGooeyInitialized must return <c>true</c> for the child version to return <c>true</c>.</remarks>
        public override bool isGooeyInitialized
        {
            get
            {
                return base.isGooeyInitialized && (_nPlayer != null);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="inlaySlider"/> class.
        /// </summary>
        public inlaySlider()
            : base()
        {
            _nPlayer = null;
            _eOnValueChanged = null;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Called when [initialize complete].
        /// </summary>
        /// <remarks>When overriding this function, you must call base.onGooeyInitializationComplete AFTER any new code.</remarks>
        public override void onGooeyInitializationComplete()
        {
            AddHandler(RangeBase.ValueChangedEvent, (_eOnValueChanged = new RoutedEventHandler(onValueChanged)));
            base.onGooeyInitializationComplete();
        }
        #endregion

        #region Events
        /// <summary>
        /// Called when [Value Changes].
        /// </summary>
        /// <param name="oSender">The origanal sender.</param>
        /// <param name="rArgs">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        public abstract void onValueChanged(object oSender, RoutedEventArgs rArgs);
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (_eOnValueChanged != null)
                RemoveHandler(RangeBase.ValueChangedEvent, _eOnValueChanged);

            base.Dispose();
        }
        #endregion
    }
}