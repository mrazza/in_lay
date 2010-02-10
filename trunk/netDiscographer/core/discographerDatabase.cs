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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Collections.Generic;

namespace netDiscographer.core
{
    /// <summary>
    /// Base class for all database systems
    /// </summary>
    public abstract class discographerDatabase : IDisposable
    {
        #region Members
        /// <summary>
        /// Memory Instance of the Media Library
        /// </summary>
        protected Dictionary<int, mediaEntry> _mLibrary;

        /// <summary>
        /// Path to the database
        /// </summary>
        protected string _sDataPath;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="sDataPath">Path to the database file</param>
        public discographerDatabase(string sDataPath)
        {
            _mLibrary = new Dictionary<int, mediaEntry>();
            _sDataPath = sDataPath;
            openDatabase();
            initFill();
        }
        #endregion

        #region Management Members
        #region Media Commands
        #region Insert Commands
        /// <summary>
        /// Adds new media to the library; if it already exists the entry is updated
        /// Updates/sets the ID entry in each mediaEntry
        /// </summary>
        /// <param name="mNewMedia">Media to add</param>
        public void addMedia(mediaEntry[] mNewMedia)
        {
            addMedia(mNewMedia, true);
        }

        /// <summary>
        /// Adds new media to the library
        /// Updates/sets the ID entry in each mediaEntry
        /// </summary>
        /// <param name="mNewMedia">Media to add</param>
        /// <param name="bUpdate">If true, the entry will be updated if it already exists, otherwise nothing will happen</param>
        public void addMedia(mediaEntry[] mNewMedia, bool bUpdate)
        {
            // Execute the implementation (update the database)
            addMedia_implemented(mNewMedia, bUpdate);

            // Update the local copy
            foreach (mediaEntry mCurr in mNewMedia)
            {
                if (_mLibrary.ContainsKey(mCurr.iEntryID))
                {
                    if (bUpdate)
                        _mLibrary[mCurr.iEntryID] = mCurr;
                }
                else
                    _mLibrary.Add(mCurr.iEntryID, mCurr);
            }
        }

        /// <summary>
        /// This is the implementation of addMedia: To be implemented by the database engine.
        /// Adds new media to the library
        /// Updates/sets the ID entry in each mediaEntry
        /// </summary>
        /// <param name="mNewMedia">Media to add</param>
        /// <param name="bUpdate">If true, the entry will be updated if it already exists, otherwise nothing will happen</param>
        protected abstract void addMedia_implemented(mediaEntry[] mNewMedia, bool bUpdate);
        #endregion

        #region Select Commands
        /// <summary>
        /// Returns all the media in the library
        /// </summary>
        /// <returns>Full library</returns>
        public mediaEntry[] searchMedia()
        {
            return searchMedia(new string[0], new metaDataFieldTypes[0], searchMethod.none);
        }

        /// <summary>
        /// Returns all the media in the library
        /// </summary>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="sSearchMethod">Search method</param>
        /// <returns>Entries returned</returns>
        public mediaEntry[] searchMedia(string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod)
        {
            if (sData.Length == 0 && mDataType.Length == 0)
            {
                mediaEntry[] mRet = new mediaEntry[_mLibrary.Count];
                _mLibrary.Values.CopyTo(mRet, 0);
                return mRet;
            }

            return getMediaData(searchMedia_implemented(sData, mDataType, sSearchMethod));
        }

        /// <summary>
        /// This is the implementation of searchMedia: To be implemented by the database engine.
        /// Returns all the media in the library
        /// </summary>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="sSearchMethod">Search method</param>
        /// <returns>Entries returned</returns>
        protected abstract HashSet<int> searchMedia_implemented(string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod);

        /// <summary>
        /// Executes a dynamic query
        /// </summary>
        /// <param name="dQuery">Dynamic Query to execute</param>
        /// <returns>Media Entries that match the query</returns>
        public mediaEntry[] searchDynamicQuery(dynamicQuery dQuery)
        {
            return searchDynamicQuery(dQuery, new string[0], new metaDataFieldTypes[0], searchMethod.none);
        }

