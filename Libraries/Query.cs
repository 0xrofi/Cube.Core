﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Threading;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// IQuery(TQuery, TResult)
    /// 
    /// <summary>
    /// 問い合わせ用プロバイダーを定義します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IQuery<TQuery, TResult>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Request
        /// 
        /// <summary>
        /// 問い合わせを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Request(QueryEventArgs<TQuery, TResult> value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Query(TQuery, TResult)
    /// 
    /// <summary>
    /// IQuery(TQuery, TResult) を実装したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Query<TQuery, TResult> : IQuery<TQuery, TResult>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Query()
        {
            _context = SynchronizationContext.Current;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="callback">コールバック関数</param>
        ///
        /* ----------------------------------------------------------------- */
        public Query(Action<QueryEventArgs<TQuery, TResult>> callback) : this()
        {
            Requested += (s, e) => callback(e);
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Requested
        /// 
        /// <summary>
        /// 問い合わせ時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event QueryEventHandler<TQuery, TResult> Requested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnRequested
        /// 
        /// <summary>
        /// Requested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void OnRequested(QueryEventArgs<TQuery, TResult> e)
        {
            if (Requested != null)
            {
                if (_context != null) _context.Send(_ => Requested(this, e), null);
                else Requested(this, e);
            }
            else e.Cancel = true;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        /// 
        /// <summary>
        /// 問い合わせを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// 問い合わせの結果が無効な場合、Cancel プロパティが true に
        /// 設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Request(QueryEventArgs<TQuery, TResult> value)
            => OnRequested(value);

        #endregion

        #region Fields
        private readonly SynchronizationContext _context;
        #endregion
    }
}
