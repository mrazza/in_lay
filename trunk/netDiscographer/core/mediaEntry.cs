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
using netAudio.core;

namespace netDiscographer.core
{
    /// <summary>
    /// Class to store media data
    /// </summary>
    public class mediaEntry
    {
        #region Members
        /// <summary>
        /// ID of the entry in the database
        /// </summary>
        public int iEntryID;

        /// <summary>
        /// Length of the track
        /// </summary>
        public TimeSpan tLength;

        /// <summary>
        /// Time added to the library
        /// </summary>
        public DateTime dTimeAdded;

        /// <summary>
        /// Path to the media
        /// </summary>
        public string sPath;

        /// <summary>
        /// Meta Data Array
        /// </summary>
        private string[] _sData;

        /// <summary>
        /// Friendly names for all metaDataFieldTypes
        /// </summary>
        private static readonly string[] _sFriendlyNames = { "Artist", "Title", "Album", "Album Artist", "Composer", "Year", "Comment", "Track #", "Total Tracks", "Disk #", "Total Disks", "Genre", "Rating", "Play Count", "Last Played" };
        #endregion

        #region Properties
        /// <summary>
        /// Specific Meta Data Type
        /// </summary>
        /// <param name="mType">metadata type</param>
        /// <returns></returns>
        public string this[metaDataFieldTypes mType]
        {
            get
            {
                if (_sData == null || mType == metaDataFieldTypes.none)
                    return null;

                return _sData[calcFieldTypesID(mType)];
            }
            set
            {
                if (mType == metaDataFieldTypes.none)
                    return;

                if (_sData == null)
                    _sData = new string[calcFieldTypesID(metaDataFieldTypes.last) + 1];

                _sData[calcFieldTypesID(mType)] = value;
            }
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Converts from netAudio.core.metaData to a mediaEntry
        /// Note: User must set their own Path and TimeAdded
        /// </summary>
        /// <param name="mOldData">metaData to convert</param>
        /// <returns>mediaEntry Version</returns>
        public static mediaEntry convertFromNetAudio(metaData mOldData)
        {
            return convertFromNetAudio(new metaData[] { mOldData })[0];
        }

        /// <summary>
        /// Converts from netAudio.core.metaData[] to a mediaEntry[]
        /// Note: User must set their own Path and TimeAdded
        /// </summary>
        /// <param name="mOldData">metaData to convert</param>
        /// <returns>mediaEntry Version</returns>
        public static mediaEntry[] convertFromNetAudio(metaData[] mOldData)
        {
            mediaEntry[] mNewData = new mediaEntry[mOldData.Length];

            for (int iLoop = 0; iLoop < mOldData.Length; iLoop++)
            {
                mNewData[iLoop] = new mediaEntry();
                mNewData[iLoop].dTimeAdded = new DateTime();
                mNewData[iLoop].tLength = mOldData[iLoop].tLength;
                mNewData[iLoop].sPath = null;
                mNewData[iLoop][metaDataFieldTypes.album] = mOldData[iLoop].sAlbum;
                mNewData[iLoop][metaDataFieldTypes.album_artist] = mOldData[iLoop].sAlbumArtist;
                mNewData[iLoop][metaDataFieldTypes.artist] = mOldData[iLoop].sArtist;
                mNewData[iLoop][metaDataFieldTypes.comment] = mOldData[iLoop].sComment;
                mNewData[iLoop][metaDataFieldTypes.composer] = mOldData[iLoop].sComposer;
                mNewData[iLoop][metaDataFieldTypes.disk] = mOldData[iLoop].iDisk.ToString();
                mNewData[iLoop][metaDataFieldTypes.genre] = mOldData[iLoop].sGenre;
                mNewData[iLoop][metaDataFieldTypes.play_count] = (0).ToString();
                mNewData[iLoop][metaDataFieldTypes.rating] = mOldData[iLoop].iRating.ToString();
                mNewData[iLoop][metaDataFieldTypes.time_last_played] = (-1).ToString();
                mNewData[iLoop][metaDataFieldTypes.title] = mOldData[iLoop].sTitle;
                mNewData[iLoop][metaDataFieldTypes.total_disks] = mOldData[iLoop].iTotalDisks.ToString();
                mNewData[iLoop][metaDataFieldTypes.total_tracks] = mOldData[iLoop].iTotalTracks.ToString();
                mNewData[iLoop][metaDataFieldTypes.track] = mOldData[iLoop].iTrack.ToString();
                mNewData[iLoop][metaDataFieldTypes.year] = mOldData[iLoop].iYear.ToString();
            }

            return mNewData;
        }

        /// <summary>
        /// Converts from mediaEntry to a netAudio.core.metaData
        /// </summary>
        /// <param name="mOldData">mediaEntry to convert</param>
        /// <returns>metaData Version</returns>
        public static metaData convertToNetAudio(mediaEntry mOldData)
        {
            return convertToNetAudio(new mediaEntry[] { mOldData })[0];
        }

        /// <summary>
        /// Converts from mediaEntry[] to a netAudio.core.metaData[]
        /// </summary>
        /// <param name="mOldData">mediaEntry to convert</param>
        /// <returns>metaData Version</returns>
        public static metaData[] convertToNetAudio(mediaEntry[] mOldData)
        {
            metaData[] mNewData = new metaData[mOldData.Length];

            for (int iLoop = 0; iLoop < mOldData.Length; iLoop++)
            {
                mNewData[iLoop] = new metaData();
                mNewData[iLoop].tLength = mOldData[iLoop].tLength;
                mNewData[iLoop].sAlbum = mOldData[iLoop][metaDataFieldTypes.album];
                mNewData[iLoop].sAlbumArtist = mOldData[iLoop][metaDataFieldTypes.album_artist];
                mNewData[iLoop].sArtist = mOldData[iLoop][metaDataFieldTypes.artist];
                mNewData[iLoop].sComment = mOldData[iLoop][metaDataFieldTypes.comment];
                mNewData[iLoop].sComposer = mOldData[iLoop][metaDataFieldTypes.composer];
                mNewData[iLoop].iDisk = uint.Parse(mOldData[iLoop][metaDataFieldTypes.disk]);
                mNewData[iLoop].sGenre = mOldData[iLoop][metaDataFieldTypes.genre];
                mNewData[iLoop].iRating = uint.Parse(mOldData[iLoop][metaDataFieldTypes.rating]);
                mNewData[iLoop].sTitle = mOldData[iLoop][metaDataFieldTypes.title];
                mNewData[iLoop].iTotalDisks = uint.Parse(mOldData[iLoop][metaDataFieldTypes.total_disks]);
                mNewData[iLoop].iTotalTracks = uint.Parse(mOldData[iLoop][metaDataFieldTypes.total_tracks]);
                mNewData[iLoop].iTrack = uint.Parse(mOldData[iLoop][metaDataFieldTypes.track]);
                mNewData[iLoop].iYear = uint.Parse(mOldData[iLoop][metaDataFieldTypes.year]);
            }

            return mNewData;
        }

        /// <summary>
        /// Sorts an array of mediaEntries based on meta data types
        /// </summary>
        /// <param name="mMediaEntries">Unsorted media entry array</param>
        /// <param name="mTypes">What to sort by (most significant first)</param>
        /// <param name="sSortOrder">Sort order for each matching metaDataFieldType</param>
        public static void sortMedia(mediaEntry[] mMediaEntries, metaDataFieldTypes[] mTypes, sortOrder sSortOrder)
        {
            sortOrder[] sNewSort = new sortOrder[mTypes.Length];
            for (int iLoop = 0; iLoop < mTypes.Length; iLoop++)
                sNewSort[iLoop] = sSortOrder;

            sortMedia(mMediaEntries, mTypes, sNewSort);
        }

        /// <summary>
        /// Sorts an array of mediaEntries based on meta data types
        /// </summary>
        /// <param name="mMediaEntries">Unsorted media entry array</param>
        /// <param name="mTypes">What to sort by (most significant first)</param>
        /// <param name="sSortOrder">Sort order for each matching metaDataFieldType</param>
        public static void sortMedia(mediaEntry[] mMediaEntries, metaDataFieldTypes[] mTypes, sortOrder[] sSortOrder)
        {
            Array.Sort<mediaEntry>(mMediaEntries, new mediaEntryComparer(mTypes, sSortOrder));
        }

        /// <summary>
        /// Gets the friendly name version of a meta data field
        /// </summary>
        /// <param name="mFieldType">Field to grab</param>
        /// <returns>Friendly string version of the field</returns>
        public static string getFriendlyFieldName(metaDataFieldTypes mFieldType)
        {
            return _sFriendlyNames[calcFieldTypesID(mFieldType)];
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Calculates a non-power 2 version of the field type
        /// </summary>
        /// <param name="mFieldID">Field ID</param>
        /// <returns>Integer Version</returns>
        private static int calcFieldTypesID(metaDataFieldTypes mFieldID)
        {
            if (mFieldID == 0)
                throw new IndexOutOfRangeException("No field type of NONE has no field ID.");

            return (int)Math.Log((double)mFieldID, 2);
        }
        #endregion
    }
}