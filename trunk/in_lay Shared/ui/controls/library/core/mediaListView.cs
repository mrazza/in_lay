/*******************************************************************
 * This file is part of the in_lay Player Shared Library.
 * 
 * in_lay source may be distributed or modified without
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

using System;
using inlayShared.ui.controls.core;
using netDiscographer.core;

namespace inlayShared.ui.controls.library.core
{
    /// <summary>
    /// ListView that has full support for displaying mediaEntry objects
    /// </summary>
    public abstract class mediaListView : inlayListView
    {
        #region Members
        /// <summary>
        /// Sort Order
        /// </summary>
        private metaDataFieldTypes[] _mSortOrder;

        /// <summary>
        /// Data to display
        /// </summary>
        private mediaEntry[] _mData;
        #endregion

        #region Properties
        /// <summary>
        /// Sort Order
        /// </summary>
        public metaDataFieldTypes[] mSortOrder
        {
            get
            {
                return _mSortOrder;
            }
            set
            {
                _mSortOrder = value;

                sortData();
            }
        }

        /// <summary>
        /// Data to display
        /// </summary>
        public mediaEntry[] mData
        {
            get
            {
                return _mData;
            }
            set
            {
                _mData = value;
                sortData();
                _gSystem.invokeOnLocalThread((Action)(() =>
                {
                        ItemsSource = _mData;
                }));
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="mediaListView"/> class.
        /// </summary>
        public mediaListView()
            : base() { }
        #endregion

        #region Protected Members
        /// <summary>
        /// Sorts the data.
        /// </summary>
        protected void sortData()
        {
            if (_mData == null)
                return;

            mediaEntry.sortMedia(_mData, _mSortOrder, sortOrder.ascending);
        }
        #endregion
    }
}
