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

using System;
using System.Collections.Generic;

namespace netDiscographer.core.dynamicQueryCore
{
    /// <summary>
    /// Single Search Entity
    /// x="y"
    /// </summary>
    [Serializable]
    public class searchEntity : ISearchBase
    {
        #region Members
        /// <summary>
        /// Field to seach
        /// </summary>
        protected metaDataFieldTypes _mFieldType;

        /// <summary>
        /// Comparison Operator
        /// </summary>
        protected comparisonOperators _cComparison;

        /// <summary>
        /// Search Term
        /// </summary>
        protected string _sSearchTerm;
        #endregion

        #region Properties
        /// <summary>
        /// Field type to search
        /// </summary>
        public metaDataFieldTypes mFieldType
        {
            get
            {
                return _mFieldType;
            }
            set
            {
                _mFieldType = value;
            }
        }

        /// <summary>
        /// Comparison Type
        /// </summary>
        public comparisonOperators cComparison
        {
            get
            {
                return _cComparison;
            }
            set
            {
                _cComparison = value;
            }
        }

        /// <summary>
        /// Search Term
        /// </summary>
        public string sSearchTerm
        {
            get
            {
                return _sSearchTerm;
            }
            set
            {
                _sSearchTerm = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public searchEntity()
        {
            _mFieldType = metaDataFieldTypes.none;
            _cComparison = comparisonOperators.none;
            _sSearchTerm = null;
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="mField">Search Field</param>
        /// <param name="cType">Comparison Operator</param>
        /// <param name="sData">Data to Search</param>
        public searchEntity(metaDataFieldTypes mField, comparisonOperators cType, string sData)
        {
            _mFieldType = mField;
            _cComparison = cType;
            _sSearchTerm = sData;
        }
        #endregion

        #region ISearchBase Members
        /// <summary>
        /// Returns the next set of query objects
        /// </summary>
        /// <returns>Next set of queries</returns>
        public ISearchBase[] getNextQueries()
        {
            return null;
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// Checks if the data has been set and is acceptable
        /// </summary>
        /// <returns>True if it is set; otherwise false</returns>
        protected bool isDataSet()
        {
            if (_cComparison == comparisonOperators.none || _sSearchTerm == null)
                return false;

            return true;
        }
        #endregion
    }
}
