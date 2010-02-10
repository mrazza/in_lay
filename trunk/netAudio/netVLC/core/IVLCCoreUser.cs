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

namespace netAudio.netVLC.core
{
    /// <summary>
    /// Identifies a class as a user of the vlcCore class
    /// </summary>
    internal interface IVLCCoreUser
    {
        #region Properties
        /// <summary>
        /// vlcCore the object is a member of
        /// </summary>
        vlcCore vCore
        {
            get;
        }
        #endregion
    }
}
