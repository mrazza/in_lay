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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Runtime.InteropServices;
using netAudio.core;

namespace netAudio.netVLC.core
{
    /// <summary>
    /// VLC Media Object - Media "File"
    /// </summary>
    public sealed class vlcMedia : IVLCCoreUser, ICloneable, IDisposable
    {
        #region vlcMetaTypes
        /// <summary>
        /// VLC Meta Data Types
        /// </summary>
        public enum vlcMetaTypes : int
        {
            /// <summary>
            /// Title of the track
            /// </summary>
            title,

            /// <summary>
            /// Artist of the track
            /// </summary>
            artist,

            /// <summary>
            /// Genre of the track
            /// </summary>
            genre,

            /// <summary>
            /// Copyright information
            /// </summary>
            copyright,

            /// <summary>
            /// Album Name
            /// </summary>
            album,

            /// <summary>
            /// Track Number
            /// </summary>
            trackNumber,

            /// <summary>
            /// Track Description
            /// </summary>
            description,

            /// <summary>
            /// Track Rating
            /// </summary>
            rating,

            /// <summary>
            /// Track Date
            /// </summary>
            date,

            /// <summary>
            /// Track Setting
            /// </summary>
            setting,

            /// <summary>
            /// Track URL
            /// </summary>
            URL,

            /// <summary>
            /// Track Language
            /// </summary>
            language,

            /// <summary>
            /// Now Playing
            /// </summary>
            nowPlaying,

            /// <summary>
            /// Publisher
            /// </summary>
            publisher,

            /// <summary>
            /// Encoded By
            /// </summary>
            encodedBy,

            /// <summary>
            /// Artwork URL
            /// </summary>
            artworkURL,

            /// <summary>
            /// Track ID
            /// </summary>
            trackID
        }
        #endregion

        #region Members
        /// <summary>
        /// Handle to the media object
        /// </summary>
        private vlcMediaHandle _vHandle;

        /// <summary>
        /// Base VLC Core
        /// </summary>
        private vlcCore _vCore;
        #endregion

        #region Properties
        /// <summary>
        /// Path/MRL of the media file
        /// </summary>
        public string sPath
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return "";

                string ret = Marshal.PtrToStringAnsi(libvlc_media_get_mrl(_vHandle, ref _vCore.vException.vExStruct));
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// vCore this media is from
        /// </summary>
        public vlcCore vCore
        {
            get
            {
                return _vCore;
            }
        }

        /// <summary>
        /// Media preparsed status
        /// </summary>
        public bool bIsPreparsed
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return false;

                bool ret = libvlc_media_is_preparsed(_vHandle, ref _vCore.vException.vExStruct) != 0;
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// Length of the track (microseconds)
        /// </summary>
        public long lLength
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return -1;

