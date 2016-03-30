﻿/* ------------------------------------------------------------------------- */
///
/// ValueCancelEventArgs.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.ComponentModel;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// ValueCancelEventArgs(TValue)
    ///
    /// <summary>
    /// イベントハンドラに特定の型の値を渡すためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ValueCancelEventArgs<TValue> : CancelEventArgs
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ValueCancelEventArgs
        /// 
        /// <summary>
        /// Cancel の値を false に設定してオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ValueCancelEventArgs(TValue value) : this(value, false) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ValueCancelEventArgs
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ValueCancelEventArgs(TValue value, bool cancel) : base(cancel)
        {
            Value = value;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        /// 
        /// <summary>
        /// 値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TValue Value { get; }

        #endregion
    }
}
