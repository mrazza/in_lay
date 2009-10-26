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

namespace netDiscographer.core.dynamicQueryCore
{
    /// <summary>
    /// Search Group; a group of search objects
    /// </summary>
    [Serializable]
    public class searchGroup : ISearchBase
    {
        #region Members
        /// <summary>
        /// First Object in the Group
        /// </summary>
        protected ISearchBase _sFirstObject;

        /// <summary>
        /// Logical Operator
        /// </summary>
        protected logicOperators _lLogicOperator;

        /// <summary>
        /// Second Object in the Group
        /// </summary>
        protected ISearchBase _sSecondObject;
        #endregion

        #region Properties
        /// <summary>
        /// First Object
        /// </summary>
        public ISearchBase sFirstObject
        {
            get
            {
                return _sFirstObject;
            }
            set
            {
                _sFirstObject = value;
            }
        }

        /// <summary>
        /// Logical Operator
        /// </summary>
        public logicOperators lLogicOperator
        {
            get
            {
                return _lLogicOperator;
            }
            set
            {
                _lLogicOperator = value;
            }
        }

        /// <summary>
        /// Second Object
        /// </summary>
        public ISearchBase sSecondObject
        {
            get
            {
                return _sSecondObject;
            }
            set
            {
                _sSecondObject = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public searchGroup()
        {
            _sFirstObject = null;
            _lLogicOperator = logicOperators.none;
            _sSecondObject = null;
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="sFirst">First Object</param>
        /// <param name="lLogic">Logical Operator</param>
        /// <param name="sSecond">Second Object</param>
        public searchGroup(ISearchBase sFirst, logicOperators lLogic, ISearchBase sSecond)
        {
            _sFirstObject = sFirst;
            _lLogicOperator = lLogic;
            _sSecondObject = sSecond;
        }
        #endregion

        #region ISearchBase Members
        /// <summary>
        /// Returns the next set of query objects
        /// </summary>
        /// <returns>Next set of queries</returns>
        public ISearchBase[] getNextQueries()
        {
            return new ISearchBase[] { _sFirstObject, _sSecondObject };
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// Checks if the data has been set and is acceptable
        /// </summary>
        /// <returns>True if it is set; otherwise false</returns>
        protected bool isDataSet()
        {
            if (_lLogicOperator == logicOperators.none || _sFirstObject == null || (_sSecondObject == null && _lLogicOperator != logicOperators.parenthesis))
                return false;

            return true;
        }
        #endregion
    }
}
