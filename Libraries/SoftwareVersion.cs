﻿/* ------------------------------------------------------------------------- */
///
/// SoftwareVersion.cs
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
using System.Text;
using System.Reflection;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// SoftwareVersion
    /// 
    /// <summary>
    /// ソフトウェアのバージョンを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SoftwareVersion
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SoftwareVersion
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SoftwareVersion() { }

        /* ----------------------------------------------------------------- */
        ///
        /// SoftwareVersion
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SoftwareVersion(Assembly assembly)
        {
            var number = assembly?.GetName()?.Version;
            if (number != null) Number = number;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Number
        /// 
        /// <summary>
        /// バージョン番号を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Version Number { get; set; } = new Version(1, 0, 0, 0);

        /* ----------------------------------------------------------------- */
        ///
        /// Digit
        /// 
        /// <summary>
        /// Number プロパティの有効桁数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Digit { get; set; } = 4;

        /* ----------------------------------------------------------------- */
        ///
        /// Prefix
        /// 
        /// <summary>
        /// バージョン番号の先頭に付与する文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Prefix { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Suffix
        /// 
        /// <summary>
        /// バージョン番号の末尾に付与する文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Suffix { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Platform
        /// 
        /// <summary>
        /// ソフトウェアのプラットフォームを示す文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Platform => (IntPtr.Size == 4) ? "x86" : "x64";

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        /// 
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override string ToString() => ToString(true);

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        /// 
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string ToString(bool platform)
        {
            var ss = new StringBuilder();

            if (!string.IsNullOrEmpty(Prefix)) ss.Append(Prefix);
            AppendNumber(ss);
            if (!string.IsNullOrEmpty(Suffix)) ss.Append(Suffix);
            if (platform) ss.Append($" ({Platform})");

            return ss.ToString();
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// AppendNumber
        ///
        /// <summary>
        /// バージョン番号を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AppendNumber(StringBuilder ss)
        {
            if (Number == null) return;

            ss.Append(Number.Major);
            if (Digit <= 1) return;

            ss.Append($".{Number.Minor}");
            if (Digit <= 2) return;

            ss.Append($".{Number.Build}");
            if (Digit <= 3) return;

            ss.Append($".{Number.Revision}");
        }

        #endregion
    }
}
