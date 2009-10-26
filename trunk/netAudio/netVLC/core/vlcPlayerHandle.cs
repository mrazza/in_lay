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
    /// Handle to an instance of the VLC player
    /// </summary>
    internal sealed class vlcPlayerHandle : vlcBaseHandle
    {
        #region Public Members
        /// <summary>
        /// Retains the handle
        /// </summary>
        public override void RetainHandle()
        {
            libvlc_media_player_retain(this);
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// Releases the handle
        /// </summary>
        /// <returns>Returns true if success</returns>
        protected override bool ReleaseHandle()
        {
            if (!IsInvalid)
            {
                libvlc_media_player_release(this);
                handle = IntPtr.Zero;
            }

            return true;
        }
        #endregion

        #region libVLC DLL Imports
        /// <summary>
        /// Releases an instance of the VLC Player
        /// </summary>
        /// <param name="vHandle">Handle to release</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_release(vlcPlayerHandle vHandle);

        /// <summary>
        /// Retains an instance of the VLC Player
        /// </summary>
        /// <param name="vHandle">Handle to retain</param>
        [DllImport("libvlc")]
        private static extern void libvlc_media_player_retain(vlcPlayerHandle vHandle);
        #endregion
    }
}
