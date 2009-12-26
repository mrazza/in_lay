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
        /// List of library instances
        /// </summary>
        private List<libraryInstance> _lLibraryInstances;

        /// <summary>
        /// inlayComponenetSystem we are a member of
        /// </summary>
        private inlayComponentSystem _iComponentSystem;
        #endregion

        #region Events
        #region eOnLibraryChanged
        /// <summary>
        /// Triggered when the active library has changed
        /// </summary>
        private event EventHandler _eOnLibraryChanged;

        /// <summary>
        /// Triggered when the active library has changed
        /// </summary>
        public event EventHandler eOnLibraryChanged
        {
            add
            {
                _eOnLibraryChanged += value;
            }
            remove
            {
                if (_eOnLibraryChanged != null)
                    _eOnLibraryChanged -= value;
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
                if (value >= _lLibraryInstances.Count || value < -1)
                    return;

                _iCurrentLibrary = value;
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
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Triggers related events when the current library has changed
        /// </summary>
        private void currentLibraryChanged()
        {
            if (_eOnLibraryChanged != null)
                _eOnLibraryChanged.Invoke(this, new EventArgs());

            if (_eOnMediaChanged != null)
                _eOnMediaChanged.Invoke(this, new EventArgs());
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
