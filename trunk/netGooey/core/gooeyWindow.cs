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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace netGooey.core
{
    /// <summary>
    /// Instance of a netGooey Window
    /// </summary>
    public class gooeyWindow : Window
    {
        #region Members
        /// <summary>
        /// Path to the XAML file containing the object UI information
        /// </summary>
        private string _sUIFile;

        /// <summary>
        /// netGooey System
        /// </summary>
        private gooeySystem _gSystem;
        #endregion

        #region Properties
        /// <summary>
        /// Path to the XAML file containing the object UI information
        /// </summary>
        public string sUIFile
        {
            get
            {
                return _sUIFile;
            }
            set
            {
                _sUIFile = value;
            }
        }

        /// <summary>
        /// gooeySystem that is the window base
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
        /// Base Constructor
        /// </summary>
        /// <param name="gSystem">gooeySystem core</param>
        public gooeyWindow(gooeySystem gSystem)
        {
            if (gSystem == null)
                throw new ArgumentNullException("gooeySystem can't be null.");

            _gSystem = gSystem;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Loads the UI elements from the XAML file
        /// </summary>
        /// <param name="dInit">Initialization delegate</param>
        /// <returns>True if loaded; otherwise false</returns>
        public bool loadUI(gooeySystem.initializationDelegate dInit)
        {
            if (_sUIFile == null)
                return false;

            Content = null;

            _gSystem.invokeOnLocalThread((Action)(() =>
                {
                    FileStream fileStream = File.OpenRead(_sUIFile);
                    DependencyObject dependencyObject = XamlReader.Load(fileStream) as DependencyObject;
                    fileStream.Close();

                    if (dependencyObject == null)
                        return;

                    Content = dependencyObject;
                }), true);

            if (Content == null)
                return false;

            // Init the window
            Grid gBase = Content as Grid;

            if (gBase == null)
            {
                Content = null;
                return false;
            }

            Width = gBase.Width;
            Height = gBase.Height;

            // Init all the objects
            initializeObjects(Content as DependencyObject, dInit); 

            return true;
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Initializes the objects.
        /// </summary>
        /// <param name="dRoot">The tree root.</param>
        /// <param name="dInit">Initialization delegate</param>
        private void initializeObjects(DependencyObject dRoot, gooeySystem.initializationDelegate dInit)
        {
            if (dRoot == null)
                return;

            initializeObject(dRoot as IGooeyControl, dInit);

            foreach (object dChild in LogicalTreeHelper.GetChildren(dRoot))
                initializeObjects(dChild as DependencyObject, dInit);
        }

        /// <summary>
        /// Attaches required hanlders to an object (if we need to)
        /// </summary>
        /// <param name="gElement">Element to attach to</param>
        /// <param name="dInit">Initialization delegate</param>
        private void initializeObject(IGooeyControl gElement, gooeySystem.initializationDelegate dInit)
        {
            if (gElement == null)
                return;

            gElement.gSystem = _gSystem;
            gElement.gWindow = this;
            dInit.BeginInvoke(gElement, new AsyncCallback(markElementInitialized), gElement);
        }

        /// <summary>
        /// Marks the element as initialized.
        /// </summary>
        /// <param name="aResult">Async result.</param>
        private void markElementInitialized(IAsyncResult aResult)
        {
            IGooeyControl gElement = aResult.AsyncState as IGooeyControl;

            if (gElement == null)
                throw new InvalidCastException("Element is not of expected type.");

            gElement.onGooeyInitializationComplete();
        }
        #endregion
    }
}
