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
using netDiscographer.core;
using netDiscographer.core.events;

namespace inlayShared.core
{
    /// <summary>
    /// Represents an instance of a library window
    /// </summary>
    public class libraryInstance
    {
        #region Members
        /// <summary>
        /// Type of library instance
        /// </summary>
        private searchType _sLibraryType;

        /// <summary>
        /// If a playlist; it's ID
        /// </summary>
        private int _iPlaylistID;

        /// <summary>
        /// String currently entered into the search box
        /// </summary>
        private string _sCurrentSearchString;

        /// <summary>
        /// Event Handler for onSearchComplete
        /// </summary>
        private EventHandler<onSearchCompleteEventArgs> _eOnSearchComplete;

        /// <summary>
        /// Current media entries that should be displayed in the library
        /// </summary>
        private mediaEntry[] _mCurrentMedia;

        /// <summary>
        /// Time it took to complete the search
        /// </summary>
        private TimeSpan _tSearchTime;

        /// <summary>
        /// Root inlay component system
        /// </summary>
        private inlayComponentSystem _iComponentSystem;

        /// <summary>
        /// Current scroll position of the library
        /// </summary>
        private int _iScrollPosition;

        /// <summary>
        /// Currently selected song
        /// </summary>
        private int _iSelectedSong;
        #endregion

        #region Events
        /// <summary>
        /// Triggered when the media array has changed
        /// </summary>
        private event EventHandler _eOnMediaChanged;

        /// <summary>
        /// Triggered when the media array has changed
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

        #region Properties
        /// <summary>
        /// Gets the type of the library.
        /// </summary>
        /// <value>The type of the library.</value>
        public searchType sLibraryType
        {
            get
            {
                return _sLibraryType;
            }
        }

        /// <summary>
        /// Gets the playlist ID.
        /// </summary>
        /// <value>The playlist ID.</value>
        public int iPlaylistID
        {
            get
            {
                return _iPlaylistID;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [the instance is a library].
        /// </summary>
        /// <value><c>true</c> if [the instance is a library]; otherwise, <c>false</c>.</value>
        public bool bIsLibrary
        {
            get
            {
                if (_sLibraryType == searchType.library)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        /// <value>The search string.</value>
        public string sSearchString
        {
            get
            {
                return _sCurrentSearchString;
            }
            set
            {
                if (_sCurrentSearchString.Equals(value))
                    return;

                _sCurrentSearchString = value;
                onSearchStringChanged();
            }
        }

        /// <summary>
        /// The current array of media data
        /// </summary>
        public mediaEntry[] mCurrentMedia
        {
            get
            {
                return _mCurrentMedia;
            }
        }

        /// <summary>
        /// Current scroll position of the library
        /// </summary>
        public int iScrollPosition
        {
            get
            {
                return _iScrollPosition;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Currently selected song
        /// </summary>
        public int iSelectedSong
        {
            get
            {
                return _iSelectedSong;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="libraryInstance"/> class.
        /// </summary>
        /// <param name="iComponentSystem">The inlay component system.</param>
        public libraryInstance(inlayComponentSystem iComponentSystem)
            : this(iComponentSystem, searchType.library, -1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="libraryInstance"/> class.
        /// </summary>
        /// <param name="iComponentSystem">The inlay component system.</param>
        /// <param name="sLibraryType">Type of the library.</param>
        /// <param name="iPlaylistID">The playlist ID.</param>
        public libraryInstance(inlayComponentSystem iComponentSystem, searchType sLibraryType, int iPlaylistID)
        {
            _iComponentSystem = iComponentSystem;
            _sLibraryType = sLibraryType;
            _iPlaylistID = iPlaylistID;

            _sCurrentSearchString = "";
            _eOnSearchComplete = new EventHandler<onSearchCompleteEventArgs>(dSystem_eSearchComplete);
            _tSearchTime = TimeSpan.Zero;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Sets the active library to the full library
        /// </summary>
        public void setLibrary()
        {
            setLibrary(searchType.library, -1);
        }

        /// <summary>
        /// Sets the active library to a given playlist.
        /// </summary>
        /// <param name="sLibraryType">Type of playlist.</param>
        /// <param name="iPlaylistID">The playlist ID.</param>
        public void setLibrary(searchType sLibraryType, int iPlaylistID)
        {
            _sLibraryType = sLibraryType;
            _iPlaylistID = iPlaylistID;
        }

        /// <summary>
        /// Triggers a refresh of the library from the database
        /// </summary>
        public void refresh()
        {
            onSearchStringChanged();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Called when the search string is changed
        /// </summary>
        private void onSearchStringChanged()
        {
            _iComponentSystem.dSystem.asyncSearchDatabase(new searchRequest(_sLibraryType, _iPlaylistID, _sCurrentSearchString.Trim().Split(' '), null, searchMethod.normal), _eOnSearchComplete);
        }

        /// <summary>
        /// Handles the eSearchComplete event of the dSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="netDiscographer.core.events.onSearchCompleteEventArgs"/> instance containing the event data.</param>
        private void dSystem_eSearchComplete(object sender, onSearchCompleteEventArgs e)
        {
            _mCurrentMedia = e.mEntries;
            _tSearchTime = e.tSearchTime;

            onMediaChanged();
        }

        /// <summary>
        /// Call when the media changed.
        /// </summary>
        private void onMediaChanged()
        {
            if (_eOnMediaChanged != null)
                _eOnMediaChanged.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
