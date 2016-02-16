﻿/* ------------------------------------------------------------------------- */
///
/// KeyValueCancelEventArgs.cs
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
using System;
using System.ComponentModel;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// KeyValueCancelEventArgs
    ///
    /// <summary>
    /// イベントハンドラに特定の型の Key-Value ペアを渡すためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class KeyValueCancelEventArgs<TKey, TValue> : CancelEventArgs
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// KeyValueCancelEventArgs
        /// 
        /// <summary>
        /// Cancel の値を false に設定してオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public KeyValueCancelEventArgs(TKey key, TValue value)
            : this(key, value, false) { }

        /* ----------------------------------------------------------------- */
        ///
        /// KeyValueCancelEventArgs
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public KeyValueCancelEventArgs(TKey key, TValue value, bool cancel) : base(cancel)
        {
            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Key
        /// 
        /// <summary>
        /// キーを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TKey Key { get; }

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