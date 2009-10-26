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
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

namespace netAudio.netVLC.events
{
    #region vlcEventType
    /// <summary>
    /// VLC Event Types
    /// </summary>
    public enum vlcEventType : int
    {
        /// <summary>
        /// Meta Data Changed
        /// </summary>
        mediaMetaChanged,
        
        /// <summary>
        /// Sub Item Added
        /// </summary>
        mediaSubItemAdded,

        /// <summary>
        /// Media Length Changed
        /// </summary>
        mediaDurationChanged,

        /// <summary>
        /// Media Preparsed Status Changed
        /// </summary>
        mediaPreparsedChanged,

        /// <summary>
        /// Media Freed
        /// </summary>
        mediaFreed,

        /// <summary>
        /// Media State Change
        /// </summary>
        mediaStateChanged,

        /// <summary>
        /// Nothing Special Status
        /// </summary>
        playerNothingSpecial,

        /// <summary>
        /// Player Opening Status
        /// </summary>
        playerOpening,

        /// <summary>
        /// Player Buffering Status
        /// </summary>
        playerBuffering,

        /// <summary>
        /// Player Playing Status
        /// </summary>
        playerPlaying,

        /// <summary>
        /// Player Paused Status
        /// </summary>
        playerPaused,

        /// <summary>
        /// Player Stopped Status
        /// </summary>
        playerStopped,

        /// <summary>
        /// Player Forward Status
        /// </summary>
        playerForward,

        /// <summary>
        /// Player Backward Status
        /// </summary>
        playerBackward,

        /// <summary>
        /// End of Track Reached
        /// </summary>
        playerEndReached,

        /// <summary>
        /// Error Encountered
        /// </summary>
        playerEncounteredError,

        /// <summary>
        /// Time Changed
        /// </summary>
        playerTimeChanged,

        /// <summary>
        /// Position Changed
        /// </summary>
        playerPositionChanged,

        /// <summary>
        /// Seekable Status Changed
        /// </summary>
        playerSeekableChanged,

        /// <summary>
        /// Pausable Status Changed
        /// </summary>
        playerPausableChanged,

        /// <summary>
        /// Item Added
        /// </summary>
        listItemAdded,

        /// <summary>
        /// Will Add Item
        /// </summary>
        listWillAddItem,

        /// <summary>
        /// Item Deleted
        /// </summary>
        listItemDeleted,

        /// <summary>
        /// Will Delete Item
        /// </summary>
        listWillDeleteItem,

        /// <summary>
        /// View Item Added
        /// </summary>
        listViewItemAdded,

        /// <summary>
        /// Will Delete View Item
        /// </summary>
        listViewWillAddItem,

        /// <summary>
        /// View Item Deleted
        /// </summary>
        listViewItemDeleted,

        /// <summary>
        /// Will Delete View Item
        /// </summary>
        listViewWillDeleteItem,

        /// <summary>
        /// Player Played
        /// </summary>
        listPlayerPlayed,

        /// <summary>
        /// Player Next Item Set
        /// </summary>
        listPlayerNextItemSet,

        /// <summary>
        /// Player Stopped
        /// </summary>
        listPlayerStopped,

        /// <summary>
        /// Discoverer Started
        /// </summary>
        discovererStarted,

        /// <summary>
        /// Discoverer Ended
        /// </summary>
        discovererEnded
    }
    #endregion
}
