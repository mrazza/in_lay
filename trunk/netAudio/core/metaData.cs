﻿/*******************************************************************
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

namespace netAudio.core
{
    /// <summary>
    /// Class to store media meta data
    /// </summary>
    public class metaData
    {
        /// <summary>
        /// Title of the track
        /// </summary>
        public string sTitle;

        /// <summary>
        /// Artist of the track
        /// </summary>
        public string sArtist;

        /// <summary>
        /// Album
        /// </summary>
        public string sAlbum;
        
        /// <summary>
        /// Album Artist
        /// </summary>
        public string sAlbumArtist;

        /// <summary>
        /// Track Composer
        /// </summary>
        public string sComposer;

        /// <summary>
        /// Album/Track Year
        /// </summary>
        public uint iYear;

        /// <summary>
        /// Track Comment
        /// </summary>
        public string sComment;

        /// <summary>
        /// Track Number
        /// </summary>
        public uint iTrack;

        /// <summary>
        /// Total Tracks
        /// </summary>
        public uint iTotalTracks;

        /// <summary>
        /// Disk of the Album
        /// </summary>
        public uint iDisk;

        /// <summary>
        /// Total Disks
        /// </summary>
        public uint iTotalDisks;

        /// <summary>
        /// Genre
        /// </summary>
        public string sGenre;

        /// <summary>
        /// URL to album art
        /// </summary>
        public string sArtURL;

        /// <summary>
        /// Track raiting
        /// </summary>
        public uint iRating;

        /// <summary>
        /// Length of the track
        /// </summary>
        public TimeSpan tLength;

        /// <summary>
        /// Does this class contain data?
        /// </summary>
        public bool bContainsData;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public metaData()
        {
            bContainsData = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="artist">Artist</param>
        /// <param name="album">Album</param>
        /// <param name="albumArtist">Album Artist</param>
        /// <param name="composer">Composer</param>
        /// <param name="year">Year</param>
        /// <param name="comment">Comment</param>
        /// <param name="track">Track #</param>
        /// <param name="totalTracks">Total Tracks</param>
        /// <param name="disk">Disk #</param>
        /// <param name="totalDisk">Total Disks</param>
        /// <param name="genre">Genre</param>
        /// <param name="artURL">Art URL</param>
        /// <param name="rating">Track Rating</param>
        /// <param name="len">Length</param>
        public metaData(string title, string artist, string album, string albumArtist, string composer, uint year, string comment, uint track, uint totalTracks, uint disk, uint totalDisk, string genre, string artURL, uint rating, TimeSpan len)
        {
            sTitle = title;
            sArtist = artist;
            sAlbum = album;
            sAlbumArtist = albumArtist;
            sComposer = composer;
            iYear = year;
            sComment = comment;
            iTrack = track;
            iTotalTracks = totalTracks;
            iDisk = disk;
            iTotalDisks = totalDisk;
            sGenre = genre;
            sArtURL = artURL;
            iRating = rating;
            tLength = len;
            bContainsData = true;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return sTitle + " - " + sArtist;
        }

        /// <summary>
        /// Returns a <see cref="System.Object"/> array that represents this instance.
        /// </summary>
        /// <remarks>
        /// Order in which the data is returned:
        /// sTitle, 
        /// sArtist, 
        /// sAlbum,
        /// sAlbumArtist,
        /// sComposer,
        /// iYear,
        /// sComment,
        /// iTrack,
        /// iTotalTracks,
        /// iDisk,
        /// iTotalDisks,
        /// sGenre,
        /// sArtURL,
        /// iRating,
        /// tLength
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Object"/> array that represents this instance.
        /// </returns>
        public object[] ToArray()
        {
            return new object[] {
                sTitle, 
                sArtist, 
                sAlbum,
                sAlbumArtist,
                sComposer,
                iYear,
                sComment,
                iTrack,
                iTotalTracks,
                iDisk,
                iTotalDisks,
                sGenre,
                sArtURL,
                iRating,
                tLength
            };
        }
    }
}
