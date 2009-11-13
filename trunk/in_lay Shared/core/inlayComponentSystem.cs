/*******************************************************************
 * This file is part of the in_lay Player Shared Library.
 * 
 * in_lay source may be distributed or modified without
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
using System.Threading;
using netAudio.core;
using netDiscographer.core;
using netGooey.core;

namespace inlayShared.core
{
    /// <summary>
    /// This class manages required objects and systems for all in_lay related UI and other controls
    /// </summary>
    public sealed class inlayComponentSystem
    {
        #region Members
        /// <summary>
        /// Shared netAudio Player
        /// </summary>
        private netAudioPlayer _nPlayer;

        /// <summary>
        /// Shared netGooey System
        /// </summary>
        private gooeySystem _gSystem;

        /// <summary>
        /// Main netDiscographer Database
        /// </summary>
        private discographerDatabase _dDatabase;

        /// <summary>
        /// netDiscographer Database System
        /// </summary>
        private discographerSystem _dSystem;
        #endregion

        #region Properties
        /// <summary>
        /// Shared netAudio Player
        /// </summary>
        public netAudioPlayer nPlayer
        {
            get
            {
                return _nPlayer;
            }
        }

        /// <summary>
        /// Shared netGooey System
        /// </summary>
        public gooeySystem gSystem
        {
            get
            {
                return _gSystem;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="inlayComponentSystem"/> class.
        /// </summary>
        /// <param name="nPlayer">The n player.</param>
        /// <param name="gSystem">The g system.</param>
        /// <param name="dDatabase">The d database.</param>
        public inlayComponentSystem(netAudioPlayer nPlayer, gooeySystem gSystem, discographerDatabase dDatabase)
        {
            if (nPlayer == null)
                throw new NullReferenceException("nPlayer can not be null when creating an inlayComponentSystem object.");

            if (gSystem == null)
                throw new NullReferenceException("gSystem can not be null when creating an inlayComponentSystem object.");

            if (dDatabase == null)
                throw new NullReferenceException("dDatabase can not be null when creating an inlayComponentSystem object.");

            _nPlayer = nPlayer;
            _gSystem = gSystem;
            _dDatabase = dDatabase;
        }
        #endregion
    }
}
