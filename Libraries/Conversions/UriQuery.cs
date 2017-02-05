﻿/* ------------------------------------------------------------------------- */
///
/// UriQuery.cs
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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Conversions
{
    /* --------------------------------------------------------------------- */
    ///
    /// UriQuery
    /// 
    /// <summary>
    /// Sytem.Uri の拡張メソッド用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class UriQuery
    {
        /* ----------------------------------------------------------------- */
        ///
        /// With
        /// 
        /// <summary>
        /// Uri オブジェクトに指定したクエリーを付与します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri With(this Uri uri, IDictionary<string, string> query)
        {
            if (uri == null || query == null || query.Count <= 0) return uri;

            var dest = new UriBuilder(uri);
            var str  = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));
            dest.Query = dest != null && dest.Query.Length > 1 ?
                         $"{dest.Query.Substring(1)}&{str}" :
                         str;
            return dest.Uri;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// With
        /// 
        /// <summary>
        /// Uri オブジェクトに指定したクエリーを付与します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri With<T>(this Uri uri, string key, T value)
        {
            var query = new Dictionary<string, string>();
            query.Add(key, value.ToString());
            return With(uri, query);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// With
        /// 
        /// <summary>
        /// Uri オブジェクトに指定した時刻を付与します。
        /// </summary>
        /// 
        /// <remarks>
        /// 時刻は UnixTime に変換した上で、t=(unix) と言う形で
        /// Uri オブジェクトに付与されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri With(this Uri uri, DateTime time)
            => With(uri, Properties.Resources.keyTime, time.ToUnixTime());

        /* ----------------------------------------------------------------- */
        ///
        /// With
        /// 
        /// <summary>
        /// Uri オブジェクトにバージョン情報を付与します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri With(this Uri uri, SoftwareVersion version)
            => With(uri, Properties.Resources.KeyVersion, version.ToString(false));

        /* ----------------------------------------------------------------- */
        ///
        /// With
        /// 
        /// <summary>
        /// Uri オブジェクトに UTM クエリの情報を付与します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri With(this Uri uri, UtmQuery utm)
        {
            if (utm == null) return uri;

            var query = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(utm.Source)) query.Add("utm_source", utm.Source);
            if (!string.IsNullOrEmpty(utm.Medium)) query.Add("utm_medium", utm.Medium);
            if (!string.IsNullOrEmpty(utm.Campaign)) query.Add("utm_campaign", utm.Campaign);
            if (!string.IsNullOrEmpty(utm.Term)) query.Add("utm_term", utm.Term);
            if (!string.IsNullOrEmpty(utm.Content)) query.Add("utm_content", utm.Content);
            return With(uri, query);
        }
    }
}
