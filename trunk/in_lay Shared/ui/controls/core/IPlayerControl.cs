﻿/*******************************************************************
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

using netAudio.core;
using netGooey.core;
using System.Windows;
using in_lay_Shared.ui.controls.core;
using netAudio.core.events;
using System;

namespace in_lay_Shared.ui.controls.core
{
    /// <summary>
    /// Interface common to all inlay controls that require access to player functionality
    /// </summary>
    public interface IPlayerControl : IGooeyControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets the netAudioPlayer.
        /// </summary>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        /// <value>The netAudioPlayer.</value>
        netAudioPlayer nPlayer
        {
            get;
            set;
        }
        #endregion
    }
}
