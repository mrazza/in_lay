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
using System.Collections.Generic;

namespace netDiscographer.core
{
    /// <summary>
    /// Comparator for mediaEntry objects
    /// </summary>
    internal class mediaEntryComparer : IComparer<mediaEntry>
    {
        #region Members
        /// <summary>
        /// Field types to sort by
        /// </summary>
        private metaDataFieldTypes[] _mTypes;

        /// <summary>
        /// Sort order for each field type
        /// </summary>
        private sortOrder[] _sSortOrder;

        /// <summary>
        /// List of string-typed metaDataFieldTypes
        /// </summary>
        private static readonly metaDataFieldTypes[] _mStringTypes = { metaDataFieldTypes.album, metaDataFieldTypes.album_artist,
                                                                         metaDataFieldTypes.artist, metaDataFieldTypes.comment,
                                                                         metaDataFieldTypes.composer, metaDataFieldTypes.genre,
                                                                         metaDataFieldTypes.title };
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="mTypes">Media data types to search by (most significant first)</param>
        /// <param name="sSortOrder">Sort order for corrisponding field types</param>
        public mediaEntryComparer(metaDataFieldTypes[] mTypes, sortOrder[] sSortOrder)
        {
            if (mTypes.Length != sSortOrder.Length)
                throw new ArgumentException("mTypes and sSortOrder arrays are of differing lengths.");

            _mTypes = mTypes;
            _sSortOrder = sSortOrder;
        }
        #endregion

        #region IComparer<mediaEntry> Members
        /// <summary>
        /// Compares to mediaEntries
        /// </summary>
        /// <param name="x">First entry</param>
        /// <param name="y">Second entry</param>
        /// <returns>Less than 0 means x less than y; Equal to 0 means x equals y; Greater than 0 means x greater than y</returns>
        public int Compare(mediaEntry x, mediaEntry y)
        {
            string sCurrX, sCurrY;

            for (int iLoop = 0; iLoop < _mTypes.Length; iLoop++)
            {
                sCurrX = x[_mTypes[iLoop]];
                sCurrY = y[_mTypes[iLoop]];

                if (sCurrX == null)
                    sCurrX = "";

                if (sCurrY == null)
                    sCurrY = "";

                if (sCurrX.Equals(sCurrY))
                    continue;

                if (isStringType(_mTypes[iLoop]))
                {
                    if (_sSortOrder[iLoop] == sortOrder.descending)
                        return sCurrY.ToLower().CompareTo(sCurrX.ToLower());
                    else
                        return sCurrX.ToLower().CompareTo(sCurrY.ToLower());
                }
                else
                {
                    if (_sSortOrder[iLoop] == sortOrder.descending)
                        return uint.Parse(sCurrY).CompareTo(uint.Parse(sCurrX));
                    else
                        return uint.Parse(sCurrX).CompareTo(uint.Parse(sCurrY));
                }
            }

            return 0; //Based on what we searched, they are the same
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Is this meta data type a string type
        /// </summary>
        /// <param name="mType">metaDataType to check</param>
        /// <returns>True if string type, otherwise false</returns>
        private bool isStringType(metaDataFieldTypes mType)
        {
            foreach (metaDataFieldTypes mCurr in _mStringTypes)
            {
                if (mType == mCurr)
                    return true;
            }

            return false;
        }
        #endregion
    }
}