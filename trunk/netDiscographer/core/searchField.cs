/*******************************************************************
 * This file is part of the netDiscographer library.
 * 
 * netDiscographer source may not be distributed or modified without
 * explicit written permission.
 * 
 * netDiscographer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009 Matt Razza
 *******************************************************************/

using System;

namespace netDiscographer.core
{
    /// <summary>
    /// Valid Search Fields
    /// </summary>
    [Flags]
    public enum searchField : ushort
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
        comment = 0x30,

        /// <summary>
        /// Track Number Search Field
        /// </summary>
        track = 0x40,

        /// <summary>
        /// Total Tracks Search Field
        /// </summary>
        total_tracks = 0x50,

        /// <summary>
        /// Disk Number Search Field
        /// </summary>
        disk = 0x60,

        /// <summary>
        /// Total Disks Search Field
        /// </summary>
        total_disks = 0x70,

        /// <summary>
        /// Genre Search Field
        /// </summary>
        genre = 0x80,

        /// <summary>
        /// Raiting Search Field
        /// </summary>
        raiting = 0x90,

        /// <summary>
        /// Length Search Field
        /// </summary>
        length = 0x100,

        /// <summary>
        /// Playcount
        /// </summary>
        play_count = 0x200
    }
}