                long ret = libvlc_media_get_duration(_vHandle, ref _vCore.vException.vExStruct);
                _vCore.handleError();
                return ret;
            }
        }

        /// <summary>
        /// vlcMediaHandle of this object
        /// </summary>
        internal vlcMediaHandle vHandle
        {
            get
            {
                return _vHandle;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="vCore">Root vlcCore Instance</param>
        /// <param name="sPath">Path/MRI to file/stream</param>
        public vlcMedia(vlcCore vCore, string sPath)
        {
            _vCore = vCore;
            _vHandle = libvlc_media_new(_vCore.vHandle, sPath, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }

        /// <summary>
        /// Create a vlcMedia instance from an existing handle
        /// </summary>
        /// <param name="vCore">Root vlcCore Instance</param>
        /// <param name="vHandle">Media handle</param>
        internal vlcMedia(vlcCore vCore, vlcMediaHandle vHandle)
        {
            _vCore = vCore;
            _vHandle = vHandle;
        }

        /// <summary>
        /// Copy data/duplicate constructor
        /// </summary>
        /// <param name="vMedia">Media to copy</param>
        private vlcMedia(vlcMedia vMedia)
        {
            _vCore = vMedia._vCore;
            _vHandle = libvlc_media_duplicate(vMedia._vHandle);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcMedia()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Gets a single meta entry
        /// </summary>
        /// <param name="vMetaType">Meta Data Type</param>
        /// <returns>Meta data of type requested</returns>
        public string getMetaData(vlcMetaTypes vMetaType)
        {
            if (_vHandle.IsInvalid)
                return "";

            string ret = Marshal.PtrToStringAnsi(libvlc_media_get_meta(_vHandle, vMetaType, ref _vCore.vException.vExStruct));
            _vCore.handleError();
            return ret;
        }

        /// <summary>
        /// Gets a meta data struct for the media
        /// </summary>
        /// <returns></returns>
        public metaData getMetaData()
        {
            // If we haven't loaded the data yet just query something random to load it
            if (!bIsPreparsed)
                getMetaData(vlcMetaTypes.title);

            for (int iLoop = 0; !bIsPreparsed && iLoop < 200; iLoop++)
                System.Threading.Thread.Sleep(5);

            metaData mData = new metaData();
            mData.sTitle = getMetaData(vlcMetaTypes.title);
            mData.sArtist = getMetaData(vlcMetaTypes.artist);
            mData.sAlbum = getMetaData(vlcMetaTypes.album);
            mData.sComment = getMetaData(vlcMetaTypes.description);
            mData.sArtURL = getMetaData(vlcMetaTypes.artworkURL);
            mData.sGenre = getMetaData(vlcMetaTypes.genre);

            string rating = getMetaData(vlcMetaTypes.rating), trackNumber = getMetaData(vlcMetaTypes.trackNumber), date = getMetaData(vlcMetaTypes.date);

            if (rating == null || rating == "")
                mData.iRating = 0;
            else
                mData.iRating = uint.Parse(rating);

            if (trackNumber == null || trackNumber == "")
                mData.iTrack = 0;
            else
                mData.iTrack = uint.Parse(trackNumber);

            if (date == null || date == "")
                mData.iYear = 0;
            else
                mData.iYear = uint.Parse(date);

            mData.bContainsData = true;

            return mData;
        }

        /// <summary>
        /// Adds the option to the media discriptor.
        /// </summary>
        /// <param name="sOption">The option.</param>
        public void addOption(string sOption)
        {
            libvlc_media_add_option(_vHandle, sOption, ref _vCore.vException.vExStruct);
            _vCore.handleError();
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Creates a new instance of vlcMedia with the same data
        /// </summary>
        /// <returns>A new vlcMedia object</returns>
        public object Clone()
        {
            return new vlcMedia(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public void Dispose()
        {
            _vHandle.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Creates a new media object with the given path/MRL
        /// </summary>
        /// <param name="vHandle">vlcCore Handle</param>
        /// <param name="sPath">Path to the file/MRL of the file</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Handle to the media object</returns>
        [DllImport("libvlc")]
        private static extern vlcMediaHandle libvlc_media_new(vlcCoreHandle vHandle, string sPath, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the path/MRL of the media file
        /// </summary>
        /// <param name="vHandle">Media handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns></returns>
        [DllImport("libvlc")]
        private static extern IntPtr libvlc_media_get_mrl(vlcMediaHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Duplicates a media handle
        /// </summary>
        /// <param name="vHandle">Media handle to duplicate</param>
        /// <returns>New handle</returns>
        [DllImport("libvlc")]
        private static extern vlcMediaHandle libvlc_media_duplicate(vlcMediaHandle vHandle);

        /// <summary>
        /// Checks the media's preparsed status
        /// </summary>
        /// <param name="vHandle">Media handle</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>true if preparsed, otherwise 0</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_media_is_preparsed(vlcMediaHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Read the meta data of the media
        /// </summary>
        /// <param name="vHandle">Media handle</param>
        /// <param name="vMetaType">Meta data type to read</param>
        /// <param name="vException">Exception struct</param>
        /// <returns>Meta data</returns>
        [DllImport("libvlc")]
        private static extern IntPtr libvlc_media_get_meta(vlcMediaHandle vHandle, vlcMetaTypes vMetaType, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Get the length of the track
        /// </summary>
        /// <param name="vHandle">Media Handle</param>
        /// <param name="vException">Exception Struct</param>
        /// <returns>Length of the track (milliseconds)</returns>
        [DllImport("libvlc")]
        private static extern long libvlc_media_get_duration(vlcMediaHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Adds an option to the media discriptor
        /// </summary>
        /// <param name="vHandle">Media Handle</param>
        /// <param name="sOption">Option to add</param>
        /// <param name="vException">Exception struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_add_option(vlcMediaHandle vHandle, string sOption, ref vlcException.vlcExceptionStruct vException);
        #endregion
    }
}
