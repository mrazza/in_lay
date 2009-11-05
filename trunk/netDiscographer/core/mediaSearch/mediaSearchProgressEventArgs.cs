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

namespace netDiscographer.core.mediaSearch
{
    /// <summary>
    /// Event Arguments for mediaSearchSystem
    /// </summary>
    public class mediaSearchProgressEventArgs : EventArgs
    {
        #region Members
        /// <summary>
        /// Current stage we are in
        /// </summary>
        private mediaSearchStage _mCurrentStage;

        /// <summary>
        /// Current progress of the stage
        /// </summary>
        private int _iCurrentProgress;

        /// <summary>
        /// Total progress of the stage
        /// </summary>
        private int _iTotalProgress;
        #endregion

        #region Properties
        /// <summary>
        /// Current stage we are in
        /// </summary>
        public mediaSearchStage mCurrentStage
        {
            get
            {
                return _mCurrentStage;
            }
        }

        /// <summary>
        /// Current progress of the current stage
        /// </summary>
        public int iCurrentProgress
        {
            get
            {
                return _iCurrentProgress;
            }
        }

        /// <summary>
        /// Total progress of the current stage
        /// </summary>
        public int iTotalProgress
        {
            get
            {
                return _iTotalProgress;
            }
        }

        /// <summary>
        /// Percent complete on current stage
        /// </summary>
        public double dPercentComplete
        {
            get
            {
                return ((double)_iCurrentProgress) / _iTotalProgress;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public mediaSearchProgressEventArgs()
            : this(mediaSearchStage.none, 0, 0) { }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="mStage">Stage we are in</param>
        /// <param name="lCurrProg">Current Progress (out of total)</param>
        /// <param name="lTotalProg">Total Progress</param>
        public mediaSearchProgressEventArgs(mediaSearchStage mStage, int lCurrProg, int lTotalProg)
        {
            _mCurrentStage = mStage;
            _iCurrentProgress = lCurrProg;
            _iTotalProgress = lTotalProg;
        }
        #endregion
    }
}
