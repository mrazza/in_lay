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
using System.Threading;
using System.Windows.Controls;
using inlayShared.ui.controls.core;

namespace inlayShared.ui.controls.library
{
    /// <summary>
    /// A TextBox that triggers searches on the currently active library
    /// </summary>
    public sealed class searchTextBox : inlayTextBox
    {
        #region Members
        /// <summary>
        /// Text Changed Event Hanlder
        /// </summary>
        private TextChangedEventHandler _eTextChanged;

        /// <summary>
        /// The search delay timer thread
        /// </summary>
        private Thread _tSearchTimer;

        /// <summary>
        /// Triggered when the current library has changed
        /// </summary>
        private EventHandler _eOnActiveLibraryChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="searchTextBox"/> class.
        /// </summary>
        public searchTextBox()
            : base() { }
        #endregion

        #region Protected Members
        /// <summary>
        /// Completes the initialization.
        /// </summary>
        /// <remarks>When overriding this function, you must call base.completeInitialization AFTER any new code.</remarks>
        protected override void completeInitialization()
        {
            TextChanged += (_eTextChanged = new TextChangedEventHandler(searchTextBox_TextChanged));
            _iSystem.iLibSystem.eOnActiveLibraryChanged += (_eOnActiveLibraryChanged = new EventHandler(iLibSystem_eOnActiveLibraryChanged));
            base.completeInitialization();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the TextChanged event of the searchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_tSearchTimer != null && _tSearchTimer.IsAlive)
                _tSearchTimer.Abort();

            _tSearchTimer = new Thread(new ParameterizedThreadStart(tSearchTimer_TriggerSearch));
            _tSearchTimer.Start(e);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the TriggerSearch event invoked by the timer
        /// </summary>
        /// <param name="state">The state.</param>
        private void tSearchTimer_TriggerSearch(object state)
        {
            TextChangedEventArgs e = (TextChangedEventArgs)state;

            Thread.Sleep(500);

            _iSystem.gSystem.invokeOnLocalThread((Action)(()=>
            {
                _iSystem.iLibSystem.lCurrentLibrary.sSearchString = this.Text;
            }));
        }

        /// <summary>
        /// Handles the eOnActiveLibraryChanged event of the iLibSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void iLibSystem_eOnActiveLibraryChanged(object sender, EventArgs e)
        {
            this.Text = _iSystem.iLibSystem.lCurrentLibrary.sSearchString;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public override void Dispose()
        {
            if (_eTextChanged != null)
                TextChanged -= _eTextChanged;

            if (_eOnActiveLibraryChanged != null)
                _iSystem.iLibSystem.eOnActiveLibraryChanged -= _eOnActiveLibraryChanged;

            base.Dispose();
        }
        #endregion
    }
}
