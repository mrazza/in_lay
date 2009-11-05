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
using inlayShared.core;
using inlayShared.ui.controls.core;
using netAudio.core;
using netGooey.core;

namespace in_lay.core
{
    /// <summary>
    /// Manages a window and netAudio player for in_lay
    /// </summary>
    public class inlayWindowManager
    {
        #region Members
        /// <summary>
        /// inlayComponentSystem for the application
        /// </summary>
        private inlayComponentSystem _iSystem;

        /// <summary>
        /// Window (main window) we are managing
        /// </summary>
        private gooeyWindow _gWindow;
        #endregion

        #region Properties
        /// <summary>
        /// Shared inlayComponentSystem instance
        /// </summary>
        public inlayComponentSystem iSystem
        {
            get
            {
                return _iSystem;
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
        /// Initializes a new instance of the <see cref="inlayWindowManager"/> class.
        /// </summary>
        /// <param name="iSystem">The i system.</param>
        public inlayWindowManager(inlayComponentSystem iSystem)
        {
            if (iSystem == null)
                throw new NullReferenceException("iSystem is not set to an instance of an object.");

            _iSystem = iSystem;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Generates and uses the window
        /// </summary>
        /// <returns>New netGooey Window</returns>
        public void generateWindow()
        {
            if (_gWindow != null)
                return;

            _gWindow = _iSystem.gSystem.createWindow(); 
        }

        /// <summary>
        /// Generates and uses the window.
        /// </summary>
        /// <param name="sFile">The path to the UI file.</param>
        /// <returns>New netGooey Window</returns>
        public void generateWindow(string sFile)
        {
            if (_gWindow != null)
                return;

            _gWindow = _iSystem.gSystem.createWindow(sFile, (gooeySystem.initializationDelegate)initUIElements);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Inits the UI elements.
        /// </summary>
        /// <param name="uiElement">The UI element.</param>
        private void initUIElements(IGooeyControl uiElement)
        {
            ((IPlayerControl)uiElement).iSystem = _iSystem;
        }
        #endregion
    }
}
