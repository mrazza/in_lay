/*******************************************************************
 * This file is part of the netGooey library.
 * 
 * netGooey source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netGooey is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netGooey.core
{
    /// <summary>
    /// Interface required for all netGooey Control Interactions
    /// </summary>
    public interface IGooeyControl : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the gooeySystem.
        /// </summary>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        /// <value>The gooeySystem.</value>
        gooeySystem gSystem
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the gooeyWindow.
        /// </summary>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        /// <value>The gooeyWindow.</value>
        gooeyWindow gWindow
        {
            set;
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool isGooeyInitialized
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when [initialization complete].
        /// </summary>
        /// <remarks>This function must fail if the object has already been initialized.</remarks>
        void onGooeyInitializationComplete();
        #endregion
    }
}