        /// <summary>
        /// Executes then searches the results of a dynamic query
        /// </summary>
        /// <param name="dQuery">Query to execute</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="sSearchMethod">Search method</param>
        /// <returns>Entries returned</returns>
        public mediaEntry[] searchDynamicQuery(dynamicQuery dQuery, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod)
        {
            return getMediaData(searchDynamicQuery_implemented(dQuery, sData, mDataType, sSearchMethod));
        }

        /// <summary>
        /// This is the implementation of searchDynamicQuery: To be implemented by the database engine.
        /// Executes then searches the results of a dynamic query
        /// </summary>
        /// <param name="dQuery">Query to execute</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="sSearchMethod">Search method</param>
        /// <returns>Entries returned</returns>
        protected abstract HashSet<int> searchDynamicQuery_implemented(dynamicQuery dQuery, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod);
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes media from the library
        /// </summary>
        /// <param name="mMedia">Media Entry to remove</param>
        /// <returns>True if removed, otherwise false</returns>
        public bool removeMedia(mediaEntry mMedia)
        {
            return removeMedia(mMedia.iEntryID);
        }

        /// <summary>
        /// Removes media from the library
        /// </summary>
        /// <param name="mMedia">Media Entry to remove</param>
        /// <returns>True if all removed, otherwise false</returns>
        public bool removeMedia(mediaEntry[] mMedia)
        {
            int[] ids = new int[mMedia.Length];

            for (int iLoop = 0; iLoop < mMedia.Length; iLoop++)
                ids[iLoop] = mMedia[iLoop].iEntryID;

            return removeMedia(ids);
        }

        /// <summary>
        /// Removes media from the library
        /// </summary>
        /// <param name="iMediaID">ID of media to remove</param>
        /// <returns>True if removed, otherwise false</returns>
        public bool removeMedia(int iMediaID)
        {
            return removeMedia(new int[] { iMediaID });
        }

        /// <summary>
        /// Removes media from the library
        /// </summary>
        /// <param name="iMediaIDs">ID's of media to remove</param>
        /// <returns>True if all removed, otherwise false</returns>
        public bool removeMedia(int[] iMediaIDs)
        {
            bool bRet = removeMedia_implemented(iMediaIDs);

            // If it removed them
            if (bRet)
            {
                foreach (int iCurr in iMediaIDs)
                    _mLibrary.Remove(iCurr);
            }

            return bRet;
        }

        /// <summary>
        /// This is the implementation of removeMedia: To be implemented by the database engine.
        /// Removes media from the library
        /// </summary>
        /// <param name="iMediaIDs">ID's of media to remove</param>
        /// <returns>True if all removed, otherwise false</returns>
        protected abstract bool removeMedia_implemented(int[] iMediaIDs);
        #endregion
        #endregion

        #region Playlist Commands
        #region Standard Playlists
        #region Insert Commands
        /// <summary>
        /// Creates an empty new standard playlist entry in the database
        /// </summary>
        /// <param name="sPlaylistName">Name of the playlist</param>
        /// <returns>Playlist ID</returns>
        public virtual int createStandardPlaylist(string sPlaylistName)
        {
            return createStandardPlaylist(sPlaylistName, null, DateTime.Now);
        }

        /// <summary>
        /// Creates a new standard playlist entry in the database
        /// </summary>
        /// <param name="sPlaylistName">Name of the playlist</param>
        /// <param name="iMediaIDs">Media to add to the playlist; if null empty playlist</param>
        /// <returns>Playlist ID</returns>
        public virtual int createStandardPlaylist(string sPlaylistName, int[] iMediaIDs)
        {
            return createStandardPlaylist(sPlaylistName, iMediaIDs, DateTime.Now);
        }

        /// <summary>
        /// Creates a new standard playlist entry in the database
        /// </summary>
        /// <param name="sPlaylistName">Name of the playlist</param>
        /// <param name="iMediaIDs">Media to add to the playlist; if null empty playlist</param>
        /// <param name="dTimeCreated">Time the playlist was created</param>
        /// <returns>Playlist ID</returns>
        public abstract int createStandardPlaylist(string sPlaylistName, int[] iMediaIDs, DateTime dTimeCreated);

