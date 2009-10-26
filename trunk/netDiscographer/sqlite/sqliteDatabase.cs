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
using System.Text;
using System.Collections.Generic;
using System.Data.SQLite;
using netDiscographer.core;
using netDiscographer.core.dynamicQueryCore;

namespace netDiscographer.sqlite
{
    /// <summary>
    /// Manages the SQLite Database: Creates the tables and provides query/transaction abilities
    /// </summary>
    public class sqliteDatabase : discographerDatabase
    {
        #region Members
        /// <summary>
        /// Connection to the SQLite database
        /// </summary>
        private SQLiteConnection _cSQLConnection;
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="sDataPath">Path to the database file</param>
        public sqliteDatabase(string sDataPath)
            : base(sDataPath) { }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~sqliteDatabase()
        {
            Dispose();
        }
        #endregion

        #region Management Members
        #region Media Commands
        #region Insert Commands
        /// <summary>
        /// Adds new media to the library
        /// Updates/sets the ID entry in each mediaEntry
        /// </summary>
        /// <param name="mNewMedia">Media to add</param>
        /// <param name="bUpdate">If true, the entry will be updated if it already exists, otherwise nothing will happen</param>
        protected override void addMedia_implemented(mediaEntry[] mNewMedia, bool bUpdate)
        {
            using (SQLiteCommand cMediaItemInsertCmd = _cSQLConnection.CreateCommand(),
                cMediaDataInsertCmd = _cSQLConnection.CreateCommand(),
                cMediaItemUpdateCmd = _cSQLConnection.CreateCommand(),
                cMediaDataUpdateCmd = _cSQLConnection.CreateCommand(),
                cLastInsertedCmd = _cSQLConnection.CreateCommand(),
                cDeleteMediaDataCmd = _cSQLConnection.CreateCommand())
            {
                // Set up the commands
                cMediaItemInsertCmd.CommandText = "INSERT INTO media_items (item_uri, item_length, time_added) VALUES (@uri, @length, @time);";
                cMediaDataInsertCmd.CommandText = "INSERT INTO media_data (media_item_id, media_data_type, media_data, media_data_searchable) VALUES (@itemID, @typeID, @data, @dataSearchable);";
                cMediaItemUpdateCmd.CommandText = "UPDATE media_items SET item_length=@length, time_added=@time WHERE media_item_id=@id;";
                cMediaDataUpdateCmd.CommandText = "UPDATE media_data SET media_data=@data, media_data_searchable=@searchableData WHERE media_item_id=@id AND media_data_type=@type;";
                cLastInsertedCmd.CommandText = "SELECT last_insert_rowid() as id;";
                cDeleteMediaDataCmd.CommandText = "DELETE FROM media_data WHERE media_item_id=@itemID AND media_data_type=@typeID;";

                // Establish the variables
                cMediaItemInsertCmd.Parameters.AddWithValue("@uri", null);
                cMediaItemInsertCmd.Parameters.AddWithValue("@length", null);
                cMediaItemInsertCmd.Parameters.AddWithValue("@time", null);

                cMediaDataInsertCmd.Parameters.AddWithValue("@itemID", null);
                cMediaDataInsertCmd.Parameters.AddWithValue("@typeID", null);
                cMediaDataInsertCmd.Parameters.AddWithValue("@data", null);
                cMediaDataInsertCmd.Parameters.AddWithValue("@dataSearchable", null);

                cMediaItemUpdateCmd.Parameters.AddWithValue("@id", null);
                cMediaItemUpdateCmd.Parameters.AddWithValue("@length", null);
                cMediaItemUpdateCmd.Parameters.AddWithValue("@time", null);

                cMediaDataUpdateCmd.Parameters.AddWithValue("@id", null);
                cMediaDataUpdateCmd.Parameters.AddWithValue("@type", null);
                cMediaDataUpdateCmd.Parameters.AddWithValue("@data", null);
                cMediaDataUpdateCmd.Parameters.AddWithValue("@searchableData", null);

                cDeleteMediaDataCmd.Parameters.AddWithValue("@itemID", null);
                cDeleteMediaDataCmd.Parameters.AddWithValue("@typeID", null);

                // Start the transaction
                using (SQLiteTransaction tSQLTransaction = _cSQLConnection.BeginTransaction())
                {
                    // Loop through each entry
                    SQLiteDataReader drDataReader;
                    metaDataFieldTypes mFieldType;
                    string sFieldData;
                    int iTempID;
                    for (int iLoop = 0; iLoop < mNewMedia.Length; iLoop++)
                    {
                        // Add/Update the item entry
                        if (bUpdate && (iTempID = itemRowID(mNewMedia[iLoop].sPath)) != -1)
                        {
                            cMediaItemUpdateCmd.Parameters[0].Value = iTempID;
                            cMediaItemUpdateCmd.Parameters[1].Value = mNewMedia[iLoop].tLength.Ticks;
                            cMediaItemUpdateCmd.Parameters[2].Value = mNewMedia[iLoop].dTimeAdded.Ticks;

                            mNewMedia[iLoop].iEntryID = iTempID;
                        }
                        else
                        {
                            cMediaItemInsertCmd.Parameters[0].Value = mNewMedia[iLoop].sPath;
                            cMediaItemInsertCmd.Parameters[1].Value = mNewMedia[iLoop].tLength.Ticks;
                            cMediaItemInsertCmd.Parameters[2].Value = mNewMedia[iLoop].dTimeAdded.Ticks;
                            cMediaItemInsertCmd.ExecuteNonQuery();

                            // Get the item ID
                            drDataReader = cLastInsertedCmd.ExecuteReader();

                            if (!drDataReader.Read())
                                continue;

                            mNewMedia[iLoop].iEntryID = drDataReader.GetInt32(0);
                            drDataReader.Close();
                        }

                        // Set constant data
                        cMediaDataInsertCmd.Parameters[0].Value = mNewMedia[iLoop].iEntryID;
                        cMediaDataUpdateCmd.Parameters[0].Value = mNewMedia[iLoop].iEntryID;
                        cDeleteMediaDataCmd.Parameters[0].Value = mNewMedia[iLoop].iEntryID;
                        bool bRowExists = false;

                        // Fill in the rest of the data
                        for (ushort iLoop2 = 1; iLoop2 <= (ushort)metaDataFieldTypes.last; iLoop2 *= 2)
                        {
                            mFieldType = (metaDataFieldTypes)iLoop2;
                            sFieldData = mNewMedia[iLoop][mFieldType];
                            
                            if (bUpdate)
                                bRowExists = dataRowExists(mNewMedia[iLoop].iEntryID, (metaDataFieldTypes)iLoop2);

                            // If no data make sure the row does not exist
                            if (!String.IsNullOrEmpty(sFieldData)) //We have data
                            {
                                if (bRowExists)
                                {
                                    cMediaDataUpdateCmd.Parameters[1].Value = (int)mFieldType;
                                    cMediaDataUpdateCmd.Parameters[2].Value = sFieldData;
                                    cMediaDataUpdateCmd.Parameters[3].Value = makeSearchable(sFieldData);
                                    cMediaDataUpdateCmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cMediaDataInsertCmd.Parameters[1].Value = (int)mFieldType;
                                    cMediaDataInsertCmd.Parameters[2].Value = sFieldData;
                                    cMediaDataInsertCmd.Parameters[3].Value = makeSearchable(sFieldData);
                                    cMediaDataInsertCmd.ExecuteNonQuery();
                                }
                            }
                            else if (bRowExists) //Null or empty string and the row exists, remove it
                            {
                                cDeleteMediaDataCmd.Parameters[1].Value = (int)mFieldType;
                                cDeleteMediaDataCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    tSQLTransaction.Commit();
                }
            }
        }
        #endregion

        #region Select Commands
        /// <summary>
        /// Returns all the media in the library
        /// </summary>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if null we'll search ALL datatypes</param>
        /// <param name="stSearchType">Search method</param>
        /// <returns>Entries returned</returns>
        protected override HashSet<int> searchMedia_implemented(string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            HashSet<int> iRet;
            using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
            {
                iRet = searchDatabase(sData, mDataType, stSearchType);
                tSQLTrans.Commit();
            }

            return iRet;
        }

        /// <summary>
        /// Executes then searches the results of a dynamic query
        /// </summary>
        /// <param name="dQuery">Query to execute</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="stSearchType">Search method</param>
        /// <returns>Entries returned</returns>
        protected override HashSet<int> searchDynamicQuery_implemented(dynamicQuery dQuery, string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            HashSet<int> iRet;
            using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
            {
                iRet = executeDynamicQuery(dQuery);

                if (sData.Length > 0)
                    iRet.IntersectWith(searchDatabase(sData, mDataType, stSearchType));

                tSQLTrans.Commit();
            }

            return iRet;
        }
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes media from the library
        /// </summary>
        /// <param name="iMediaIDs">ID's of media to remove</param>
        /// <returns>True if all removed, otherwise false</returns>
        protected override bool removeMedia_implemented(int[] iMediaIDs)
        {
            bool bWorked = true;

            using (SQLiteCommand cDeleteItemsCmd = _cSQLConnection.CreateCommand(),
                cDeleteDataCmd = _cSQLConnection.CreateCommand())
            {
                cDeleteItemsCmd.CommandText = "DELETE FROM media_items WHERE media_item_id=@id;";
                cDeleteDataCmd.CommandText = "DELETE FROM media_data WHERE media_item_id=@id;";

                cDeleteItemsCmd.Parameters.AddWithValue("@id", null);
                cDeleteDataCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction cSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    foreach (int iCurr in iMediaIDs)
                    {
                        cDeleteItemsCmd.Parameters[0].Value = iCurr;
                        cDeleteDataCmd.Parameters[0].Value = iCurr;

                        if (cDeleteItemsCmd.ExecuteNonQuery() < 1)
                            bWorked = false;

                        if (cDeleteDataCmd.ExecuteNonQuery() < 1)
                            bWorked = false;
                    }

                    if (!bWorked)
                        cSQLTrans.Rollback();
                    else
                        cSQLTrans.Commit();
                }
            }

            return bWorked;
        }
        #endregion
        #endregion

        #region Playlist Commands
        #region Standard Playlists
        #region Insert Commands
        /// <summary>
        /// Creates a new standard playlist entry in the database
        /// </summary>
        /// <param name="sPlaylistName">Name of the playlist</param>
        /// <param name="iMediaIDs">Media to add to the playlist; if null empty playlist</param>
        /// <param name="dTimeCreated">Time the playlist was created</param>
        /// <returns>Playlist ID; -1 if failed</returns>
        public override int createStandardPlaylist(string sPlaylistName, int[] iMediaIDs, DateTime dTimeCreated)
        {
            using (SQLiteCommand cCreatePlaylistCmd = _cSQLConnection.CreateCommand(),
                cAddTrackCmd = _cSQLConnection.CreateCommand(),
                cLastInsertedCmd = _cSQLConnection.CreateCommand())
            {
                // Setup the commands
                cCreatePlaylistCmd.CommandText = "INSERT INTO playlist_standard (playlist_name, time_created) VALUES (@name, @time);";
                cAddTrackCmd.CommandText = "INSERT INTO playlist_standard_tracks (playlist_id, track_number, media_item_id) VALUES (@playlistID, @trackNum, @mediaID);";
                cLastInsertedCmd.CommandText = "SELECT last_insert_rowid() as id;";

                cCreatePlaylistCmd.Parameters.AddWithValue("@name", sPlaylistName);
                cCreatePlaylistCmd.Parameters.AddWithValue("@time", dTimeCreated.Ticks);

                cAddTrackCmd.Parameters.AddWithValue("@playlistID", null);
                cAddTrackCmd.Parameters.AddWithValue("@trackNum", null);
                cAddTrackCmd.Parameters.AddWithValue("@mediaID", null);

                // Let's do the SQL!
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    int iPlaylistID;
                    cCreatePlaylistCmd.ExecuteNonQuery(); //Creates the playlist

                    // Let's get it's ID
                    SQLiteDataReader dataReader = cLastInsertedCmd.ExecuteReader();
                    if (!dataReader.Read())
                    {
                        tSQLTrans.Rollback();
                        return -1;
                    }

                    iPlaylistID = dataReader.GetInt32(0);
                    dataReader.Close();

                    if (iMediaIDs != null)
                    {
                        cAddTrackCmd.Parameters[0].Value = iPlaylistID; //Save the ID

                        for (int iLoop = 0; iLoop < iMediaIDs.Length; iLoop++)
                        {
                            cAddTrackCmd.Parameters[1].Value = iLoop;
                            cAddTrackCmd.Parameters[2].Value = iMediaIDs[iLoop];
                            cAddTrackCmd.ExecuteNonQuery();
                        }
                    }

                    tSQLTrans.Commit();

                    return iPlaylistID;
                }
            }
        }

        /// <summary>
        /// Adds an array of tracks to the playlist
        /// It is important to note that changes take effect in order of position in the array.
        /// Adding a new track in the middle forces all tracks below it to bump up to a higher ID, make sure you accont for this
        /// when passing in iTrackPos
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to add to</param>
        /// <param name="iTrackIDs">ID of the tracks to add</param>
        /// <param name="iTrackPos">Position to add the tracks to the playlist</param>
        public override void addTrackToPlaylist(int iPlaylistID, int[] iTrackIDs, int[] iTrackPos)
        {
            if (iTrackIDs.Length != iTrackPos.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cAddTrackCmd = _cSQLConnection.CreateCommand(),
                cUpdateTrackIDs = _cSQLConnection.CreateCommand())
            {
                // Set up the commands
                cAddTrackCmd.CommandText = "INSERT INTO playlist_standard_tracks (playlist_id, track_number, media_item_id) VALUES (@playlistID, @trackNum, @mediaID);";
                cUpdateTrackIDs.CommandText = "UPDATE playlist_standard_tracks SET track_number=track_number+1 WHERE playlist_id=@playlisID AND track_number>=@trackNum;";

                cAddTrackCmd.Parameters.AddWithValue("@playlistID", iPlaylistID);
                cAddTrackCmd.Parameters.AddWithValue("@trackNum", null);
                cAddTrackCmd.Parameters.AddWithValue("@mediaID", null);

                cUpdateTrackIDs.Parameters.AddWithValue("@playlisID", iPlaylistID);
                cUpdateTrackIDs.Parameters.AddWithValue("@trackNum", null);

                // Time to do some SQL!
                using (SQLiteTransaction cSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iTrackIDs.Length; iLoop++)
                    {
                        cUpdateTrackIDs.Parameters[1].Value = iTrackPos[iLoop];
                        cUpdateTrackIDs.ExecuteNonQuery();

                        cAddTrackCmd.Parameters[1].Value = iTrackPos[iLoop];
                        cAddTrackCmd.Parameters[2].Value = iTrackIDs[iLoop];
                        cAddTrackCmd.ExecuteNonQuery();
                    }

                    cSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Moves tracks around in a playlist
        /// It is important to note that changes take effect in order of position in the array.
        /// Old and new positions should account for changes of all previous moves in the array(s)
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to make moves</param>
        /// <param name="iOldPos">Old positions to move from</param>
        /// <param name="iNewPos">New positions to move to</param>
        public override void moveTrackInPlaylist(int iPlaylistID, int[] iOldPos, int[] iNewPos)
        {
            if (iOldPos.Length != iNewPos.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cBatchUpdateDown = _cSQLConnection.CreateCommand(),
                cBatchUpdateUp = _cSQLConnection.CreateCommand(),
                cMoveTrack = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cBatchUpdateDown.CommandText = "UPDATE playlist_standard_tracks SET track_number=track_number+1 WHERE playlist_id=@playID AND track_number>=@newTrack ORDER BY track_number DESC;";
                cBatchUpdateUp.CommandText = "UPDATE playlist_standard_tracks SET track_number=track_number-1 WHERE playlist_id=@playID AND track_number>=@oldTrack ORDER BY track_number ASC;";
                cMoveTrack.CommandText = "UPDATE playlist_standard_tracks SET track_number=@newTrack WHERE playlist_id=@playID AND track_number=@oldTrack;";

                cBatchUpdateDown.Parameters.AddWithValue("@playID", iPlaylistID);
                cBatchUpdateDown.Parameters.AddWithValue("@newTrack", null);
                cBatchUpdateUp.Parameters.AddWithValue("@playID", iPlaylistID);
                cBatchUpdateUp.Parameters.AddWithValue("@oldTrack", null);
                cMoveTrack.Parameters.AddWithValue("@playID", iPlaylistID);
                cMoveTrack.Parameters.AddWithValue("@newTrack", null);
                cMoveTrack.Parameters.AddWithValue("@oldTrack", null);

                // Times for t3h SQL's
                using (SQLiteTransaction cSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iOldPos.Length; iLoop++)
                    {
                        cBatchUpdateDown.Parameters[0].Value = iNewPos[iLoop];
                        cBatchUpdateDown.ExecuteNonQuery();

                        cMoveTrack.Parameters[0].Value = iNewPos[iLoop];
                        cMoveTrack.Parameters[1].Value = iOldPos[iLoop];
                        cMoveTrack.ExecuteNonQuery();

                        cBatchUpdateUp.Parameters[0].Value = iOldPos[iLoop];
                        cBatchUpdateUp.ExecuteNonQuery();
                    }

                    cSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Changes the name of a playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="sName">New Playlist Name</param>
        public override void changeStandardPlaylistName(int[] iPlaylistID, string[] sName)
        {
            if (iPlaylistID.Length != sName.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cUpdateCmd = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cUpdateCmd.CommandText = "UPDATE playlist_standard SET playlist_name=@name WHERE playlist_id=@id;";

                cUpdateCmd.Parameters.AddWithValue("@name", null);
                cUpdateCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cUpdateCmd.Parameters[0].Value = sName[iLoop];
                        cUpdateCmd.Parameters[1].Value = iPlaylistID[iLoop];

                        cUpdateCmd.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Changes the time/date the playlist was created
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist</param>
        /// <param name="dTimeCreated">New DateTime that contains the new created date</param>
        public override void changeStandardPlaylistCreated(int[] iPlaylistID, DateTime[] dTimeCreated)
        {
            if (iPlaylistID.Length != dTimeCreated.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cUpdateCmd = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cUpdateCmd.CommandText = "UPDATE playlist_standard SET time_created=@time WHERE playlist_id=@id;";

                cUpdateCmd.Parameters.AddWithValue("@time", null);
                cUpdateCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cUpdateCmd.Parameters[0].Value = dTimeCreated[iLoop].Ticks;
                        cUpdateCmd.Parameters[1].Value = iPlaylistID[iLoop];

                        cUpdateCmd.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }
        #endregion

        #region Select Commands
        /// <summary>
        /// Gets all the playlist IDs that currently exist
        /// </summary>
        /// <returns>An array of playlist IDs</returns>
        public override int[] getStandardPlaylistIDs()
        {
            using (SQLiteCommand cSelectPlaylists = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectPlaylists.CommandText = "SELECT playlist_id FROM playlist_standard;";

                SQLiteDataReader dIDReader = cSelectPlaylists.ExecuteReader();

                LinkedList<int> iRetIDs = new LinkedList<int>();
                while (dIDReader.Read())
                    iRetIDs.AddLast(dIDReader.GetInt32(0));

                dIDReader.Close();
                int[] iRet = new int[iRetIDs.Count];
                iRetIDs.CopyTo(iRet, 0);
                return iRet;
            }
        }

        /// <summary>
        /// Gets the name of an array of playlist
        /// </summary>
        /// <param name="iPlaylistIDs">Playlists to look up; if null returns all names in the same order as getStandardPlaylistIDs() returns IDs</param>
        /// <returns>Names of the playlists</returns>
        public override string[] getStandardPlaylistName(int[] iPlaylistIDs)
        {
            if (iPlaylistIDs == null)
            {
                // Let's get everything
                using (SQLiteCommand cSelectName = _cSQLConnection.CreateCommand())
                {
                    // Setup the command
                    cSelectName.CommandText = "SELECT playlist_name FROM playlist_standard;";
                    // Get the names
                    SQLiteDataReader dNameReader = cSelectName.ExecuteReader();

                    // Build the array
                    LinkedList<string> sRetNames = new LinkedList<string>();
                    while (dNameReader.Read())
                        sRetNames.AddLast(dNameReader.GetString(0));

                    dNameReader.Close();
                    string[] sRet = new string[sRetNames.Count];
                    sRetNames.CopyTo(sRet, 0);
                    return sRet;
                }
            }

            // We need to follow the array
            using (SQLiteCommand cSelectName = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectName.CommandText = "SELECT playlist_name FROM playlist_standard WHERE playlist_id=@id;";

                cSelectName.Parameters.AddWithValue("@id", null);

                // Get the names
                SQLiteDataReader dNameReader;

                // Build the array
                string[] sRet = new string[iPlaylistIDs.Length];
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistIDs.Length; iLoop++)
                    {
                        cSelectName.Parameters[0].Value = iPlaylistIDs[iLoop];
                        dNameReader = cSelectName.ExecuteReader();

                        if (!dNameReader.Read())
                            throw new IndexOutOfRangeException("A playlist ID does not have a valid name/the playlist could not be found.");

                        sRet[iLoop] = dNameReader.GetString(0);
                        dNameReader.Close();
                    }

                    tSQLTrans.Commit();
                }

                return sRet;
            }
        }

        /// <summary>
        /// Returns the time each playlist was created
        /// </summary>
        /// <param name="iPlaylistIDs">Playlists to look up; if null returns all names in the same order as getStandardPlaylistIDs() returns IDs</param>
        /// <returns>All of the times created</returns>
        public override DateTime[] getStandardPlaylistCreated(int[] iPlaylistIDs)
        {
            if (iPlaylistIDs == null)
            {
                // Let's get everything
                using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
                {
                    // Setup the command
                    cSelectTime.CommandText = "SELECT time_created FROM playlist_standard;";
                    // Get the names
                    SQLiteDataReader dNameReader = cSelectTime.ExecuteReader();

                    // Build the array
                    LinkedList<DateTime> sRetNames = new LinkedList<DateTime>();
                    while (dNameReader.Read())
                        sRetNames.AddLast(new DateTime(dNameReader.GetInt64(0)));

                    dNameReader.Close();
                    DateTime[] sRet = new DateTime[sRetNames.Count];
                    sRetNames.CopyTo(sRet, 0);
                    return sRet;
                }
            }

            // We need to follow the array
            using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectTime.CommandText = "SELECT time_created FROM playlist_standard WHERE playlist_id=@id;";

                cSelectTime.Parameters.AddWithValue("@id", null);

                // Get the names
                SQLiteDataReader dNameReader;

                // Build the array
                DateTime[] sRet = new DateTime[iPlaylistIDs.Length];
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistIDs.Length; iLoop++)
                    {
                        cSelectTime.Parameters[0].Value = iPlaylistIDs[iLoop];
                        dNameReader = cSelectTime.ExecuteReader();

                        if (!dNameReader.Read())
                            throw new IndexOutOfRangeException("A playlist ID does not have a valid name/the playlist could not be found.");

                        sRet[iLoop] = new DateTime(dNameReader.GetInt64(0));

                        dNameReader.Close();
                    }

                    tSQLTrans.Commit();
                }

                return sRet;
            }
        } 

        /// <summary>
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type)</param>
        /// <param name="stSearchType">Method to search with</param>
        /// <returns>Array of mediaEntries that match the search terms</returns>
        protected override HashSet<int> searchStandardPlaylistMedia_implemented(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            HashSet<int> iRet;
            using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
            {
                iRet = searchStandardPlaylist(iPlaylistID, sData, mDataType, stSearchType);
                tSQLTrans.Commit();
            }

            return iRet;
        }
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes a tracks from the playlist
        /// It is important to note that changes take effect in order of position in the array.
        /// Removing one track will cause all other tracks to bump-back and fill the gap, make sure you account for this
        /// when you pass in the track position IDs.
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to remove from</param>
        /// <param name="iTrackPos">Location of the tracks in the playlist to remove</param>
        public override void removeTrackFromPlaylist(int iPlaylistID, int[] iTrackPos)
        {
            using (SQLiteCommand cRemoveTrack = _cSQLConnection.CreateCommand(),
                cMoveTracks = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cRemoveTrack.CommandText = "DELETE FROM playlist_standard_tracks WHERE playlist_id=@playID AND track_number=@trackPos;";
                cMoveTracks.CommandText = "UPDATE playlist_standard_tracks SET track_number=track_number-1 WHERE playlist_id=@playID AND track_number>=@trackPos ORDER BY track_number ASC;";

                cRemoveTrack.Parameters.AddWithValue("@playID", iPlaylistID);
                cRemoveTrack.Parameters.AddWithValue("@trackPos", null);
                cMoveTracks.Parameters.AddWithValue("@playID", iPlaylistID);
                cMoveTracks.Parameters.AddWithValue("@trackPos", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    foreach (int iCurrTrack in iTrackPos)
                    {
                        cRemoveTrack.Parameters[1].Value = iCurrTrack;
                        cRemoveTrack.ExecuteNonQuery();

                        cMoveTracks.Parameters[1].Value = iCurrTrack;
                        cMoveTracks.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Removes an entire playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to remove</param>
        public override void removeStandardPlaylist(int iPlaylistID)
        {
            using (SQLiteCommand cRemovePlaylist = _cSQLConnection.CreateCommand(),
                cRemoveTracks = _cSQLConnection.CreateCommand())
            {
                // Setup the commands
                cRemovePlaylist.CommandText = "DELETE FROM playlist_standard WHERE playlist_id=@playID;";
                cRemoveTracks.CommandText = "DELETE FROM playlist_standard_tracks WHERE playlist_id=@playID;";

                cRemovePlaylist.Parameters.AddWithValue("@playID", iPlaylistID);
                cRemoveTracks.Parameters.AddWithValue("@playID", iPlaylistID);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    cRemovePlaylist.ExecuteNonQuery();
                    cRemoveTracks.ExecuteNonQuery();
                    tSQLTrans.Commit();
                }
            }
        }
        #endregion
        #endregion

        #region Dynamic Playlists
        #region Insert Commands
        /// <summary>
        /// Creates a new dynamic playlist
        /// </summary>
        /// <param name="sPlaylistName">Playlist Name</param>
        /// <param name="dQuery">Query for the playlist; if null none set</param>
        /// <param name="dTimeCreated">Time Created</param>
        /// <returns>Playlist ID</returns>
        public override int createDynamicPlaylist(string sPlaylistName, dynamicQuery dQuery, DateTime dTimeCreated)
        {
            using (SQLiteCommand cCreatePlaylistCmd = _cSQLConnection.CreateCommand(),
                cLastInsertedCmd = _cSQLConnection.CreateCommand())
            {
                // Setup the commands
                cCreatePlaylistCmd.CommandText = "INSERT INTO playlist_dynamic (playlist_name, binary_query, time_created) VALUES (@name, @query, @time);";
                cLastInsertedCmd.CommandText = "SELECT last_insert_rowid() as id;";

                cCreatePlaylistCmd.Parameters.AddWithValue("@name", sPlaylistName);
                cCreatePlaylistCmd.Parameters.AddWithValue("@query", dQuery == null ? null : dQuery.serialize());
                cCreatePlaylistCmd.Parameters.AddWithValue("@time", dTimeCreated.Ticks);

                int iPlaylistID;
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    cCreatePlaylistCmd.ExecuteNonQuery();

                    // Let's get it's ID
                    SQLiteDataReader dataReader = cLastInsertedCmd.ExecuteReader();
                    if (!dataReader.Read())
                    {
                        tSQLTrans.Rollback();
                        return -1;
                    }

                    iPlaylistID = dataReader.GetInt32(0);
                    dataReader.Close();

                    tSQLTrans.Commit();

                    return iPlaylistID;
                }
            }
        }

        /// <summary>
        /// Changes the name associated with an array of dynamic playlists
        /// </summary>
        /// <param name="iPlaylistIDs">IDs of playlists with names that need changin'</param>
        /// <param name="sNewNames">New names for each playlist</param>
        public override void changeDynamicPlaylistName(int[] iPlaylistIDs, string[] sNewNames)
        {
            if (iPlaylistIDs.Length != sNewNames.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cUpdateCmd = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cUpdateCmd.CommandText = "UPDATE playlist_dynamic SET playlist_name=@name WHERE playlist_id=@id;";

                cUpdateCmd.Parameters.AddWithValue("@name", null);
                cUpdateCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistIDs.Length; iLoop++)
                    {
                        cUpdateCmd.Parameters[0].Value = sNewNames[iLoop];
                        cUpdateCmd.Parameters[1].Value = iPlaylistIDs[iLoop];

                        cUpdateCmd.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Changes the date and time of an array of playlists
        /// </summary>
        /// <param name="iPlaylistIDs">IDs of the playlists to change</param>
        /// <param name="dTimeCreated">New times to be set</param>
        public override void changeDynamicPlaylistCreated(int[] iPlaylistIDs, DateTime[] dTimeCreated)
        {
            if (iPlaylistIDs.Length != dTimeCreated.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cUpdateCmd = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cUpdateCmd.CommandText = "UPDATE playlist_dynamic SET time_created=@time WHERE playlist_id=@id;";

                cUpdateCmd.Parameters.AddWithValue("@time", null);
                cUpdateCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistIDs.Length; iLoop++)
                    {
                        cUpdateCmd.Parameters[0].Value = dTimeCreated[iLoop].Ticks;
                        cUpdateCmd.Parameters[1].Value = iPlaylistIDs[iLoop];

                        cUpdateCmd.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Sets the dynamic playlist for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs</param>
        /// <param name="dQuery">Queries</param>
        public override void changeDynamicPlaylistQuery(int[] iPlaylistID, dynamicQuery[] dQuery)
        {
            if (iPlaylistID.Length != dQuery.Length)
                throw new ArgumentException("Argument array lengths differ.");

            using (SQLiteCommand cUpdateCmd = _cSQLConnection.CreateCommand())
            {
                // Setup Commands
                cUpdateCmd.CommandText = "UPDATE playlist_dynamic SET binary_query=@query WHERE playlist_id=@id;";

                cUpdateCmd.Parameters.AddWithValue("@query", null);
                cUpdateCmd.Parameters.AddWithValue("@id", null);

                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cUpdateCmd.Parameters[0].Value = dQuery[iLoop].serialize();
                        cUpdateCmd.Parameters[1].Value = iPlaylistID[iLoop];

                        cUpdateCmd.ExecuteNonQuery();
                    }

                    tSQLTrans.Commit();
                }
            }
        }
        #endregion

        #region Select Commands
        /// <summary>
        /// Get all the dynamic playlist IDs
        /// </summary>
        /// <returns>All IDs</returns>
        public override int[] getDynamicPlaylistIDs()
        {
            using (SQLiteCommand cSelectPlaylists = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectPlaylists.CommandText = "SELECT playlist_id FROM playlist_dynamic;";

                SQLiteDataReader dIDReader = cSelectPlaylists.ExecuteReader();

                LinkedList<int> iRetIDs = new LinkedList<int>();
                while (dIDReader.Read())
                    iRetIDs.AddLast(dIDReader.GetInt32(0));

                dIDReader.Close();
                int[] iRet = new int[iRetIDs.Count];
                iRetIDs.CopyTo(iRet, 0);
                return iRet;
            }
        }

        /// <summary>
        /// Gets a bunch of playlist names
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up; if null returns all names in the same order as getDynamicPlaylistIDs() returns IDs</param>
        /// <returns>Names of the playlists</returns>
        public override string[] getDynamicPlaylistName(int[] iPlaylistID)
        {
            if (iPlaylistID == null)
            {
                // Let's get everything
                using (SQLiteCommand cSelectName = _cSQLConnection.CreateCommand())
                {
                    // Setup the command
                    cSelectName.CommandText = "SELECT playlist_name FROM playlist_dynamic;";
                    // Get the names
                    SQLiteDataReader dNameReader = cSelectName.ExecuteReader();

                    // Build the array
                    LinkedList<string> sRetNames = new LinkedList<string>();
                    while (dNameReader.Read())
                        sRetNames.AddLast(dNameReader.GetString(0));

                    dNameReader.Close();
                    string[] sRet = new string[sRetNames.Count];
                    sRetNames.CopyTo(sRet, 0);
                    return sRet;
                }
            }

            // We need to follow the array
            using (SQLiteCommand cSelectName = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectName.CommandText = "SELECT playlist_name FROM playlist_dynamic WHERE playlist_id=@id;";

                cSelectName.Parameters.AddWithValue("@id", null);

                // Get the names
                SQLiteDataReader dNameReader;

                // Build the array
                string[] sRet = new string[iPlaylistID.Length];
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cSelectName.Parameters[0].Value = iPlaylistID[iLoop];
                        dNameReader = cSelectName.ExecuteReader();

                        if (!dNameReader.Read())
                            throw new IndexOutOfRangeException("A playlist ID does not have a valid name/the playlist could not be found.");

                        sRet[iLoop] = dNameReader.GetString(0);
                        dNameReader.Close();
                    }

                    tSQLTrans.Commit();
                }

                return sRet;
            }
        }

        /// <summary>
        /// Gets datetime created for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up,</param>
        /// <returns></returns>
        public override DateTime[] getDynamicPlaylistCreated(int[] iPlaylistID)
        {
            if (iPlaylistID == null)
            {
                // Let's get everything
                using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
                {
                    // Setup the command
                    cSelectTime.CommandText = "SELECT time_created FROM playlist_dynamic;";
                    // Get the names
                    SQLiteDataReader dNameReader = cSelectTime.ExecuteReader();

                    // Build the array
                    LinkedList<DateTime> sRetNames = new LinkedList<DateTime>();
                    while (dNameReader.Read())
                        sRetNames.AddLast(new DateTime(dNameReader.GetInt64(0)));

                    dNameReader.Close();
                    DateTime[] sRet = new DateTime[sRetNames.Count];
                    sRetNames.CopyTo(sRet, 0);
                    return sRet;
                }
            }

            // We need to follow the array
            using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectTime.CommandText = "SELECT time_created FROM playlist_dynamic WHERE playlist_id=@id;";

                cSelectTime.Parameters.AddWithValue("@id", null);

                // Get the names
                SQLiteDataReader dNameReader;

                // Build the array
                DateTime[] sRet = new DateTime[iPlaylistID.Length];
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cSelectTime.Parameters[0].Value = iPlaylistID[iLoop];
                        dNameReader = cSelectTime.ExecuteReader();

                        if (!dNameReader.Read())
                            throw new IndexOutOfRangeException("A playlist ID does not have a valid name/the playlist could not be found.");

                        sRet[iLoop] = new DateTime(dNameReader.GetInt64(0));

                        dNameReader.Close();
                    }

                    tSQLTrans.Commit();
                }

                return sRet;
            }
        }

        /// <summary>
        /// Gets the dynamic playlist query for an array of playlists
        /// </summary>
        /// <param name="iPlaylistID">Playlist IDs to look up; if null returns all names in the same order as getDynamicPlaylistIDs() returns IDs</param>
        /// <returns>Queries</returns>
        public override dynamicQuery[] getDynamicPlaylistQuery(int[] iPlaylistID)
        {
            if (iPlaylistID == null)
            {
                // Let's get everything
                using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
                {
                    // Setup the command
                    cSelectTime.CommandText = "SELECT binary_query FROM playlist_dynamic;";
                    // Get the names
                    SQLiteDataReader dNameReader = cSelectTime.ExecuteReader();

                    // Build the array
                    LinkedList<dynamicQuery> sRetNames = new LinkedList<dynamicQuery>();
                    byte[] buffer;
                    while (dNameReader.Read())
                    {
                        buffer = new byte[dNameReader.GetBytes(0, 0, null, 0, 0)];
                        dNameReader.GetBytes(0, 0, buffer, 0, buffer.Length);
                        sRetNames.AddLast(dynamicQuery.deserialize(buffer));
                    }

                    dNameReader.Close();
                    dynamicQuery[] sRet = new dynamicQuery[sRetNames.Count];
                    sRetNames.CopyTo(sRet, 0);
                    return sRet;
                }
            }

            // We need to follow the array
            using (SQLiteCommand cSelectTime = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectTime.CommandText = "SELECT binary_query FROM playlist_dynamic WHERE playlist_id=@id;";

                cSelectTime.Parameters.AddWithValue("@id", null);

                // Get the names
                SQLiteDataReader dNameReader;

                // Build the array
                dynamicQuery[] sRet = new dynamicQuery[iPlaylistID.Length];
                byte[] buffer;
                using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
                {
                    for (int iLoop = 0; iLoop < iPlaylistID.Length; iLoop++)
                    {
                        cSelectTime.Parameters[0].Value = iPlaylistID[iLoop];
                        dNameReader = cSelectTime.ExecuteReader();

                        if (!dNameReader.Read())
                            throw new IndexOutOfRangeException("A playlist ID does not have a valid name/the playlist could not be found.");

                        buffer = new byte[dNameReader.GetBytes(0, 0, null, 0, 0)];
                        dNameReader.GetBytes(0, 0, buffer, 0, buffer.Length);
                        sRet[iLoop] = dynamicQuery.deserialize(buffer);

                        dNameReader.Close();
                    }

                    tSQLTrans.Commit();
                }

                return sRet;
            }
        }

        /// <summary>
        /// Searches the tracks in a given playlist
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID to seach</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Term type (meta data type)</param>
        /// <param name="stSearchType">Method to search with</param>
        /// <returns>Array of mediaEntries that match the search terms</returns>
        protected override HashSet<int> searchDynamicPlaylistMedia_implemented(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            HashSet<int> iRet;
            using (SQLiteTransaction tSQLTrans = _cSQLConnection.BeginTransaction())
            {
                iRet = searchDynamicPlaylist(iPlaylistID, sData, mDataType, stSearchType);
                tSQLTrans.Commit();
            }

            return iRet;
        }
        #endregion

        #region Delete Commands
        /// <summary>
        /// Removes a dynamic playlist
        /// </summary>
        /// <param name="iPlaylistID">ID of the playlist to remove</param>
        public override void removeDynamicPlaylist(int iPlaylistID)
        {
            using (SQLiteCommand cRemovePlaylist = _cSQLConnection.CreateCommand())
            {
                // Setup the commands
                cRemovePlaylist.CommandText = "DELETE FROM playlist_dynamic WHERE playlist_id=@playID;";

                cRemovePlaylist.Parameters.AddWithValue("@playID", iPlaylistID);

                cRemovePlaylist.ExecuteNonQuery();
            }
        }
        #endregion
        #endregion
        #endregion
        #endregion

        #region Protected Members
        /// <summary>
        /// Performs the initial fill of the memory dictionary
        /// </summary>
        protected override void initFill()
        {
            mediaEntry[] mAll = getTrackData(searchDatabase(new string[0], new metaDataFieldTypes[0], searchType.none));

            foreach (mediaEntry mCurr in mAll)
                _mLibrary.Add(mCurr.iEntryID, mCurr);
        }

        /// <summary>
        /// Opens the database
        /// </summary>
        /// <returns>True if worked; otherwise false</returns>
        protected override bool openDatabase()
        {
            _cSQLConnection = new SQLiteConnection("Data Source=" + _sDataPath);
            _cSQLConnection.Open();
            createTables();

            return true;
        }
        #endregion

        #region Private Members
        #region Search Functions
        /// <summary>
        /// Searches the media library and returns ID's that match the search terms
        /// </summary>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="stSearchType">Search method</param>
        /// <returns>Track ID's that fit the search terms</returns>
        private HashSet<int> searchDatabase(string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            if (sData.Length != mDataType.Length)
                throw new ArgumentException("Argument array lengths differ with matchSearchField.");

            HashSet<int> iMediaIDs = null;

            // Let's get the media entries that match the query (just their ID's)
            using (SQLiteCommand cSearchSelectCmd = _cSQLConnection.CreateCommand())
            {
                // We need a reader
                SQLiteDataReader dEntryIDReader;

                // Let's make sure we search correctly
                string sSearchFieldAppend;
                if ((stSearchType & searchType.matchAllFields) != 0)
                    sSearchFieldAppend = "AND";
                else
                    sSearchFieldAppend = "OR";

                // If we need to search more than one time
                if (sData.Length != 0)
                {
                    cSearchSelectCmd.Parameters.AddWithValue("", null);
                    for (int iLoop = 0; iLoop < sData.Length; iLoop++)
                    {
                        StringBuilder sbCmdBuilder = new StringBuilder("SELECT media_item_id FROM media_data WHERE (media_data_searchable LIKE ?");
                        cSearchSelectCmd.Parameters[0].Value =  "%" + makeSearchable(sData[iLoop]) + "%";

                        // If we're not matching all field types; build the field type part of the query
                        bool bHasWritten;
                        if (mDataType[iLoop] != metaDataFieldTypes.none && mDataType[iLoop] != metaDataFieldTypes.all)
                        {
                            sbCmdBuilder.Append(" AND (");

                            bHasWritten = false;
                            for (int iLoop2 = 1; iLoop2 <= (int)metaDataFieldTypes.last; iLoop2 *= 2)
                            {
                                if ((mDataType[iLoop] & (metaDataFieldTypes)iLoop2) == 0)
                                    continue;

                                if (bHasWritten)
                                {
                                    sbCmdBuilder.Append(" ");
                                    sbCmdBuilder.Append(sSearchFieldAppend);
                                    sbCmdBuilder.Append(" ");
                                }

                                sbCmdBuilder.Append("media_data_type=?");
                                cSearchSelectCmd.Parameters.AddWithValue("", iLoop2);
                                bHasWritten = true;
                            }

                            sbCmdBuilder.Append(")");
                        }

                        sbCmdBuilder.Append(");");

                        cSearchSelectCmd.CommandText = sbCmdBuilder.ToString();
                        dEntryIDReader = cSearchSelectCmd.ExecuteReader(); //This will return a reader with all the media_item_id's that match the search

                        // Let's grab all the media ids
                        HashSet<int> iTempIDs = new HashSet<int>();
                        while (dEntryIDReader.Read())
                            iTempIDs.Add(dEntryIDReader.GetInt32(0));

                        dEntryIDReader.Close(); //Done reading

                        if (iMediaIDs == null)
                            iMediaIDs = iTempIDs;
                        else if ((stSearchType & searchType.matchAllParams) != 0)
                            iMediaIDs.IntersectWith(iTempIDs);
                        else
                            iMediaIDs.UnionWith(iTempIDs);
                    }
                }
                else //Otherwise do a simple search
                {
                    StringBuilder sbCmdBuilder = new StringBuilder("SELECT media_item_id FROM media_data;");

                    // Build select cmd
                    cSearchSelectCmd.CommandText = sbCmdBuilder.ToString();
                    dEntryIDReader = cSearchSelectCmd.ExecuteReader(); //This will return a reader with all the media_item_id's that match the search

                    // Let's grab all the media ids
                    iMediaIDs = new HashSet<int>();
                    while (dEntryIDReader.Read())
                        iMediaIDs.Add(dEntryIDReader.GetInt32(0));

                    dEntryIDReader.Close(); //Done reading
                }
            }

            return iMediaIDs;
        }

        /// <summary>
        /// Searches a playlist and returns ID's that match the search terms
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="stSearchType">Search method</param>
        /// <returns>Track ID's that fit the search terms</returns>
        private HashSet<int> searchStandardPlaylist(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            if (sData.Length != mDataType.Length)
                throw new ArgumentException("Argument array lengths differ with matchSearchField.");

            HashSet<int> iMediaIDs = null;

            // Let's get the media entries that match the query (just their ID's)
            SQLiteDataReader dEntryIDReader;
            using (SQLiteCommand cSearchSelectCmd = _cSQLConnection.CreateCommand())
            {
                // Let's make sure we search correctly
                string sSearchFieldAppend;
                if ((stSearchType & searchType.matchAllFields) != 0)
                    sSearchFieldAppend = "AND";
                else
                    sSearchFieldAppend = "OR";

                // If we need to search more than one time
                if (sData.Length != 0)
                {
                    cSearchSelectCmd.Parameters.AddWithValue("", null);
                    cSearchSelectCmd.Parameters.AddWithValue("@playlistID", iPlaylistID);
                    for (int iLoop = 0; iLoop < sData.Length; iLoop++)
                    {
                        StringBuilder sbCmdBuilder = new StringBuilder("SELECT media_data.media_item_id FROM media_data INNER JOIN playlist_standard_tracks ON media_data.media_item_id=playlist_standard_tracks.media_item_id WHERE playlist_standard_tracks.playlist_id=@playlistID AND (media_data.media_data_searchable LIKE ?");
                        cSearchSelectCmd.Parameters[0].Value = "%" + makeSearchable(sData[iLoop]) + "%";

                        // If we're not matching all field types; build the field type part of the query
                        bool bHasWritten;
                        if (mDataType[iLoop] != metaDataFieldTypes.none && mDataType[iLoop] != metaDataFieldTypes.all)
                        {
                            sbCmdBuilder.Append(" AND (");

                            bHasWritten = false;
                            for (int iLoop2 = 1; iLoop2 <= (int)metaDataFieldTypes.last; iLoop2 *= 2)
                            {
                                if ((mDataType[iLoop] & (metaDataFieldTypes)iLoop2) == 0)
                                    continue;

                                if (bHasWritten)
                                {
                                    sbCmdBuilder.Append(" ");
                                    sbCmdBuilder.Append(sSearchFieldAppend);
                                    sbCmdBuilder.Append(" ");
                                }

                                sbCmdBuilder.Append("media_data_type=?");
                                cSearchSelectCmd.Parameters.AddWithValue("", iLoop2);
                                bHasWritten = true;
                            }

                            sbCmdBuilder.Append(")");
                        }

                        sbCmdBuilder.Append(") ORDER BY playlist_standard_tracks.track_number;");

                        cSearchSelectCmd.CommandText = sbCmdBuilder.ToString();
                        dEntryIDReader = cSearchSelectCmd.ExecuteReader(); //This will return a reader with all the media_item_id's that match the search

                        // Let's grab all the media ids
                        HashSet<int> iTempIDs = new HashSet<int>();
                        while (dEntryIDReader.Read())
                            iTempIDs.Add(dEntryIDReader.GetInt32(0));

                        dEntryIDReader.Close(); //Done reading

                        if (iMediaIDs == null)
                            iMediaIDs = iTempIDs;
                        else if ((stSearchType & searchType.matchAllParams) != 0)
                            iMediaIDs.IntersectWith(iTempIDs);
                        else
                            iMediaIDs.UnionWith(iTempIDs);
                    }
                }
                else //Otherwise do a simple search
                {
                    StringBuilder sbCmdBuilder = new StringBuilder("SELECT media_data.media_item_id FROM media_data INNER JOIN playlist_standard_tracks ON media_data.media_item_id=playlist_standard_tracks.media_item_id WHERE playlist_standard_tracks.playlist_id=@playlistID;");

                    // Build select cmd
                    cSearchSelectCmd.Parameters.AddWithValue("@playlistID", iPlaylistID);
                    cSearchSelectCmd.CommandText = sbCmdBuilder.ToString();
                    dEntryIDReader = cSearchSelectCmd.ExecuteReader(); //This will return a reader with all the media_item_id's that match the search

                    // Let's grab all the media ids
                    iMediaIDs = new HashSet<int>();
                    while (dEntryIDReader.Read())
                        iMediaIDs.Add(dEntryIDReader.GetInt32(0));

                    dEntryIDReader.Close(); //Done reading
                }
            }

            return iMediaIDs;
        }

        /// <summary>
        /// Searches a playlist and returns ID's that match the search terms
        /// </summary>
        /// <param name="iPlaylistID">Playlist ID</param>
        /// <param name="sData">Search terms</param>
        /// <param name="mDataType">Search term type; if none/all we'll search ALL datatypes</param>
        /// <param name="stSearchType">Search method</param>
        /// <returns>Track ID's that fit the search terms</returns>
        private HashSet<int> searchDynamicPlaylist(int iPlaylistID, string[] sData, metaDataFieldTypes[] mDataType, searchType stSearchType)
        {
            // Let's get the dynamic query
            dynamicQuery dDynamicQuery = getDynamicPlaylistQuery(iPlaylistID);
            HashSet<int> iMediaIDs = executeDynamicQuery(dDynamicQuery.sSearchQuery);
            iMediaIDs.IntersectWith(searchDatabase(sData, mDataType, stSearchType));
            return iMediaIDs;
        }

        /// <summary>
        /// Executes a dynamic query
        /// </summary>
        /// <param name="sQuery">Query to execute</param>
        /// <returns>Media IDs that matched the query</returns>
        private HashSet<int> executeDynamicQuery(ISearchBase sQuery)
        {
            HashSet<int> iMediaIDs;

            // Are we a query group?
            if (sQuery.getNextQueries() != null)
            {
                searchGroup sGroup = (searchGroup)sQuery;

                switch (sGroup.lLogicOperator)
                {
                    case logicOperators.none:
                        throw new ApplicationException("No logical operator set for logical grouping.");

                    case logicOperators.parenthesis:
                        return executeDynamicQuery(sGroup.sFirstObject);

                    case logicOperators.and:
                        iMediaIDs = executeDynamicQuery(sGroup.sFirstObject);
                        iMediaIDs.IntersectWith(executeDynamicQuery(sGroup.sSecondObject));
                        return iMediaIDs;

                    case logicOperators.or:
                        iMediaIDs = executeDynamicQuery(sGroup.sFirstObject);
                        iMediaIDs.UnionWith(executeDynamicQuery(sGroup.sSecondObject));
                        return iMediaIDs;

                    default:
                        throw new NotSupportedException("Logical operator specified is not supported by sqliteDatabase.");
                }
            }
            else
            {
                searchEntity sEntity = (searchEntity)sQuery;

                using (SQLiteCommand cSearchSelectCmd = _cSQLConnection.CreateCommand())
                {
                    // We need a reader
                    SQLiteDataReader dEntryIDReader;
                    StringBuilder sbCmdBuilder = new StringBuilder("SELECT media_item_id FROM media_data WHERE (media_data_searchable LIKE ?");
                    cSearchSelectCmd.Parameters.AddWithValue("", "%" + makeSearchable(sEntity.sSearchTerm) + "%");

                    // If we're not matching all field types; build the field type part of the query
                    bool bHasWritten;
                    if (sEntity.mFieldType != metaDataFieldTypes.none && sEntity.mFieldType != metaDataFieldTypes.all)
                    {
                        sbCmdBuilder.Append(" AND (");

                        bHasWritten = false;
                        for (int iLoop2 = 1; iLoop2 <= (int)metaDataFieldTypes.last; iLoop2 *= 2)
                        {
                            if ((sEntity.mFieldType & (metaDataFieldTypes)iLoop2) == 0)
                                continue;

                            if (bHasWritten)
                            {
                                sbCmdBuilder.Append(" OR ");
                            }

                            sbCmdBuilder.Append("media_data_type=?");
                            cSearchSelectCmd.Parameters.AddWithValue("", iLoop2);
                            bHasWritten = true;
                        }

                        sbCmdBuilder.Append(")");
                    }

                    sbCmdBuilder.Append(");");

                    // Build select cmd
                    cSearchSelectCmd.CommandText = sbCmdBuilder.ToString();
                    dEntryIDReader = cSearchSelectCmd.ExecuteReader(); //This will return a reader with all the media_item_id's that match the search

                    // Let's grab all the media ids
                    iMediaIDs = new HashSet<int>();
                    while (dEntryIDReader.Read())
                        iMediaIDs.Add(dEntryIDReader.GetInt32(0));

                    dEntryIDReader.Close(); //Done reading

                    return iMediaIDs;
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets the mediaEntry and all track data for an array of track ids
        /// </summary>
        /// <param name="iTrackIDs">Track IDs to get data for</param>
        /// <returns>Data reader to be parsed</returns>
        private mediaEntry[] getTrackData(HashSet<int> iTrackIDs)
        {
            // Let's build the array
            mediaEntry[] mMediaEntries = new mediaEntry[iTrackIDs.Count];
            using (SQLiteCommand cSelectCmd = _cSQLConnection.CreateCommand())
            {
                // Setup the command
                cSelectCmd.CommandText = "SELECT item_uri, item_length, time_added, media_data_type, media_data FROM media_items INNER JOIN media_data ON media_items.media_item_id=media_data.media_item_id WHERE media_items.media_item_id=@id;";

                cSelectCmd.Parameters.AddWithValue("@id", null);

                // While there are still ID's to search
                SQLiteDataReader dDataReader;
                mediaEntry mCurrMediaEntry;
                int iLoop = 0;
                foreach (int iCurrID in iTrackIDs)
                {
                    cSelectCmd.Parameters[0].Value = iCurrID;

                    // Let's get to work
                    mCurrMediaEntry = new mediaEntry();
                    mCurrMediaEntry.iEntryID = iCurrID;

                    dDataReader = cSelectCmd.ExecuteReader();

                    if (!dDataReader.Read())
                        throw new InvalidOperationException("Database out of sync.");

                    mCurrMediaEntry.sPath = dDataReader.GetString(0);
                    mCurrMediaEntry.tLength = new TimeSpan(dDataReader.GetInt64(1));
                    mCurrMediaEntry.dTimeAdded = new DateTime(dDataReader.GetInt64(2));

                    do
                    {
                        mCurrMediaEntry[(metaDataFieldTypes)dDataReader.GetInt32(3)] = dDataReader.GetString(4);
                    } while (dDataReader.Read());

                    dDataReader.Close();

                    mMediaEntries[iLoop] = mCurrMediaEntry;
                    iLoop++;
                }
            }

            return mMediaEntries;
        }

        /// <summary>
        /// Creates the table structure if they do not exist yet
        /// </summary>
        private void createTables()
        {
            SQLiteCommand cCmd = _cSQLConnection.CreateCommand();
            cCmd.CommandText = "CREATE TABLE IF NOT EXISTS media_items (media_item_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, item_uri TEXT NOT NULL UNIQUE, item_length BIGINT NOT NULL, time_added BIGINT NOT NULL);" +
                "CREATE TABLE IF NOT EXISTS media_data (media_item_id INTEGER NOT NULL, media_data_type INTEGER NOT NULL, media_data TEXT, media_data_searchable TEXT, PRIMARY KEY(media_item_id, media_data_type));" +
                "CREATE TABLE IF NOT EXISTS playlist_standard (playlist_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, playlist_name TEXT NOT NULL, time_created BIGINT NOT NULL);" +
                "CREATE TABLE IF NOT EXISTS playlist_standard_tracks (playlist_id INTEGER NOT NULL, track_number INTEGER NOT NULL, media_item_id INTEGER NOT NULL, PRIMARY KEY (playlist_id, track_number));" +
                "CREATE TABLE IF NOT EXISTS playlist_dynamic (playlist_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, playlist_name TEXT NOT NULL, binary_query BLOB, time_created BIGINT NOT NULL);";
            cCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns if a row exists in the media_items table based off the unique key
        /// </summary>
        /// <param name="sItemURI">Item URI</param>
        /// <returns>If row exists</returns>
        private int itemRowID(string sItemURI)
        {
            int iRet;
            using (SQLiteCommand cCountItemCmd = _cSQLConnection.CreateCommand())
            {
                cCountItemCmd.CommandText = "SELECT media_item_id FROM media_items WHERE item_uri=@uri;";
                cCountItemCmd.Parameters.AddWithValue("@uri", sItemURI);

                SQLiteDataReader dReader = cCountItemCmd.ExecuteReader();

                if (!dReader.Read())
                    return -1;

                iRet = dReader.GetInt32(0);
                dReader.Close();
            }

            return iRet;
        }

        /// <summary>
        /// Returns if a row exists based off the primary key constraint
        /// </summary>
        /// <param name="iItemID">media item ID</param>
        /// <param name="mDataType">media data type</param>
        /// <returns>If the row exists</returns>
        private bool dataRowExists(int iItemID, metaDataFieldTypes mDataType)
        {
            bool bRet;
            using (SQLiteCommand cCountDataCmd = _cSQLConnection.CreateCommand())
            {
                cCountDataCmd.CommandText = "SELECT COUNT(1) FROM media_data WHERE media_item_id=@item_id AND media_data_type=@data_type;";
                cCountDataCmd.Parameters.AddWithValue("@item_id", iItemID);
                cCountDataCmd.Parameters.AddWithValue("@data_type", mDataType);

                SQLiteDataReader dReader = cCountDataCmd.ExecuteReader();

                if (!dReader.Read())
                    return false;

                bRet = dReader.GetBoolean(0);
                dReader.Close();
            }

            return bRet;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes resources
        /// </summary>
        public override void Dispose()
        {
            _cSQLConnection.Close();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
