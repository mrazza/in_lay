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
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Windows.Controls;
using netGooey.core;

namespace netGooey.controls
{
    /// <summary>
    /// Abstract netGooey Slider Control
    /// </summary>
    public abstract class gooeySlider : Slider, IGooeyControl
    {
        #region Members
        /// <summary>
        /// gooeySystem Root
        /// </summary>
        protected gooeySystem _gSystem;

        /// <summary>
        /// gooeyWindow we are a member of
        /// </summary>
        protected gooeyWindow _gWindow;

        /// <summary>
        /// Has "onGooeyInitializationComplete" been executed
        /// </summary>
        protected bool _isInitializationComplete;
        #endregion

        #region Properties
        #region IGooeyControl Members
        /// <summary>
        /// Gets the gooeySystem.
        /// </summary>
        /// <value>The gooeySystem.</value>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        public virtual gooeySystem gSystem
        {
            get
            {
                return _gSystem;
            }
            set
            {
                if (_gSystem != null)
                    throw new InvalidOperationException("You can not modify gSystem once it has been set.");

                _gSystem = value;
            }
        }

        /// <summary>
        /// Gets the gooeyWindow.
        /// </summary>
        /// <value>The gooeyWindow.</value>
        /// <remarks>This value should either throw an exception if changed after it was initially set
        /// or should handle the change explicitly (within the set function).</remarks>
        public virtual gooeyWindow gWindow
        {
            get
            {
                return _gWindow;
            }
            set
            {
                if (_gWindow != null)
                    throw new InvalidOperationException("You can not modify gSystem once it has been set.");

                _gWindow = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>If overridden, base.isGooeyInitialized must return <c>true</c> for the child version to return <c>true</c>.</remarks>
        public virtual bool isGooeyInitialized
        {
            get
            {
                return _isInitializationComplete && (_gSystem != null) && (_gWindow != null);
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="inlaySlider"/> class.
        /// </summary>
        public gooeySlider()
            : base()
        {
            _isInitializationComplete = false;
            _gWindow = null;
            _gSystem = null;
        }
        #endregion

        #region IGooeyControl Members
        /// <summary>
        /// Called when [initialize complete].
        /// </summary>
        /// <remarks>When overriding this function, you must call base.onGooeyInitializationComplete AFTER any new code.</remarks>
        public virtual void onGooeyInitializationComplete()
        {
            if (_isInitializationComplete)
                throw new InvalidOperationException("Can not execute onGooeyInitializationComplete() because the object has already been initialized.");

            _isInitializationComplete = true;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>base.Dispose must be called when overriding.</remarks>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
