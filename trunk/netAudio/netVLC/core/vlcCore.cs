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

namespace netAudio.netVLC.core
{
    /// <summary>
    /// Core interactions with the libVLC DLL
    /// Controls the output of media; setting changes here apply to ALL players
    /// </summary>
    public sealed class vlcCore : IDisposable
    {
        #region Members
        /// <summary>
        /// Instance of VLC
        /// </summary>
        private vlcCoreHandle _vHandle;

        /// <summary>
        /// Internal vlcException (last exception)
        /// </summary>
        private vlcException _vException;
        #endregion

        #region Properties
        /// <summary>
        /// libVLC Instance Handle
        /// </summary>
        internal vlcCoreHandle vHandle
        {
            get
            {
                return _vHandle;
            }
        }

        /// <summary>
        /// Core exception struct
        /// </summary>
        internal vlcException vException
        {
            get
            {
                return _vException;
            }
        }

        /// <summary>
        /// The libVLC version
        /// </summary>
        public string sVersion
        {
            get
            {
                return libvlc_get_version();
            }
        }

        /// <summary>
        /// The libVLC compiler
        /// </summary>
        public string sCompiler
        {
            get
            {
                return libvlc_get_compiler();
            }
        }

        /// <summary>
        /// The libVLC changeset
        /// I.E. "aa9bce0bc3"
        /// </summary>
        public string sChangeset
        {
            get
            {
                return Marshal.PtrToStringAnsi(libvlc_get_changeset());
            }
        }

        /// <summary>
        /// Sound Mute Status
        /// </summary>
        public bool bMute
        {
            get
            {
                if (_vHandle.IsInvalid)
                    return false;

                bool ret = libvlc_audio_get_mute(_vHandle, ref _vException.vExStruct) != 0;
                handleError();
                return ret;
            }
            set
            {
                if (_vHandle.IsInvalid)
                    return;

                libvlc_audio_set_mute(_vHandle, value ? 1 : 0, ref _vException.vExStruct);
                handleError();
            }
        }

        /// <summary>
        /// Volume
        /// </summary>
        public int iVolume
        {
            get
            {
                int ret = libvlc_audio_get_volume(_vHandle, ref _vException.vExStruct);
                handleError();
                return ret;
            }
            set
            {
                libvlc_audio_set_volume(_vHandle, value, ref _vException.vExStruct);
                handleError();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="argv">Arguments to pass</param>
        public vlcCore(string[] argv)
        {
            _vException = new vlcException();
            _vHandle = libvlc_new(argv.Length, argv, ref _vException.vExStruct);
            handleError();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcCore()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Checks if there has been an error
        /// </summary>
        /// <returns>Returns true if there has been an error, otherwise false</returns>
        public bool hasError()
        {
            return _vException.raisedException();
        }

        /// <summary>
        /// Gets the last error and clears it
        /// </summary>
        /// <returns>String representation of the error</returns>
        public string getLastError()
        {
            return getLastError(true);
        }

        /// <summary>
        /// Gets the last error
        /// </summary>
        /// <param name="bClearError">If true the error is cleared</param>
        /// <returns>String representation of the error</returns>
        public string getLastError(bool bClearError)
        {
            string sError = _vException.getExceptionMessage();

            if (bClearError)
                _vException.clearException();

            return sError;
        }

        /// <summary>
        /// Handles an error if one exists
        /// </summary>
        public void handleError()
        {
            if (!hasError())
                return;

            Console.WriteLine(getLastError());
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public void Dispose()
        {
            _vHandle.Dispose();
            _vException.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Creates an instance of VLC
        /// </summary>
        /// <param name="argc">Argument Count</param>
        /// <param name="argv">Argument Values</param>
        /// <param name="vException">Exception Struct</param>
        /// <returns>vlcCoreHandle instance</returns>
        [DllImport("libvlc")]
        private static extern vlcCoreHandle libvlc_new(int argc, string[] argv, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the libVLC version
        /// </summary>
        /// <returns>A string with the version of libVLC</returns>
        [DllImport("libvlc")]
        private static extern string libvlc_get_version();

        /// <summary>
        /// Retrive the libVLC compiler version
        /// </summary>
        /// <returns>A string with the compiler version</returns>
        [DllImport("libvlc")]
        private static extern string libvlc_get_compiler();

        /// <summary>
        /// Retrive the libVLC changeset
        /// I.E. "aa9bce0bc4"
        /// </summary>
        /// <returns>A string with the libvlc changeset</returns>
        [DllImport("libvlc")]
        private static extern IntPtr libvlc_get_changeset();

        /// <summary>
        /// Gets the current mute status
        /// </summary>
        /// <param name="vHandle">Handle to the core object</param>
        /// <param name="vException">Exception Struct</param>
        /// <returns>Returns 1 if muted, otherwise 0</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_audio_get_mute(vlcCoreHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the mute state
        /// </summary>
        /// <param name="vHandle">Handle to core object</param>
        /// <param name="iStatus">Status, 1 for muted, otherwise 0</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_audio_set_mute(vlcCoreHandle vHandle, int iStatus, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Gets the current volume setting
        /// </summary>
        /// <param name="vHandle">Handle to core object</param>
        /// <param name="vException">Exception Struct</param>
        /// <returns>Returns the volume setting</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_audio_get_volume(vlcCoreHandle vHandle, ref vlcException.vlcExceptionStruct vException);

        /// <summary>
        /// Sets the volume
        /// </summary>
        /// <param name="vHandle">Handle to the core object</param>
        /// <param name="iVolume">Volume setting</param>
        /// <param name="vException">Exception Struct</param>
        [DllImport("libvlc")]
        private static extern void libvlc_audio_set_volume(vlcCoreHandle vHandle, int iVolume, ref vlcException.vlcExceptionStruct vException);
        #endregion
    }
}
