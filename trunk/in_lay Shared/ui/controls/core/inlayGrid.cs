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

using netAudio.core;
using netGooey.controls;

namespace inlayShared.ui.controls.core
{
    /// <summary>
    /// Abstract in_lay Grid base
    /// </summary>
    public abstract class inlayGrid : gooeyGrid, IPlayerControl
    {
        #region Members
        /// <summary>
        /// netAudioPlayer the control manages
        /// </summary>
        protected netAudioPlayer _nPlayer;
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
        /// Initializes a new instance of the <see cref="inlayButton"/> class.
        /// </summary>
        public inlayGrid()
            : base()
        {
            _nPlayer = null;
        }
        #endregion
    }
}