        /// <summary>
        /// Adds a track to the playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to add to</param>
        /// <param name="iTrackID">ID of the track to add</param>
        /// <param name="iTrackPos">Position to add the track to the playlist</param>
        public virtual void addTrackToPlaylist(int iPlaylistID, int iTrackID, int iTrackPos)
        {
            addTrackToPlaylist(iPlaylistID, new int[] { iTrackID }, new int[] { iTrackPos });
        }

        /// <summary>
        /// Adds an array of tracks to the playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to add to</param>
        /// <param name="iTrackIDs">ID of the tracks to add</param>
        /// <param name="iTrackPos">Position to add the tracks to the playlist</param>
        public abstract void addTrackToPlaylist(int iPlaylistID, int[] iTrackIDs, int[] iTrackPos);

        /// <summary>
        /// Moves track around in a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to make moves</param>
        /// <param name="iOldPos">Old position to move from</param>
        /// <param name="iNewPos">New position to move to</param>
        public virtual void moveTrackInPlaylist(int iPlaylistID, int iOldPos, int iNewPos)
        {
            moveTrackInPlaylist(iPlaylistID, new int[] { iOldPos }, new int[] { iNewPos });
        }

        /// <summary>
        /// Moves tracks around in a playlist
        /// It is important to note that changes take effect in order of position in the array.
        /// Old and new positions should account for changes of all previous moves in the array(s)
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to make moves</param>
        /// <param name="iOldPos">Old positions to move from</param>
        /// <param name="iNewPos">New positions to move to</param>
        public abstract void moveTrackInPlaylist(int iPlaylistID, int[] iOldPos, int[] iNewPos);

        /// <summary>
        /// Changes the name of a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="sName">New Playlist Name</param>
        public virtual void changeStandardPlaylistName(int iPlaylistID, string sName)
        {
            changeStandardPlaylistName(new int[] { iPlaylistID }, new string[] { sName });
        }

        /// <summary>
        /// Changes the name of a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="sName">New Playlist Name</param>
        public abstract void changeStandardPlaylistName(int[] iPlaylistID, string[] sName);

        /// <summary>
        /// Changes the time/date the playlist was created
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist</param>
        /// <param name="dTimeCreated">New DateTime that contains the new created date</param>
        public virtual void changeStandardPlaylistCreated(int iPlaylistID, DateTime dTimeCreated)
        {
            changeStandardPlaylistCreated(new int[] { iPlaylistID }, new DateTime[] { dTimeCreated });
        }

        /// <summary>
        /// Changes the time/date the playlist was created
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist</param>
        /// <param name="dTimeCreated">New DateTime that contains the new created date</param>
        public abstract void changeStandardPlaylistCreated(int[] iPlaylistID, DateTime[] dTimeCreated);
        #endregion

        #region Select Commands
        /// <summary>
        /// Gets all the playlist IDs that currently exist
        /// </summary>
        /// <returns>An array of playlist IDs</returns>
        public abstract int[] getStandardPlaylistIDs();

        /// <summary>
        /// Gets all playlist names
        /// </summary>
        /// <returns>Names of the playlists</returns>
        public virtual string[] getStandardPlaylistName()
        {
            return getStandardPlaylistName(null);
        }

        /// <summary>
        /// Gets the name of a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist to look up</param>
        /// <returns>Name of the playlist</returns>
        public virtual string getStandardPlaylistName(int iPlaylistID)
        {
            return getStandardPlaylistName(new int[] { iPlaylistID })[0];
        }

        /// <summary>
        /// Gets the name of an array of playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlists to look up; if null returns all names in the same order as getStandardPlaylistIDs() returns IDs</param>
        /// <returns>Names of the playlists</returns>
        public abstract string[] getStandardPlaylistName(int[] iPlaylistID);

        /// <summary>
        /// Returns the time each playlist was created
        /// </summary>
        /// <returns>All of the times created</returns>
        public virtual DateTime[] getStandardPlaylistCreated()
        {
            return getStandardPlaylistCreated(null);
        }

