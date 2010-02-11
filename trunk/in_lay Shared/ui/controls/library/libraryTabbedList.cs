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
using System.Windows.Controls;
using inlayShared.ui.controls.core;

namespace inlayShared.ui.controls.library
{
    /// <summary>
    /// Tab Control that lists all of the available libraries
    /// </summary>
    public sealed class libraryTabbedList : inlayTabControl
    {
        #region Members
        /// <summary>
        /// Triggered when the list of valid libraries has changed
        /// </summary>
        private EventHandler _eOnLibrariesChanged;

        /// <summary>
        /// Called when a tab is clicked
        /// </summary>
        private SelectionChangedEventHandler _eOnTabClicked;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="libraryTabbedList"/> class.
        /// </summary>
        public libraryTabbedList()
        {
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            SelectionChanged += (_eOnTabClicked = new SelectionChangedEventHandler(libraryTabbedList_SelectionChanged));
            iSystem.iLibSystem.eOnLibrariesChanged += (_eOnLibrariesChanged = new EventHandler(iLibSystem_eOnLibrariesChanged));
            reloadLibraryList();
            base.completeInitialization();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the SelectionChanged event of the libraryTabbedList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void libraryTabbedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iSystem.iLibSystem.iCurrentLibrary = SelectedIndex;
            e.Handled = true;
        }

        /// <summary>
        /// Handles the eOnLibrariesChanged event of the iLibSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void iLibSystem_eOnLibrariesChanged(object sender, EventArgs e)
        {
            reloadLibraryList();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Reloads the library list
        /// </summary>
        private void reloadLibraryList()
        {
            iSystem.gSystem.invokeOnLocalThread((Action)(() =>
            {
                int iCount = iSystem.iLibSystem.getLibraryCount();

                this.Items.Clear();
                TabItem tCurrItem;

                for (int iLoop = 0; iLoop < iCount; iLoop++)
                {
                    tCurrItem = new TabItem();
                    tCurrItem.Header = iSystem.iLibSystem.getLibrary(iLoop).sName;
                    this.Items.Add(tCurrItem);
                }
            }));
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (_eOnLibrariesChanged != null)
                iSystem.iLibSystem.eOnLibrariesChanged -= _eOnLibrariesChanged;

            if (_eOnTabClicked != null)
                SelectionChanged -= _eOnTabClicked;
            
            base.Dispose();
        }
        #endregion
    }
}
