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
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System.Windows.Controls;
using System.Windows.Data;
using inlayShared.ui.controls.core;
using netDiscographer.core;

namespace inlayShared.ui.controls.library.core
{
    /// <summary>
    /// GridView that has full support for displaying mediaEntry objects
    /// </summary>
    public abstract class mediaGridView : inlayGridView
    {
        #region Members
        /// <summary>
        /// Fields to display
        /// </summary>
        private metaDataFieldTypes[] _mFields;
        #endregion

        #region Properties
        /// <summary>
        /// Fields to display
        /// </summary>
        public metaDataFieldTypes[] mFields
        {
            get
            {
                return _mFields;
            }
            set
            {
                _mFields = value;

                updateFields();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="mediaGridView"/> class.
        /// </summary>
        public mediaGridView()
            : base() { }
        #endregion

        #region Protected Members
        /// <summary>
        /// Updates the fields.
        /// </summary>
        protected void updateFields()
        {
            // Clear out the collection
            Columns.Clear();
            GridViewColumn gNewColumn;

            foreach (metaDataFieldTypes mCurr in _mFields)
            {
                gNewColumn = new GridViewColumn();
                gNewColumn.Header = mediaEntry.getFriendlyFieldName(mCurr);
                gNewColumn.DisplayMemberBinding = new Binding(string.Format("[{0}]", (int)mCurr));
                gNewColumn.Width = 100;
                Columns.Add(gNewColumn);
            }
        }
        #endregion
    }
}
