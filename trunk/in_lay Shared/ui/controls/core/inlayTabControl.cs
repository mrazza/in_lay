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
using inlayShared.core;
using netGooey.controls;

namespace inlayShared.ui.controls.core
{
    /// <summary>
    /// Abstract in_lay TabControl Implementation
    /// </summary>
    public abstract class inlayTabControl : gooeyTabControl, IPlayerControl
    {
        #region Members
        /// <summary>
        /// Shared inlayComponentSystem
        /// </summary>
        protected inlayComponentSystem _iSystem;
        #endregion

        #region Properties
        #region IPlayerControl Members
        /// <summary>
        /// Gets or sets the inlayComponentSystem.
        /// </summary>
        /// <value>The inlayComponentSystem.</value>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        public virtual inlayComponentSystem iSystem
        {
            get
            {
                return _iSystem;
            }
            set
            {
                if (_iSystem != null)
                    throw new InvalidOperationException("Can not change the value of iSystem once it has been set.");

                _iSystem = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>If overridden, base.isGooeyInitialized must return <c>true</c> for the child version to return <c>true</c>.</remarks>
        public override bool isGooeyInitialized
        {
            get
            {
                return base.isGooeyInitialized && (_iSystem != null);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="inlayTabControl"/> class.
        /// </summary>
        public inlayTabControl()
            : base()
        {
            _iSystem = null;
        }
        #endregion
    }
}
