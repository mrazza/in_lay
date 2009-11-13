/*******************************************************************
 * This file is part of the netDiscographer library.
 * 
 * netDiscographer source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netDiscographer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netDiscographer.core
{
    /// <summary>
    /// This class represents a search request of any type
    /// </summary>
    public sealed class searchRequest
    {
        #region Members
        /// <summary>
        /// The type of search this request is
        /// </summary>
        private searchType _sType;

        /// <summary>
        /// ID of the playlist; used if playlist or dynamic search
        /// </summary>
        private int _iPlaylistID;

        /// <summary>
        /// Metadata field types to search
        /// </summary>
        private metaDataFieldTypes[] _mFieldTypes;

        /// <summary>
        /// Method used for searching
        /// </summary>
        private searchMethod _sSearchMethod;

        /// <summary>
        /// Search terms
        /// </summary>
        private string[] _sData;
        #endregion

        #region Properties
        /// <summary>
        /// Search Type
        /// </summary>
        public searchType sType
        {
            get
            {
                return _sType;
            }
            set
            {
                if (value == searchType.none)
                    throw new InvalidOperationException("You can not set the search type to none.");

                if (_sType == searchType.library)
                    throw new InvalidOperationException("Once the searchType has been set to library, you can not change it.");

                _sType = value;
            }
        }

        /// <summary>
        /// Playlist ID; if playlist or dynamic search
        /// </summary>
        public int iPlaylistID
        {
            get
            {
                return _iPlaylistID;
            }
        }

        /// <summary>
        /// Search method
        /// </summary>
        public searchMethod sSearchMethod
        {
            get
            {
                return _sSearchMethod;
            }
        }

        /// <summary>
        /// Search data
        /// </summary>
        public string[] sSearchData
        {
            get
            {
                return _sData;
            }
        }

        /// <summary>
        /// Metadata field types we're searching
        /// </summary>
        public metaDataFieldTypes[] mFieldTypes
        {
            get
            {
                return _mFieldTypes;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a library search instance that grabs the entire library.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        public searchRequest()
            : this(new string[0]) { }

        /// <summary>
        /// Creates a library search instance.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="sData">The search data.</param>
        public searchRequest(string[] sData)
            : this(sData, null, searchMethod.normal) { }

        /// <summary>
        /// Creates a library search instance.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="sData">The search data.</param>
        /// <param name="mFieldTypes">Metadata field types we are searching</param>
        /// <param name="sSearchMethod">The search method.</param>
        public searchRequest(string[] sData, metaDataFieldTypes[] mFieldTypes, searchMethod sSearchMethod)
            : this(searchType.library, 0, sData, mFieldTypes, sSearchMethod) { }

        /// <summary>
        /// Creates a playlist search instance that grabs the entire playlist.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="iPlaylistID">The playlist ID.</param>
        public searchRequest(int iPlaylistID)
            : this(iPlaylistID, new string[0], null, searchMethod.normal) { }

        /// <summary>
        /// Creates a playlist search instance.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="iPlaylistID">The playlist ID.</param>
        /// <param name="sData">The search data.</param>
        public searchRequest(int iPlaylistID, string[] sData)
            : this(iPlaylistID, sData, null, searchMethod.normal) { }

        /// <summary>
        /// Creates a playlist search instance.
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="iPlaylistID">The playlist ID.</param>
        /// <param name="sData">The search data.</param>
        /// <param name="mFieldTypes">Metadata field types we are searching</param>
        /// <param name="sSearchMethod">The search method.</param>
        public searchRequest(int iPlaylistID, string[] sData, metaDataFieldTypes[] mFieldTypes, searchMethod sSearchMethod)
            : this(searchType.playlist, iPlaylistID, sData, mFieldTypes, sSearchMethod) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="searchRequest"/> class.
        /// </summary>
        /// <param name="sType">Search Type</param>
        /// <param name="iPlaylistID">The playlist ID; if playlist search.</param>
        /// <param name="sData">The search data.</param>
        /// <param name="mFieldTypes">Metadata field types we are searching</param>
        /// <param name="sSearchMethod">The search method.</param>
        public searchRequest(searchType sType, int iPlaylistID, string[] sData, metaDataFieldTypes[] mFieldTypes, searchMethod sSearchMethod)
        {
            if (mFieldTypes != null && mFieldTypes.Length != sData.Length && mFieldTypes.Length != 0)
                throw new ArgumentException("sData and mFieldTypes must have the same number of entries.");

            _sType = sType;
            _iPlaylistID = iPlaylistID;
            _sSearchMethod = sSearchMethod;
            _sData = sData;
            _mFieldTypes = mFieldTypes;

            if (_mFieldTypes == null)
            {
                _mFieldTypes = new metaDataFieldTypes[sData.Length];
                for (int iLoop = 0; iLoop < _mFieldTypes.Length; iLoop++)
                    _mFieldTypes[iLoop] = metaDataFieldTypes.all;
            }
        }
        #endregion
    }
}
