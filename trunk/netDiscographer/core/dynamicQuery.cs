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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using netDiscographer.core.dynamicQueryCore;

namespace netDiscographer.core
{
    /// <summary>
    /// This class represents a dynamic playlist query and is used to build and manage a query.
    /// </summary>
    [Serializable]
    public class dynamicQuery : ISearchBase
    {
        #region Members
        /// <summary>
        /// Search query we are building
        /// </summary>
        private ISearchBase _sSearchQuery;

        /// <summary>
        /// Max number of returned results
        /// </summary>
        private int _iMaxCount;
        #endregion

        #region Properties
        /// <summary>
        /// Search Query
        /// </summary>
        public ISearchBase sSearchQuery
        {
            get
            {
                return _sSearchQuery;
            }
            set
            {
                _sSearchQuery = value;
            }
        }

        /// <summary>
        /// Max number of returned results
        /// </summary>
        public int iMaxCount
        {
            get
            {
                return _iMaxCount;
            }
            set
            {
                _iMaxCount = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        public dynamicQuery()
        {
            iMaxCount = -1;
            _sSearchQuery = null;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Merges this query with another
        /// </summary>
        /// <param name="dQuery">Query to merge</param>
        /// <param name="lLogicalOperator">Logical Binding Operator</param>
        /// <returns>True if worked; otherwise false</returns>
        public bool mergeQueries(dynamicQuery dQuery, logicOperators lLogicalOperator)
        {
            // Check for valid arguments
            if (lLogicalOperator == logicOperators.none || lLogicalOperator == logicOperators.parenthesis || dQuery == null)
                return false;

            _sSearchQuery = new searchGroup(_sSearchQuery, lLogicalOperator, dQuery._sSearchQuery);

            return true;
        }

        /// <summary>
        /// Adds a condition to the query
        /// </summary>
        /// <param name="mDataType">Meta data type(s) we're checking</param>
        /// <param name="cCompareOperator">Compare operator</param>
        /// <param name="oValue">Valuing we are checking for</param>
        /// <param name="lBindingOperator">Logical operator to bind with existing query</param>
        /// <returns>True if worked; otherwise false</returns>
        public bool addCondition(metaDataFieldTypes mDataType, comparisonOperators cCompareOperator, string oValue, logicOperators lBindingOperator)
        {
            // Check for valid arguments
            if (cCompareOperator == comparisonOperators.none)
                return false;

            oValue = discographerDatabase.makeSearchable(oValue);

            // Can we take the easy way out?
            if (_sSearchQuery == null)
            {
                _sSearchQuery = new searchGroup(new searchEntity(mDataType, cCompareOperator, oValue), logicOperators.parenthesis, null);
                return true;
            }

            if (lBindingOperator == logicOperators.none)
                return false;

            _sSearchQuery = new searchGroup(_sSearchQuery, lBindingOperator, new searchGroup(new searchEntity(mDataType, cCompareOperator, oValue), logicOperators.parenthesis, null));
            return true;
        }

        /// <summary>
        /// Groups the query
        /// </summary>
        /// <returns>True if worked; otherwise false</returns>
        public bool groupQuery()
        {
            if (_sSearchQuery == null)
                return false;

            _sSearchQuery = new searchGroup(_sSearchQuery, logicOperators.parenthesis, null);
            return true;
        }

        /// <summary>
        /// Checks if there is a max number of records
        /// </summary>
        /// <returns>True if there is a record count cap; otherwise false</returns>
        public bool hasMaxCount()
        {
            return _iMaxCount != -1;
        }

        /// <summary>
        /// Serializes this object
        /// </summary>
        /// <returns>Binary serialization</returns>
        public byte[] serialize()
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(mStream, this);

            return mStream.ToArray();
        }

        /// <summary>
        /// Deserializes the object
        /// </summary>
        /// <param name="bData">Binary serialization</param>
        /// <returns>The object</returns>
        public static dynamicQuery deserialize(byte[] bData)
        {
            MemoryStream mStream = new MemoryStream(bData);
            BinaryFormatter bFormatter = new BinaryFormatter();

            return (dynamicQuery)bFormatter.Deserialize(mStream);
        }

        #region ISearchBase Members
        /// <summary>
        /// Returns the next set of query objects
        /// </summary>
        /// <returns>Next set of queries</returns>
        public ISearchBase[] getNextQueries()
        {
            return new ISearchBase[] { _sSearchQuery };
        }
        #endregion
        #endregion
    }
}