        /// <summary>
        /// Gets the time a playlist was created
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <returns>Time the playlist was created</returns>
        public virtual DateTime getStandardPlaylistCreated(int iPlaylistID)
        {
            return getStandardPlaylistCreated(new int[] { iPlaylistID })[0];
        }

        /// <summary>
        /// Returns the time each playlist was created
        /// </summary>
        /// <param name="iPlaylistID">Playlists to look up; if null returns all names in the same order as getStandardPlaylistIDs() returns IDs</param>
        /// <returns>All of the times created</returns>
        public abstract DateTime[] getStandardPlaylistCreated(int[] iPlaylistID);

        /// <summary>
        /// Returns all tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to get</param>
        /// <returns>Array of mediaEntries for each track in the playlist</returns>
        public mediaEntry[] searchStandardPlaylistMedia(int iPlaylistID)
        {
            return searchStandardPlaylistMedia(iPlaylistID, new string[0], new metaDataFieldTypes[0], searchMethod.none);
        }

        /// <summary>
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type); if none/all we search all types</param>
        /// <param name="sSearchMethod">Method to search with</param>
        /// <returns>Array of mediaEntries that match the search terms</returns>
        public mediaEntry[] searchStandardPlaylistMedia(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod)
        {
            return getMediaData(searchStandardPlaylistMedia_implemented(iPlaylistID, sData, mDataType, sSearchMethod));
        }


