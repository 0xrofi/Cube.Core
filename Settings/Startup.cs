﻿/* ------------------------------------------------------------------------- */
///
/// Startup.cs
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
using Microsoft.Win32;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Startup
    /// 
    /// <summary>
    /// スタートアップへの登録および削除を行うためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public abstract class Startup
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// レジストリへスタートアップを登録します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Register(string name, string command)
        {
            using (var subkey = Registry.CurrentUser.OpenSubKey(_RegRoot, true))
            {
                subkey.SetValue(name, command);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// レジストリからスタートアップ設定を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Delete(string name)
        {
            using (var subkey = Registry.CurrentUser.OpenSubKey(_RegRoot, true))
            {
                if (subkey == null) return;
                subkey.DeleteValue(name);
            }
        }

        #region Constant fields
        private static readonly string _RegRoot = @"Software\Microsoft\Windows\CurrentVersion\Run";
        #endregion
    }
}
