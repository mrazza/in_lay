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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

            // Subscribe to self-events
            this.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(libraryListView_MouseDoubleClick);
            this.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(libraryListView_MouseRightButtonUp);
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
        /// Handles the MouseDoubleClick event of the libraryListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void libraryListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mediaEntry mSelectedTrack = (mediaEntry)this.SelectedItem;

            if (mSelectedTrack == null)
                return;

            _iSystem.iLibSystem.lCurrentLibrary.iSelectedSong = this.SelectedIndex;
            _iSystem.nPlayer.sMediaPath = mSelectedTrack.sPath;
            _iSystem.nPlayer.playMedia();
        }

        /// <summary>
        /// Handles the MouseRightButtonUp event of the libraryListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void libraryListView_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

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