        /// <summary>
        /// This is the implementation of searchDynamicQuery: To be implemented by the database engine.
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type); if none/all we search all types</param>
        /// <param name="sSearchMethod">Method to search with</param>
        /// <returns>Array of media IDs that match the search terms</returns>
        protected abstract HashSet<int> searchStandardPlaylistMedia_implemented(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod);
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes a track from the playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to remove from</param>
        /// <param name="iTrackPos">Location of the track in the playlist to remove</param>
        public virtual void removeTrackFromPlaylist(int iPlaylistID, int iTrackPos)
        {
            removeTrackFromPlaylist(iPlaylistID, new int[] { iTrackPos });
        }

        /// <summary>
        /// Removes a tracks from the playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to remove from</param>
        /// <param name="iTrackPos">Location of the tracks in the playlist to remove</param>
        public abstract void removeTrackFromPlaylist(int iPlaylistID, int[] iTrackPos);

        /// <summary>
        /// Removes an entire playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to remove</param>
        public abstract void removeStandardPlaylist(int iPlaylistID);
        #endregion
        #endregion

        #region Dynamic Playlists
        #region Insert Commands
        /// <summary>
        /// Creates an empty new dynamic playlist with the current time and date as the created time
        /// </summary>
        /// <param name="sPlaylistName">Playlist Name</param>
        /// <returns>Playlist ID</returns>
        public virtual int createDynamicPlaylist(string sPlaylistName)
        {
            return createDynamicPlaylist(sPlaylistName, null, DateTime.Now);
        }

        /// <summary>
        /// Creates a new dynamic playlist with the current time and date as the created time
        /// </summary>
        /// <param name="sPlaylistName">Playlist Name</param>
        /// <param name="dQuery">Query for the playlist; if null none set</param>
        /// <returns>Playlist ID</returns>
        public virtual int createDynamicPlaylist(string sPlaylistName, dynamicQuery dQuery)
        {
            return createDynamicPlaylist(sPlaylistName, dQuery, DateTime.Now);
        }

        /// <summary>
        /// Creates a new dynamic playlist
        /// </summary>
        /// <param name="sPlaylistName">Playlist Name</param>
        /// <param name="dQuery">Query for the playlist; if null none set</param>
        /// <param name="dTimeCreated">Time Created</param>
        /// <returns>Playlist ID</returns>
        public abstract int createDynamicPlaylist(string sPlaylistName, dynamicQuery dQuery, DateTime dTimeCreated);

        /// <summary>
        /// Changes the name associated with a dynamic playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist with the name that needs changin'</param>
        /// <param name="sNewName">New name for the playlist</param>
        public virtual void changeDynamicPlaylistName(int iPlaylistID, string sNewName)
        {
            changeDynamicPlaylistName(new int[] { iPlaylistID }, new string[] { sNewName });
        }

        /// <summary>
        /// Changes the name associated with an array of dynamic playlists
        /// </summary>
        /// <param name="iPlaylistIDs">IDs of playlists with names that need changin'</param>
        /// <param name="sNewNames">New names for each playlist</param>
        public abstract void changeDynamicPlaylistName(int[] iPlaylistIDs, string[] sNewNames);

        /// <summary>
        /// Changes the date and time a playlist was created
        /// </summary>
        /// <param name="iPlaylistIDs">ID of the playlist to be changed</param>
        /// <param name="dTimeCreated">New time created</param>
        public virtual void changeDynamicPlaylistsCreated(int iPlaylistIDs, DateTime dTimeCreated)
        {
            changeDynamicPlaylistCreated(new int[] { iPlaylistIDs }, new DateTime[] { dTimeCreated });
        }

        /// <summary>
        /// Changes the date and time of an array of playlists
        /// </summary>
        /// <param name="iPlaylistIDs">IDs of the playlists to change</param>
        /// <param name="dTimeCreated">New times to be set</param>
        public abstract void changeDynamicPlaylistCreated(int[] iPlaylistIDs, DateTime[] dTimeCreated);

        /// <summary>
        /// Sets the dynamic playist query for a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="dQuery">Query</param>
        public virtual void changeDynamicPlaylistQuery(int iPlaylistID, dynamicQuery dQuery)
        {
            changeDynamicPlaylistQuery(new int[] { iPlaylistID }, new dynamicQuery[] { dQuery });
        }

        /// <summary>
        /// Sets the dynamic playlist for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs</param>
        /// <param name="dQuery">Queries</param>
        public abstract void changeDynamicPlaylistQuery(int[] iPlaylistID, dynamicQuery[] dQuery);
        #endregion

        #region Select Commands
        /// <summary>
        /// Get all the dynamic playlist IDs
        /// </summary>
        /// <returns>All IDs</returns>
        public abstract int[] getDynamicPlaylistIDs();

        /// <summary>
        /// Gets the name of all the playlists
        /// </summary>
        /// <returns>Name of all the playlists</returns>
        public virtual string[] getDynamicPlaylistName()
        {
            return getDynamicPlaylistName(null);
        }

        /// <summary>
        /// Gets a specific playlist's name
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to get the name for</param>
        /// <returns>Playlist Name</returns>
        public virtual string getDynamicPlaylistName(int iPlaylistID)
        {
            return getDynamicPlaylistName(new int[] { iPlaylistID })[0];
        }

        /// <summary>
        /// Gets a bunch of playlist names
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up; if null returns all names in the same order as getDynamicPlaylistIDs() returns IDs</param>
        /// <returns>Names of the playlists</returns>
        public abstract string[] getDynamicPlaylistName(int[] iPlaylistID);

        /// <summary>
        /// Gets datetime created for all dynamic playlists
        /// </summary>
        /// <returns>Time all playlists were created</returns>
        public virtual DateTime[] getDynamicPlaylistCreated()
        {
            return getDynamicPlaylistCreated(null);
        }

        /// <summary>
        /// Gets datetime created for a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <returns>Time playlist was created</returns>
        public virtual DateTime getDynamicPlaylistCreated(int iPlaylistID)
        {
            return getDynamicPlaylistCreated(new int[] { iPlaylistID })[0];
        }

        /// <summary>
        /// Gets datetime created for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up,</param>
        /// <returns></returns>
        public abstract DateTime[] getDynamicPlaylistCreated(int[] iPlaylistID);

        /// <summary>
        /// Gets all the dynamic playlist query for an array of playlists
        /// </summary>
        /// <returns>Queries</returns>
        public virtual dynamicQuery[] getDynamicPlaylistQuery()
        {
            return getDynamicPlaylistQuery(null);
        }

        /// <summary>
        /// Gets the query for a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <returns>Query</returns>
        public virtual dynamicQuery getDynamicPlaylistQuery(int iPlaylistID)
        {
            return getDynamicPlaylistQuery(new int[] { iPlaylistID })[0];
        }

        /// <summary>
        /// Gets the dynamic playlist query for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up; if null returns all names in the same order as getDynamicPlaylistIDs() returns IDs</param>
        /// <returns>Queries</returns>
        public abstract dynamicQuery[] getDynamicPlaylistQuery(int[] iPlaylistID);

        /// <summary>
        /// Returns all tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to get</param>
        /// <returns>Array of mediaEntries for each track in the playlist</returns>
        public mediaEntry[] searchDynamicPlaylistMedia(int iPlaylistID)
        {
            return searchDynamicPlaylistMedia(iPlaylistID, new string[0], new metaDataFieldTypes[0], searchMethod.none);
        }

        /// <summary>
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type); if none/all we search all types</param>
        /// <param name="sSearchMethod">Method to search with</param>
        /// <returns>Array of mediaEntries that match the search terms</returns>
        public mediaEntry[] searchDynamicPlaylistMedia(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod)
        {
            return getMediaData(searchDynamicPlaylistMedia_implemented(iPlaylistID, sData, mDataType, sSearchMethod));
        }

        /// <summary>
        /// This is the implementation of searchDynamicPlaylistMedia: To be implemented by the database engine.
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type); if none/all we search all types</param>
        /// <param name="sSearchMethod">Method to search with</param>
        /// <returns>Array of mediaEntries that match the search terms</returns>
        protected abstract HashSet<int> searchDynamicPlaylistMedia_implemented(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchMethod sSearchMethod);
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes a dynamic playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to remove</param>
        public abstract void removeDynamicPlaylist(int iPlaylistID);
        #endregion
        #endregion
        #endregion

        #region Global Commands
        /// <summary>
        /// Searches the database via a searchRequest
        /// </summary>
        /// <param name="sRequest">What to search</param>
        /// <returns>Resulting entries</returns>
        public mediaEntry[] searchDatabase(searchRequest sRequest)
        {
            switch (sRequest.sType)
            {
                case searchType.library:
                    if (sRequest.sSearchData.Length == 0)
                        return searchMedia();

                    return searchMedia(sRequest.sSearchData, sRequest.mFieldTypes, sRequest.sSearchMethod);
                
                case searchType.playlist:
                    if (sRequest.sSearchData.Length == 0)
                        return searchStandardPlaylistMedia(sRequest.iPlaylistID);

                    return searchStandardPlaylistMedia(sRequest.iPlaylistID, sRequest.sSearchData, sRequest.mFieldTypes, sRequest.sSearchMethod);

                case searchType.dynamic:
                    if (sRequest.sSearchData.Length == 0)
                        return searchDynamicPlaylistMedia(sRequest.iPlaylistID);

                    return searchDynamicPlaylistMedia(sRequest.iPlaylistID, sRequest.sSearchData, sRequest.mFieldTypes, sRequest.sSearchMethod); 
            }

            throw new ArgumentException("searchType for database query is not valid.");
        }
        #endregion

        /// <summary>
        /// Formats a string to fit the searchability requirement
        /// </summary>
        /// <param name="sData">Data to make searchable</param>
        /// <returns>Searchable version string</returns>
        internal static string makeSearchable(string sData)
        {
            sData = sData.ToLower();
            sData = sData.Trim();

            return sData;
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// Performs the initial fill of the memory dictionary
        /// </summary>
        protected abstract void initFill();

        /// <summary>
        /// Opens the database
        /// </summary>
        /// <returns>True if worked; otherwise false</returns>
        protected abstract bool openDatabase();
        #endregion

        #region Private Members
        /// <summary>
        /// Gets the media data for a set of tracks
        /// </summary>
        /// <param name="iMediaIDs">Media Data to Get</param>
        /// <returns>The media data requested</returns>
        private mediaEntry[] getMediaData(HashSet<int> iMediaIDs)
        {
            mediaEntry[] mMediaEntries = new mediaEntry[iMediaIDs.Count];
            int iLoop = 0;

            foreach (int iCurr in iMediaIDs)
            {
                mMediaEntries[iLoop] = _mLibrary[iCurr];
                iLoop++;
            }

            return mMediaEntries;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes of any allocated resources
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}
