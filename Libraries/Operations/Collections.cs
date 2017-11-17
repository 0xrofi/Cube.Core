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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cube.Collections
{
    /* --------------------------------------------------------------------- */
    ///
    /// Collections.Operations
    /// 
    /// <summary>
    /// Collection クラスの拡張メソッド用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Operations
    {
        #region IEnumerable(T)

        /* ----------------------------------------------------------------- */
        ///
        /// ToObservable
        /// 
        /// <summary>
        /// ObservableCollection に変換します。
        /// </summary>
        /// 
        /// <param name="src">変換前のコレクション</param>
        /// 
        /// <returns>ObservableCollection オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> src)
            => new ObservableCollection<T>(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Diff
        /// 
        /// <summary>
        /// 差分を検出します。
        /// </summary>
        /// 
        /// <param name="newer">変更後のオブジェクト</param>
        /// <param name="older">変更前のオブジェクト</param>
        /// <param name="diffonly">
        /// 差分のみを取得するかどうかを示す真偽値
        /// </param>
        /// 
        /// <returns>
        /// 差分の結果を保持するオブジェクト
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Cube.Differences.Result<T>> Diff<T>(
            this IEnumerable<T> newer, IEnumerable<T> older, bool diffonly = true)
            => new Cube.Differences.OnpAlgorithm<T>().Compare(older, newer, diffonly);

        /* ----------------------------------------------------------------- */
        ///
        /// Diff
        /// 
        /// <summary>
        /// 差分を検出します。
        /// </summary>
        ///
        /// <param name="newer">変更後のオブジェクト</param>
        /// <param name="older">変更前のオブジェクト</param>
        /// <param name="comparer">比較用オブジェクト</param>
        /// <param name="diffonly">
        /// 差分のみを取得するかどうかを示す真偽値
        /// </param>
        /// 
        /// <returns>
        /// 差分の結果を保持するオブジェクト
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Cube.Differences.Result<T>> Diff<T>(
            this IEnumerable<T> newer, IEnumerable<T> older,
            IEqualityComparer<T> comparer, bool diffonly = true)
            => new Cube.Differences.OnpAlgorithm<T>(comparer).Compare(older, newer, diffonly);

        /* ----------------------------------------------------------------- */
        ///
        /// Diff
        /// 
        /// <summary>
        /// 差分を検出します。
        /// </summary>
        ///
        /// <param name="newer">変更後のオブジェクト</param>
        /// <param name="older">変更前のオブジェクト</param>
        /// <param name="compare">比較関数</param>
        /// <param name="diffonly">
        /// 差分のみを取得するかどうかを示す真偽値
        /// </param>
        /// 
        /// <returns>
        /// 差分の結果を保持するオブジェクト
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Cube.Differences.Result<T>> Diff<T>(
            this IEnumerable<T> newer, IEnumerable<T> older,
            Func<T, T, bool> compare, bool diffonly = true)
            => Diff(older, newer, new GenericEqualityComparer<T>(compare), diffonly);

        #endregion

        #region IList(T)

        /* ----------------------------------------------------------------- */
        ///
        /// Clamp
        /// 
        /// <summary>
        /// 指定されたインデックスを [0, IList(T).Count) の範囲に丸めます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int Clamp<T>(this IList<T> src, int index)
            => Math.Min(Math.Max(index, 0), LastIndex(src));

        /* ----------------------------------------------------------------- */
        ///
        /// LastIndex
        /// 
        /// <summary>
        /// 最後のインデックスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int LastIndex<T>(this IList<T> src)
            => (src == null || src.Count == 0) ? 0 : src.Count - 1;

        #endregion
    }
}
