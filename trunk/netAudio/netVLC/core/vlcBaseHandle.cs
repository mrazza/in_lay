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
using System.Runtime.InteropServices;

namespace netAudio.netVLC.core
{
    /// <summary>
    /// Internal base handle class
    /// </summary>
    internal abstract class vlcBaseHandle : SafeHandle, IDisposable
    {
        #region Properties
        /// <summary>
        /// Is the handle invalid?
        /// </summary>
        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Base Constructor
        /// </summary>
        public vlcBaseHandle()
            : base(IntPtr.Zero, true) { }

        /// <summary>
        /// Creates a handle from a previous one
        /// </summary>
        /// <param name="vHandle">Previous handle</param>
        public vlcBaseHandle(vlcBaseHandle vHandle)
            : base(IntPtr.Zero, true)
        {
            handle = vHandle.handle;
            RetainHandle();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor
        /// </summary>
        ~vlcBaseHandle()
        {
            Dispose();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Retains the handle
        /// </summary>
        public abstract void RetainHandle();
        #endregion

        #region Protected Members
        /// <summary>
        /// Dispose of the handle
        /// </summary>
        /// <param name="disposing">Perform a normal dispose? (set to true)</param>
        protected override void Dispose(bool disposing)
        {
            ReleaseHandle(); //We need to release the handle before the rest of dispose is called
            GC.SuppressFinalize(this);
            base.Dispose(disposing);
        }
        #endregion
    }
}
