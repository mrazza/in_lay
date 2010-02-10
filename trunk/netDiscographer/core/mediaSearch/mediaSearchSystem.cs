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
using System.IO;
using System.Collections.Generic;
using netAudio.core;
using netAudio.core.tagLib;

namespace netDiscographer.core.mediaSearch
{
    /// <summary>
    /// This class is used to gather tracks and meta data from the system
    /// </summary>
    public class mediaSearchSystem
    {
        #region Members
        /// <summary>
        /// Event invoked when the stage of the search changes or the progress on the current stage changes
        /// </summary>
        public event EventHandler<mediaSearchProgressEventArgs> eSearchProgress;

        /// <summary>
        /// Valid Extensions
        /// </summary>
        public static readonly string[] sValidExtensions = { ".aac", ".ac3", ".m4a", ".amr", ".xm", ".flac",
                                                               ".mod", ".mp3", ".pls", ".ra", ".ram", ".spx",
                                                               ".tta", ".ogg", ".oga", ".ogx", ".spx", ".wv",
                                                               ".wav", ".wma", ".s3m", ".mp4", ".midi", ".mid",
                                                               ".aiff", ".aif", ".aifc", ".mka", ".mks", ".3gp",
                                                               ".3g2", ".it", ".s3m" };
        #endregion

        #region Public Members
        /// <summary>
        /// Searches a directory for media
        /// </summary>
        /// <param name="nPlayer">netAudioPlayer to use as backup; can be null</param>
        /// <param name="sPath">Path to the directory</param>
        /// <returns>Media found</returns>
        public mediaEntry[] searchDirectory(netAudioPlayer nPlayer, string sPath)
        {
            return searchDirectory(nPlayer, sPath, true, DateTime.Now);
        }

        /// <summary>
        /// Searches a directory for media
        /// </summary>
        /// <param name="nPlayer">netAudioPlayer to use as backup; can be null</param>
        /// <param name="sPath">Path to the directory</param>
        /// <param name="bIncludeSubFolders">Search subfolders of that directory?</param>
        /// <returns>Media found</returns>
        public mediaEntry[] searchDirectory(netAudioPlayer nPlayer, string sPath, bool bIncludeSubFolders)
        {
            return searchDirectory(nPlayer, sPath, bIncludeSubFolders, DateTime.Now);
        }

        /// <summary>
        /// Searches a directory for media
        /// </summary>
        /// <param name="nPlayer">netAudioPlayer to use as backup; can be null</param>
        /// <param name="sPath">Path to the directory</param>
        /// <param name="bIncludeSubFolders">Search subfolders of that directory?</param>
        /// <param name="dTimeAdded">Time added to use</param>
        /// <returns>Media found</returns>
        public mediaEntry[] searchDirectory(netAudioPlayer nPlayer, string sPath, bool bIncludeSubFolders, DateTime dTimeAdded)
        {
            //Trigger starting event
            statusUpdate(new mediaSearchProgressEventArgs(mediaSearchStage.starting, 0, 0));

            // Create variables and collections
            metaDataManager mDataMgr = new metaDataManager(nPlayer);
            LinkedList<mediaEntry> mTotalEntries = new LinkedList<mediaEntry>();
            SearchOption sSearchOption = SearchOption.AllDirectories;
            mediaEntry mCurrEntry;

            if (!bIncludeSubFolders)
                sSearchOption = SearchOption.TopDirectoryOnly;

            statusUpdate(new mediaSearchProgressEventArgs(mediaSearchStage.gatheringFiles, 0, 0)); //Trigger file search event
            string[] sFiles = Directory.GetFiles(sPath, "*", sSearchOption);

            // Start actual search
            int iLoop = 0;
            foreach (string sCurrFile in sFiles)
            {
                statusUpdate(new mediaSearchProgressEventArgs(mediaSearchStage.gatheringMetaData, ++iLoop, sFiles.Length));

                if (!hasValidExtension(sCurrFile))
                    continue;

                // Gather the data
                mDataMgr.sFile = sCurrFile;
                mCurrEntry = mediaEntry.convertFromNetAudio(mDataMgr.mData);

                if (mCurrEntry[metaDataFieldTypes.title] == "" && mCurrEntry[metaDataFieldTypes.artist] == "" && mCurrEntry[metaDataFieldTypes.album] == "")
                    mCurrEntry[metaDataFieldTypes.title] = getFileName(sCurrFile);

                mCurrEntry.dTimeAdded = dTimeAdded;
                mCurrEntry.sPath = sCurrFile;

                mTotalEntries.AddLast(mCurrEntry);
            }

            mediaEntry[] mAllMedia = new mediaEntry[mTotalEntries.Count];
            mTotalEntries.CopyTo(mAllMedia, 0);

            statusUpdate(new mediaSearchProgressEventArgs(mediaSearchStage.finished, 0, 0)); //Trigger finished event

            return mAllMedia;
        }

        /// <summary>
        /// Checks if a file has a valid extension
        /// </summary>
        /// <param name="sPath">Path/File</param>
        /// <returns>True if it's valid, otherwise false</returns>
        public static bool hasValidExtension(string sPath)
        {
            int iStartIndex = sPath.LastIndexOf(".");

            // No file extension
            if (iStartIndex < 0)
                return false;

            string sExtension = sPath.Substring(iStartIndex).ToLower();
            bool bRet = false;

            // Do the check
            foreach (string sCurr in sValidExtensions)
            {
                if (sExtension.Equals(sCurr))
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }

        /// <summary>
        /// Returns the filename from the path
        /// </summary>
        /// <param name="sPath">Path of the file</param>
        /// <returns>Just the file</returns>
        public static string getFileName(string sPath)
        {
            int iStartIndex = sPath.LastIndexOf("\\");

            // No file extension
            if (iStartIndex < 0)
                return "";

            return sPath.Substring(iStartIndex);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Triggers a search progress update
        /// </summary>
        /// <param name="mEventArgs">Event args for the update</param>
        private void statusUpdate(mediaSearchProgressEventArgs mEventArgs)
        {
            if (eSearchProgress != null)
                eSearchProgress.Invoke(this, mEventArgs);
        }
        #endregion
    }
}
