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

using System;
using netDiscographer.core;
using netDiscographer.core.events;
using inlayShared.ui.controls.library.core;

namespace inlayShared.ui.controls.library
{
    /// <summary>
    /// ListView used to display the main library
    /// </summary>
    public sealed class libraryListView : mediaListView
    {
        #region Members
        /// <summary>
        /// On search complete event
        /// </summary>
        private EventHandler _eOnSearchComplete;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="libraryListView"/> class.
        /// </summary>
        public libraryListView()
            : base()
        {
            mSortOrder = new metaDataFieldTypes[] { metaDataFieldTypes.artist, metaDataFieldTypes.album, metaDataFieldTypes.track, metaDataFieldTypes.title };
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            _iSystem.iLibSystem.eOnMediaChanged += (_eOnSearchComplete = new EventHandler(iLibSystem_eSearchComplete));
            mData = _iSystem.iLibSystem.lCurrentLibrary.mCurrentMedia;
            base.completeInitialization();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the eSearchComplete event of the iLibSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void iLibSystem_eSearchComplete(object sender, EventArgs e)
        {
            mData = _iSystem.iLibSystem.lCurrentLibrary.mCurrentMedia;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (_eOnSearchComplete != null)
                _iSystem.iLibSystem.eOnMediaChanged -= _eOnSearchComplete;

            base.Dispose();
        }
        #endregion
    }
}
