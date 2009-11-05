/*******************************************************************
 * This file is part of the netAudio library.
 * 
 * netAudio source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netAudio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using netAudio.core.tagLib;

namespace netAudio.core
{
    /// <summary>
    /// Provides the functions and methods required to read
    /// the metaData from audio files (via tagLib)
    /// </summary>
    public sealed class metaDataManager
    {
        #region Members
        /// <summary>
        /// Current File Selected
        /// </summary>
        private string _sCurrentFile;

        /// <summary>
        /// tagLib instance of the file
        /// </summary>
        private File _fCurrentFile;

        /// <summary>
        /// Does the class contain the correct data? (was it loaded via tagLib)
        /// </summary>
        private bool _bIsCorrectData;

        /// <summary>
        /// Did we totally fail?
        /// </summary>
        private bool _bTotalFail;

        /// <summary>
        /// Current file's meta data
        /// </summary>
        private metaData _mFileData;

        /// <summary>
        /// netAudio Player to use to grab metadata
        /// </summary>
        private netAudioPlayer _nPlayer;
        #endregion

        #region Properties
        /// <summary>
        /// Current file we're reading
        /// </summary>
        public string sFile
        {
            get
            {
                return _sCurrentFile;
            }
            set
            {
                setFile(value);
            }
        }

        /// <summary>
        /// Gets the meta data
        /// </summary>
        public metaData mData
        {
            get
            {
                if (_bTotalFail)
                    return new metaData();

                if (!_mFileData.bContainsData)
                    loadClass();

                return _mFileData;
            }
            set
            {
                _mFileData = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that takes a init file
        /// </summary>
        /// <param name="sFile">File to open</param>
        /// <param name="player">Player to use if needed to grab meta data (can be null to not use one)</param>
        public metaDataManager(netAudioPlayer player, string sFile)
            : this(player)
        {
            setFile(sFile);
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="player">Player to use if needed to grab meta data (can be null to not use one)</param>
        public metaDataManager(netAudioPlayer player)
        {
            _sCurrentFile = "";
            _fCurrentFile = null;
            _bIsCorrectData = false;
            _mFileData = new metaData();
            _mFileData.bContainsData = true;
            _nPlayer = player;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves any changes
        /// </summary>
        public void save()
        {
            setClass(_mFileData);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the current file
        /// </summary>
        /// <param name="sFile">File to set</param>
        private void setFile(string sFile)
        {
            _sCurrentFile = sFile;
            _bIsCorrectData = false;
            _mFileData.bContainsData = false;
            _bTotalFail = false;
        }

        /// <summary>
        /// Fills the class with data
        /// </summary>
        private void loadClass()
        {
            // No need to fill it twice
            if (_bIsCorrectData || _bTotalFail)
                return;
            try
            {
                _fCurrentFile = File.Create(sFile);
                _mFileData.iDisk = _fCurrentFile.Tag.Disc;
                _mFileData.iRating = 0;
                _mFileData.iTotalDisks = _fCurrentFile.Tag.DiscCount;
                _mFileData.iTotalTracks = _fCurrentFile.Tag.TrackCount;
                _mFileData.iTrack = _fCurrentFile.Tag.Track;
                _mFileData.iYear = _fCurrentFile.Tag.Year;
                _mFileData.sAlbum = _fCurrentFile.Tag.Album;
                _mFileData.sAlbumArtist = stringImplode(_fCurrentFile.Tag.AlbumArtists, "\n");
                _mFileData.sArtist = stringImplode(_fCurrentFile.Tag.Performers, "\n");
                _mFileData.sComment = _fCurrentFile.Tag.Comment;
                _mFileData.sComposer = stringImplode(_fCurrentFile.Tag.Composers, "\n");
                _mFileData.sGenre = stringImplode(_fCurrentFile.Tag.Genres, "\n");
                _mFileData.sTitle = _fCurrentFile.Tag.Title;
                _mFileData.tLength = _fCurrentFile.Properties.Duration;
                _mFileData.bContainsData = true;
                _bIsCorrectData = true;
                _bTotalFail = false;

                // If the tags were bad and have now been repaired
                if (_mFileData.iTotalDisks != _fCurrentFile.Tag.DiscCount || _mFileData.iTotalTracks != _fCurrentFile.Tag.TrackCount)
                {
                    _mFileData.iTotalDisks = _fCurrentFile.Tag.DiscCount;
                    _mFileData.iTotalTracks = _fCurrentFile.Tag.TrackCount;
                    save(); //Save the repair
                }
            }
            catch (Exception e)
            {
                _bIsCorrectData = false; //Taglib failed

                if (_nPlayer == null)
                {
                    Console.WriteLine("Could not fetch meta data for track (" + getFileName(sFile) + "): " + e.Message);
                    _bTotalFail = true;
                    return;
                }

                // Let's try to salvage this:
                _nPlayer.sMediaPath = sFile;
                if (_nPlayer.mTrackData == null)
                {
                    Console.WriteLine("Could not fetch meta data for track " + sFile + ". (netAudio)");
                    _bTotalFail = true;
                    return;
                }
                _mFileData = _nPlayer.mTrackData;
                _bTotalFail = false;
            }
        }

        /// <summary>
        /// Fills the class with data
        /// </summary>
        private void setClass(metaData newData)
        {
            // Make sure we're loaded and ready
            if (_bTotalFail)
                return;

            if (!_mFileData.bContainsData)
                loadClass();

            if (!_bIsCorrectData)
                return;

            try
            {
                _fCurrentFile.Tag.Disc = newData.iDisk;
                _fCurrentFile.Tag.DiscCount = newData.iTotalDisks;
                _fCurrentFile.Tag.TrackCount = newData.iTotalTracks;
                _fCurrentFile.Tag.Track = newData.iTrack;
                _fCurrentFile.Tag.Year = newData.iYear;
                _fCurrentFile.Tag.Album = newData.sAlbum;
                _fCurrentFile.Tag.AlbumArtists = stringExplode(newData.sAlbumArtist, "\n");
                _fCurrentFile.Tag.Performers = stringExplode(newData.sArtist, "\n");
                _fCurrentFile.Tag.Comment = newData.sComment;
                _fCurrentFile.Tag.Composers = stringExplode(newData.sComposer, "\n");
                _fCurrentFile.Tag.Genres = stringExplode(newData.sGenre, "\n");
                _fCurrentFile.Tag.Title = newData.sTitle;
                _fCurrentFile.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not set meta data: " + e.ToString());
            }
        }

        /// <summary>
        /// Explodes a string array into a single string
        /// </summary>
        /// <param name="input">Input array</param>
        /// <param name="delim">Delimiter</param>
        /// <returns>String from array</returns>
        private string stringImplode(string[] input, string delim)
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

            foreach (string curr in input)
            {
                sBuilder.Append(curr);
                sBuilder.Append(delim);
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Implodes a string into an array
        /// </summary>
        /// <param name="data">Input data</param>
        /// <param name="delim">Delimiter</param>
        /// <returns>String array from string</returns>
        private string[] stringExplode(string data, string delim)
        {
            if (data == null || delim == null)
                return new string[0];

            int iCount = 0, iCurrPos = 0;

            while (iCurrPos < data.Length && (iCurrPos = data.IndexOf(delim, iCurrPos)) != -1)
            {
                iCurrPos += delim.Length;
                iCount++;
            }

            string[] sRet = new string[iCount];

            iCount = 0;
            iCurrPos = 0;
            int iPrev = 0;

            while (iCurrPos < data.Length && (iCurrPos = data.IndexOf(delim, iCurrPos)) != -1)
            {
                sRet[iCount++] = data.Substring(iPrev, iCurrPos - iPrev);

                iCurrPos += delim.Length;
                iPrev = iCurrPos;
            }

            return sRet;
        }

        /// <summary>
        /// Returns the filename from the path
        /// </summary>
        /// <param name="sPath">Path of the file</param>
        /// <returns>Just the file</returns>
        private string getFileName(string sPath)
        {
            int iStartIndex = sPath.LastIndexOf("\\");

            // No file extension
            if (iStartIndex < 0)
                return "";

            return sPath.Substring(iStartIndex);
        }
        #endregion
    }
}
