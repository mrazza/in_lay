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
    /// VLC Exception Manager
    /// libvlc_exception_t Struct Manager
    /// </summary>
    internal sealed class vlcException : IDisposable
    {
        #region VLC Exception Struct
        /// <summary>
        /// VLC Exception Data Struct
        /// libvlc_exception_t Struct
        /// </summary>
        public struct vlcExceptionStruct
        {
            /// <summary>
            /// Boolean, is raised
            /// </summary>
            private int bRaised;

            /// <summary>
            /// Exception code
            /// </summary>
            private int iCode;

            /// <summary>
            /// Exception message
            /// </summary>
            private string sMessage;
        }
        #endregion

        #region Members
        /// <summary>
        /// VLC Exception Struct
        /// </summary>
        public vlcExceptionStruct vExStruct;
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        public vlcException()
        {
            vExStruct = new vlcExceptionStruct();
            initException();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcException()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Has the exception been raised?
        /// </summary>
        /// <returns>Returns true if it has been raised, otherwise false</returns>
        public bool raisedException()
        {
            return libvlc_exception_raised(ref vExStruct) != 0;
        }

        /// <summary>
        /// Gets the exception message
        /// </summary>
        /// <returns>The exception message</returns>
        public string getExceptionMessage()
        {
            return libvlc_exception_get_message(ref vExStruct);
        }

        /// <summary>
        /// Clears the exception struct
        /// </summary>
        public void clearException()
        {
            libvlc_exception_clear(ref vExStruct);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Initializes the exception struct
        /// </summary>
        private void initException()
        {
            libvlc_exception_init(ref vExStruct);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes any allocated resources
        /// </summary>
        public void Dispose()
        {
            clearException();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Initializes an exception struct
        /// </summary>
        /// <param name="vExStruct">The structure to initialize</param>
        [DllImport("libvlc")]
        private static extern void libvlc_exception_init(ref vlcExceptionStruct vExStruct);

        /// <summary>
        /// Has the exception been raised?
        /// </summary>
        /// <param name="vExStruct">The structure to check</param>
        /// <returns>0 if the exception was raised, otherwise 1</returns>
        [DllImport("libvlc")]
        private static extern int libvlc_exception_raised(ref vlcExceptionStruct vExStruct);

        /// <summary>
        /// Clears an exception object so that it can be reused
        /// </summary>
        /// <param name="vExStruct">The exception to clear</param>
        [DllImport("libvlc")]
        private static extern void libvlc_exception_clear(ref vlcExceptionStruct vExStruct);

        /// <summary>
        /// Gets an exception message
        /// </summary>
        /// <param name="vExStruct">The exception to query</param>
        /// <returns>The exception message</returns>
        [DllImport("libvlc")]
        private static extern string libvlc_exception_get_message(ref vlcExceptionStruct vExStruct);
        #endregion
    }
}
