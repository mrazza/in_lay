/*******************************************************************
 * This file is part of the in_lay Player.
 * 
 * in_lay Player source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * in_lay Player is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using in_lay_Shared.ui.controls.core;
using netAudio.core;
using netGooey.core;

namespace in_lay.core
{
    /// <summary>
    /// Manages the netAudio player for in_lay
    /// <remarks>Make sure the inlayPlayerManager and the gooeyWindow are created on the same thread</remarks>
    /// </summary>
    public class inlayPlayerManager
    {
        #region Members
        /// <summary>
        /// Player we are managing
        /// </summary>
        private netAudioPlayer _nPlayer;

        /// <summary>
        /// Window (main window) we are managing
        /// </summary>
        private gooeyWindow _gWindow;
        #endregion

        #region Properties
        /// <summary>
        /// netAudioPlayer used for this instance of inlay
        /// </summary>
        public netAudioPlayer nPlayer
        {
            get
            {
                return _nPlayer;
            }
        }

        /// <summary>
        /// Main gooeyWindow used for this instance of inlay
        /// </summary>
        public gooeyWindow gWindow
        {
            get
            {
                return _gWindow;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="nPlayer">netAudioPlayer to use</param>
        public inlayPlayerManager(netAudioPlayer nPlayer)
        {
            if (nPlayer == null)
                throw new NullReferenceException("nPlayer is not set to an instance of an object.");

            _nPlayer = nPlayer;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Generates and uses the window
        /// </summary>
        /// <param name="gSystem">The netGooey system.</param>
        /// <returns>New netGooey Window</returns>
        public void generateWindow(gooeySystem gSystem)
        {
            if (_gWindow != null)
                return;

            _gWindow = gSystem.createWindow(); 
        }

        /// <summary>
        /// Generates and uses the window.
        /// </summary>
        /// <param name="gSystem">The netGooey system.</param>
        /// <param name="sFile">The path to the UI file.</param>
        /// <returns>New netGooey Window</returns>
        public void generateWindow(gooeySystem gSystem, string sFile)
        {
            if (_gWindow != null)
                return;

            _gWindow = gSystem.createWindow(sFile, (gooeySystem.initializationDelegate)initUIElements);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Invokes on the GUI's local thread
        /// </summary>
        /// <param name="dFunction">Function to invoke</param>
        private void invokeOnLocalThread(Delegate dFunction)
        {
            _gWindow.gSystem.invokeOnLocalThread(dFunction);
        }

        /// <summary>
        /// Inits the UI elements.
        /// </summary>
        /// <param name="uiElement">The UI element.</param>
        private void initUIElements(IGooeyControl uiElement)
        {
            ((IPlayerControl)uiElement).nPlayer = _nPlayer;
        }
        #endregion
    }
}
