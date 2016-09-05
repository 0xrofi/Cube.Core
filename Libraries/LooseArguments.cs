﻿/* ------------------------------------------------------------------------- */
///
/// LooseArguments.cs
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
using System.Collections.Generic;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// LooseArguments
    /// 
    /// <summary>
    /// コマンドライン等の引数を解析するクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// このクラスでは、各オプション ("-" または "--" で始まる引数）は最大
    /// 1 つの引数しか指定できないと言う制約を設けています。それ以外の
    /// 引数は、全て Get(void) メソッドで取得できる配列に格納されます。
    /// また、同じオプションが複数回指定された場合、最後に指定された引数を
    /// 保持します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class LooseArguments
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// LooseArguments
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public LooseArguments(char prefix = '-')
        {
            Prefix = prefix;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// LooseArguments
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public LooseArguments(IEnumerable<string> args, char prefix = '-')
            : this(prefix)
        {
            Parse(args);
        }

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Prefix
        /// 
        /// <summary>
        /// オプションを示す接頭辞を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public char Prefix { get; set; }

        #endregion

        #region Methods

        /* --------------------------------------------------------------------- */
        ///
        /// Parse
        /// 
        /// <summary>
        /// コマンドライン等の引数を解析します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Parse(IEnumerable<string> args)
        {
            var option = string.Empty;

            foreach (var s in args)
            {
                if (string.IsNullOrEmpty(s)) continue;

                if (s[0] == Prefix)
                {
                    if (!string.IsNullOrEmpty(option)) UpdateOption(option, null);
                    option = TrimLeft(s);
                }
                else if (!string.IsNullOrEmpty(option))
                {
                    UpdateOption(option, s);
                    option = string.Empty;
                }
                else _arguments.Add(s);
            }

            if (!string.IsNullOrEmpty(option)) UpdateOption(option, null);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Get
        /// 
        /// <summary>
        /// 引数一覧を取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// 各種オプションに対応する引数を取得する場合は Get(string) を
        /// 実行して下さい。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public IEnumerable<string> Get()
            => _arguments;

        /* --------------------------------------------------------------------- */
        ///
        /// Get
        /// 
        /// <summary>
        /// 各種オプションに対応する引数を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public string Get(string option)
            => Contains(option) ? _options[option] : null;

        /* --------------------------------------------------------------------- */
        ///
        /// Contains
        /// 
        /// <summary>
        /// オプションが指定されかどうかを判別します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public bool Contains(string option)
            => _options.ContainsKey(option);

        #endregion

        #region Others

        /* --------------------------------------------------------------------- */
        ///
        /// TrimLeft
        /// 
        /// <summary>
        /// 文字列から先頭の "-" および空白文字を除去します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private string TrimLeft(string option)
        {
            var index = 0;
            while (index < option.Length)
            {
                var c = option[index];
                if (!char.IsWhiteSpace(c) && c != Prefix) break;
                ++index;
            }
            return option.Substring(index);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// UpdateOption
        /// 
        /// <summary>
        /// オプションの内容を更新します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void UpdateOption(string key, string value)
        {
            if (_options.ContainsKey(key)) _options[key] = value;
            else _options.Add(key, value);
        }

        #endregion

        #region Fields
        private IList<string> _arguments = new List<string>();
        private IDictionary<string, string> _options = new Dictionary<string, string>();
        #endregion
    }
}
