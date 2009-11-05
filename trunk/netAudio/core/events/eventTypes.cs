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

namespace netAudio.core.events
{
    /// <summary>
    /// netAudio Event Types
    /// </summary>
    enum eventTypes
    {
        /// <summary>
        /// Position of the current track changed
        /// </summary>
        positionChanged,

        /// <summary>
        /// Speed of the playback changed
        /// </summary>
        speedChanged,

        /// <summary>
        /// Player state changed
        /// </summary>
        stateChanged,

        /// <summary>
        /// Player volume changed
        /// </summary>
        volumeChanged
    }
}