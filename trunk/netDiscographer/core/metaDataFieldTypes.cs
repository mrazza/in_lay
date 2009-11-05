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
 * Copyright (C) 2009 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;

namespace netDiscographer.core
{
    /// <summary>
    /// Valid Search Fields
    /// </summary>
    [Flags]
    public enum metaDataFieldTypes : ushort
    {
        /// <summary>
        /// Search all fields
        /// </summary>
        none = 0x0,

        /// <summary>
        /// Artist Search Field
        /// </summary>
        artist = 0x1,

        /// <summary>
        /// Title/Track Name Search Field
        /// </summary>
        title = 0x2,

        /// <summary>
        /// Album Name Search Field
        /// </summary>
        album = 0x4,

        /// <summary>
        /// Album Artist Search Field
        /// </summary>
        album_artist = 0x8,

        /// <summary>
        /// Composer Search Field
        /// </summary>
        composer = 0x10,

        /// <summary>
        /// Year Search Field
        /// </summary>
        year = 0x20,

        /// <summary>
        /// Comment Search Field
        /// </summary>
        comment = 0x40,

        /// <summary>
        /// Track Number Search Field
        /// </summary>
        track = 0x80,

        /// <summary>
        /// Total Tracks Search Field
        /// </summary>
        total_tracks = 0x100,

        /// <summary>
        /// Disk Number Search Field
        /// </summary>
        disk = 0x200,

        /// <summary>
        /// Total Disks Search Field
        /// </summary>
        total_disks = 0x400,

        /// <summary>
        /// Genre Search Field
        /// </summary>
        genre = 0x800,

        /// <summary>
        /// Raiting Search Field
        /// </summary>
        raiting = 0x1000,

        /// <summary>
        /// Playcount
        /// </summary>
        play_count = 0x2000,

        /// <summary>
        /// Time Last Played
        /// </summary>
        time_last_played = 0x4000,

        /// <summary>
        /// All of the fields
        /// </summary>
        all = 0x7FFF,

        /// <summary>
        /// Last Entry ID
        /// </summary>
        last = 0x4000
    }
}