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

namespace netAudio.core
{
    /// <summary>
    /// Media Player States
    /// </summary>
    public enum playerState : int
    {
        /// <summary>
        /// Waiting to receive input
        /// </summary>
        waiting,

        /// <summary>
        /// Ready to play
        /// </summary>
        ready,

        /// <summary>
        /// Opening media
        /// </summary>
        opening,

        /// <summary>
        /// Buffering media
        /// </summary>
        buffering,

        /// <summary>
        /// Playing media
        /// </summary>
        playing,

        /// <summary>
        /// Media paused
        /// </summary>
        paused,

        /// <summary>
        /// Media stopped
        /// </summary>
        stopped,

        /// <summary>
        /// Media reached end
        /// </summary>
        ended,

        /// <summary>
        /// There was a fatal error
        /// </summary>
        error
    }
}
