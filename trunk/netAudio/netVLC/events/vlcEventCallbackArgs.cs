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
using System.Runtime.InteropServices;
using netAudio.netVLC.core;

namespace netAudio.netVLC.events
{
    #region vlcCallbackArgs
    /// <summary>
    /// vlcCallbackArgs Struct
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct vlcCallbackArgs
    {
        /// <summary>
        /// Event type triggered
        /// </summary>
        [FieldOffset(0)]
        public vlcEventType vEventType;

        /// <summary>
        /// Media/Player that triggered the event
        /// </summary>
        [FieldOffset(8)]
        public IntPtr vSendingMedia;

        /// <summary>
        /// Meta Changed Argument Struct (if meta type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaMetaChangedArgs vMediaMetaChanged;

        /// <summary>
        /// Sub Item Added Argument Struct (if sub item added type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaSubItemAddedArgs vMediaSubItemAdded;

        /// <summary>
        /// Length Changed Argument Struct (if length changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaLengthChangedArgs vMediaLengthChanged;

        /// <summary>
        /// Preparsed Status Changed Argument Struct (if preparsed changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaPreparsedChangedArgs vMediaPreparsedChanged;

        /// <summary>
        /// Freed Status Changed Argument Struct (if freed changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaFreedArgs vMediaFreed;

        /// <summary>
        /// State Changed Argument Struct (if state changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcMediaStateChangedArgs vMediaStateChanged;

        /// <summary>
        /// Player Position Changed Argument Struct (if player position changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcPlayerPositionChangedArgs vPlayerPositionChanged;

        /// <summary>
        /// Player Time Changed Argument Struct (if player time changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcPlayerTimeChangedArgs vPlayerTimeChanged;

        /// <summary>
        /// Player Seekable Changed Argument Struct (if player seekable changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcPlayerSeekableChangedArgs vPlayerSeekableChanged;

        /// <summary>
        /// Player Pausable Status Changed Argument Struct (if player pausable changed type)
        /// </summary>
        [FieldOffset(16)]
        public vlcPlayerPausableChangedArgs vPlayerPausableChanged;

        /// <summary>
        /// List Item Added Argument Struct (if list item added type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListItemAddedArgs vListItemAdded;

        /// <summary>
        /// List Will Add Item Argument Struct (if list will add item type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListWillAddItemArgs vListWillAddItem;

        /// <summary>
        /// List Item Deleted Argument Struct (if list item deleted type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListItemDeletedArgs vListItemDeleted;

        /// <summary>
        /// List Will Delete Item Argument Struct (if list will delete item type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListWillDeleteItemArgs vListWillDeleteItem;

        /// <summary>
        /// List View Item Added Argument Struct (if list view item added type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListViewItemAddedArgs vListViewItemAdded;

        /// <summary>
        /// List View Will Add Item Argument Struct (if list view will add item type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListViewWillAddItemArgs vListViewWill;

        /// <summary>
        /// List View Item Delete Argument Struct (if list view item deleted type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListViewItemDeletedArgs vListViewItemDeleted;

        /// <summary>
        /// List View Will Delete Item Argument Struct (if list view will delete item type)
        /// </summary>
        [FieldOffset(16)]
        public vlcListViewWillDeleteItemArgs vListViewWillDeleteItem;
    }
    #endregion

    #region vlcMedia
    /// <summary>
    /// Data struct for vlc meta changed
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaMetaChangedArgs
    {
        /// <summary>
        /// Meta type that changed
        /// </summary>
        public vlcMedia.vlcMetaTypes vMetaType;
    }

    /// <summary>
    /// Data struct for sub item added
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaSubItemAddedArgs
    {
        /// <summary>
        /// Added handle
        /// </summary>
        public IntPtr vMediaHandle;
    }

    /// <summary>
    /// Length changed struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaLengthChangedArgs
    {
        /// <summary>
        /// New Length (milliseconds)
        /// </summary>
        public long lLength;
    }

    /// <summary>
    /// Media Preparsed changed struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaPreparsedChangedArgs
    {
        /// <summary>
        /// New status (1 == true; otherwise 0)
        /// </summary>
        public int iStatus;
    }

    /// <summary>
    /// Freed struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaFreedArgs
    {
        /// <summary>
        /// Freed Media Handle
        /// </summary>
        public IntPtr vMediaHandle;
    }

    /// <summary>
    /// Media State Changed
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcMediaStateChangedArgs
    {
        /// <summary>
        /// New State
        /// </summary>
        public vlcPlayer.vlcPlayerState vState;
    }
    #endregion

    #region vlcPlayer
    /// <summary>
    /// Player Position Changed Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcPlayerPositionChangedArgs
    {
        /// <summary>
        /// New Position of the player (%)
        /// </summary>
        public float fPosition;
    }

    /// <summary>
    /// Player Time Changed Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcPlayerTimeChangedArgs
    {
        /// <summary>
        /// New Time Value (microseconds)
        /// </summary>
        public long lTime;
    }

    /// <summary>
    /// Player Seekable Status Changed
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcPlayerSeekableChangedArgs
    {
        /// <summary>
        /// New Seekable Status (1 = true; 0 = false)
        /// </summary>
        public long lSeekable;
    }

    /// <summary>
    /// New Pausable Status
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcPlayerPausableChangedArgs
    {
        /// <summary>
        /// New Pausable Status (1 = true; 0 = false)
        /// </summary>
        public long lPausable;
    }
    #endregion

    #region vlcList
    /// <summary>
    /// List Item Added Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListItemAddedArgs
    {
        /// <summary>
        /// Media Added
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// Will Add Item to List Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListWillAddItemArgs
    {
        /// <summary>
        /// Media to Add
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// Deleted Item from List Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListItemDeletedArgs
    {
        /// <summary>
        /// Media Deleted
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// Will Delete Item from List Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListWillDeleteItemArgs
    {
        /// <summary>
        /// Media to Delete
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }
    #endregion

    #region vlcListView
    /// <summary>
    /// List View Item Added Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListViewItemAddedArgs
    {
        /// <summary>
        /// Media Item
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// List View Will Add Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListViewWillAddItemArgs
    {
        /// <summary>
        /// Media Item
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// List View Item Deleted Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListViewItemDeletedArgs
    {
        /// <summary>
        /// Media Item
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }

    /// <summary>
    /// List View Will Delete Item Argument Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct vlcListViewWillDeleteItemArgs
    {
        /// <summary>
        /// Media Item
        /// </summary>
        public IntPtr vMediaHandle;

        /// <summary>
        /// List Index
        /// </summary>
        public int iIndex;
    }
    #endregion
}
