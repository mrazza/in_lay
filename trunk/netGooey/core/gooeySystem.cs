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
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace netGooey.core
{
    /// <summary>
    /// netGooey Core/System
    /// </summary>
    public sealed class gooeySystem
    {
        #region Members
        /// <summary>
        /// Path to the XAML file containing the UI styling information
        /// </summary>
        private string _sStyleFile;

        /// <summary>
        /// Old Skin Dictionary
        /// </summary>
        private ResourceDictionary _rOldSkin;

        /// <summary>
        /// Dispatcher for main thread
        /// </summary>
        private Dispatcher _dThreadDispatcher;

        /// <summary>
        /// Delegate used to execute inialization commands when the object is added to the window
        /// </summary>
        /// <param name="eElement">Element to init</param>
        public delegate void initializationDelegate(IGooeyControl eElement);
        #endregion

        #region Properties
        /// <summary>
        /// Path to the XAML file containing the UI styling information
        /// </summary>
        public string sStyleFile
        {
            get
            {
                return _sStyleFile;
            }
            set
            {
                _sStyleFile = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        public gooeySystem()
        {
            _rOldSkin = null;
            _dThreadDispatcher = Dispatcher.CurrentDispatcher;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Creates a window
        /// </summary>
        /// <returns>new gooeyWindow</returns>
        public gooeyWindow createWindow()
        {
            gooeyWindow gWindow = null;

            invokeOnLocalThread((Action)(() =>
                {
                    gWindow = new gooeyWindow(this);
                }), true);

            return gWindow;
        }

        /// <summary>
        /// Creates a window from a XAML file
        /// </summary>
        /// <param name="sFile">XAML File</param>
        /// <param name="dInit">Initialization Delegate</param>
        /// <returns>new gooeyWindow</returns>
        public gooeyWindow createWindow(string sFile, gooeySystem.initializationDelegate dInit)
        {
            gooeyWindow gWindow = createWindow();
            gWindow.sUIFile = sFile;
            gWindow.loadUI(dInit);
            return gWindow;
        }

        /// <summary>
        /// Loads the style configuration from the XAML file
        /// </summary>
        /// <returns>True if loaded; otherwise false</returns>
        public bool loadStyle()
        {
            if (_sStyleFile == null)
            {
                if (_rOldSkin != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(_rOldSkin);
                    _rOldSkin = null;
                    return true;
                }

                return false;
            }

            FileStream fileStream = File.OpenRead(_sStyleFile);
            ResourceDictionary resourceObject = XamlReader.Load(fileStream) as ResourceDictionary;
            fileStream.Close();

            if (resourceObject == null)
                return false;

            Application.Current.Resources.MergedDictionaries.Add(resourceObject);
            Application.Current.Resources.MergedDictionaries.Remove(_rOldSkin);
            _rOldSkin = resourceObject;
            return true;
        }

        /// <summary>
        /// Invokes a method on the thread the instance was created on
        /// </summary>
        /// <param name="dFunction">Function delegate to invoke</param>
        public void invokeOnLocalThread(Delegate dFunction)
        {
            invokeOnLocalThread(dFunction, false);
        }

        /// <summary>
        /// Invokes a method on the thread the instance was created on
        /// </summary>
        /// <param name="dFunction">Function delegate to invoke</param>
        /// <param name="bBlocking">If set to true, this thread will wait till the invoke finishes</param>
        public void invokeOnLocalThread(Delegate dFunction, bool bBlocking)
        {
            if (_dThreadDispatcher.Thread == Thread.CurrentThread)
            {
                ((Action)dFunction)();
                return;
            }

            if (!bBlocking)
            {
                _dThreadDispatcher.BeginInvoke(dFunction);
                return;
            }

            _dThreadDispatcher.Invoke(dFunction);
        }
        #endregion
    }
}
