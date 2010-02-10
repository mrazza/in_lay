/*******************************************************************
 * This file is part of the netDiscographer library.
 * 
 * netDiscographer source may be distributed or modified without
 * permission if attribution is given and this message and copyright
 * remain.
 * 
 * netDiscographer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

namespace netDiscographer.core.mediaSearch
{
    /// <summary>
    /// Stages the media search can be in
    /// </summary>
    public enum mediaSearchStage : ushort
    {
        /// <summary>
        /// No stage set
        /// </summary>
        none = 0,

        /// <summary>
        /// Starting search
        /// </summary>
        starting = 1,

        /// <summary>
        /// Gathering files and folders to grab data from
        /// </summary>
        gatheringFiles = 2,

        /// <summary>
        /// Grabbing data
        /// </summary>
        gatheringMetaData = 3,

        /// <summary>
        /// Finished with the search
        /// </summary>
        finished = 4
    }
}
