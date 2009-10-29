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

using System.Windows;
using in_lay.core;
using netAudio.netVLC;
using netGooey.core;

namespace in_lay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructor/Entry Point
        /// <summary>
        /// Base Constructor (Entry Point for the Application)
        /// </summary>
        public App()
        {
            gooeySystem gSystem = new gooeySystem();
            inlayWindowManager inSys = new inlayWindowManager(new netVLCPlayer());
            inSys.generateWindow(gSystem, @"ui\default\main.xaml");
            inSys.gWindow.Show();
            inSys.nPlayer.sMediaPath = @""; //Enter path to track here C:\someFile.mp3
            inSys.nPlayer.iVolume = 10;
        }
        #endregion
    }
}
