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
using System.Collections.Generic;
using netAudio.core;
using netDiscographer.core;
using netGooey.core;

namespace inlayShared.core
{
    /// <summary>
    /// Manages the inlay Library Display Systems
    /// </summary>
    public class inlayLibrarySystem
    {
        #region Members
        /// <summary>
        /// Currently selected library index; -1 if none selected
        /// </summary>
        private int _iCurrentLibrary;

        /// <summary>
        /// Currently library with playback focus; -1 if nothing has playback focus
        /// </summary>
        private int _iCurrentPlaybackLibrary;

        /// <summary>
        /// List of library instances
        /// </summary>
        private List<libraryInstance> _lLibraryInstances;

        /// <summary>
        /// inlayComponenetSystem we are a member of
        /// </summary>
        private inlayComponentSystem _iComponentSystem;
        #endregion

        #region Events
        #region eOnActiveLibraryChanged
        /// <summary>
        /// Triggered when the active library has changed
        /// </summary>
        private event EventHandler _eOnActiveLibraryChanged;

        /// <summary>
        /// Triggered when the active library has changed
        /// </summary>
        public event EventHandler eOnActiveLibraryChanged
        {
            add
            {
                _eOnActiveLibraryChanged += value;
            }
            remove
            {
                if (_eOnActiveLibraryChanged != null)
                    _eOnActiveLibraryChanged -= value;
            }
        }
        #endregion

        #region eOnMediaChanged
        /// <summary>
        /// Triggered when the media array containing the meta data to display has changed
        /// </summary>
        private event EventHandler _eOnMediaChanged;

        /// <summary>
        /// Triggered when the media array containing the meta data to display has changed
        /// </summary>
        public event EventHandler eOnMediaChanged
        {
            add
            {
                _eOnMediaChanged += value;
            }
            remove
            {
                if (_eOnMediaChanged != null)
                    _eOnMediaChanged -= value;
            }
        }
        #endregion

        #region eOnLibrariesChanged
        /// <summary>
        /// Triggered when the list of available libraries changes
        /// </summary>
        private event EventHandler _eOnLibrariesChanged;

        /// <summary>
        /// Triggered when the list of available libraries changes
        /// </summary>
        public event EventHandler eOnLibrariesChanged
        {
            add
            {
                _eOnLibrariesChanged += value;
            }
            remove
            {
                if (_eOnLibrariesChanged != null)
                    _eOnLibrariesChanged -= value;
            }
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Index of the current library
        /// </summary>
        public int iCurrentLibrary
        {
            get
            {
                return _iCurrentLibrary;
            }
            set
            {
                if (_iCurrentLibrary == value)
                    return;

                if (value >= getLibraryCount() || value < -1)
                    return;

                _iCurrentLibrary = value;
                currentLibraryChanged();
            }
        }

        /// <summary>
        /// Index of the library that currently has playback focus; -1 if none
        /// </summary>
        public int iCurrentPlaybackLibrary
        {
            get
            {
                return _iCurrentPlaybackLibrary;
            }
            set
            {
                if (_iCurrentPlaybackLibrary == value)
                    return;

                if (value >= getLibraryCount() || value < -1)
                    return;

                _iCurrentPlaybackLibrary = value;
            }
        }

        /// <summary>
        /// Current Library Instance
        /// </summary>
        public libraryInstance lCurrentLibrary
        {
            get
            {
                return getLibrary(_iCurrentLibrary);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="inlayLibrarySystem"/> class.
        /// </summary>
        /// <param name="iParent">The parent.</param>
        public inlayLibrarySystem(inlayComponentSystem iParent)
        {
            if (iParent == null)
                throw new ArgumentNullException("The inlayComponentSystem (parent) argument can not be null. Could not create instance.");

            _iComponentSystem = iParent;
            _lLibraryInstances = new List<libraryInstance>();
            _iCurrentLibrary = -1;
            _iCurrentPlaybackLibrary = -1;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Gets a library.
        /// </summary>
        /// <param name="iID">The ID of the library.</param>
        /// <returns>Library instance of the ID</returns>
        public libraryInstance getLibrary(int iID)
        {
            if (iID < 0 || iID >= _lLibraryInstances.Count)
                return null;

            return _lLibraryInstances[iID];
        }

        /// <summary>
        /// Gets the library count.
        /// </summary>
        /// <returns>Number of libraries we have</returns>
        public int getLibraryCount()
        {
            return _lLibraryInstances.Count;
        }

        /// <summary>
        /// Adds a library.
        /// </summary>
        /// <param name="lNewLibrary">The new library.</param>
        public void addLibrary(libraryInstance lNewLibrary)
        {
            lNewLibrary.eOnMediaChanged += new EventHandler(lLibraries_eOnMediaChanged);
            _lLibraryInstances.Add(lNewLibrary);
            librariesChanged();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Triggers related events when the current library has changed
        /// </summary>
        private void currentLibraryChanged()
        {
            if (_eOnActiveLibraryChanged != null)
                _eOnActiveLibraryChanged.Invoke(this, new EventArgs());

            if (_eOnMediaChanged != null)
                _eOnMediaChanged.Invoke(this, new EventArgs());
        }
        
        /// <summary>
        /// Triggeres related events when the list of libraries changes
        /// </summary>
        private void librariesChanged()
        {
            if (_eOnLibrariesChanged != null)
                _eOnLibrariesChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Handles the eOnMediaChanged event of the lLibraries control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lLibraries_eOnMediaChanged(object sender, EventArgs e)
        {
            if (sender == lCurrentLibrary && _eOnMediaChanged != null)
                _eOnMediaChanged.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
