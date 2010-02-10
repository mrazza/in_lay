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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using netDiscographer.core;
using inlayShared.ui.controls.library.core;

namespace inlayShared.ui.controls.library
{
    /// <summary>
    /// GridView used to display the main library
    /// </summary>
    public sealed class libraryGridView : mediaGridView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="libraryGridView"/> class.
        /// </summary>
        public libraryGridView()
            : base()
        {
            mFields = new metaDataFieldTypes[] { metaDataFieldTypes.track, metaDataFieldTypes.title, metaDataFieldTypes.artist, metaDataFieldTypes.album };
        }
        #endregion
    }
}
