﻿/* ------------------------------------------------------------------------- */
///
/// ObservableValue.cs
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Cube.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ObservableValue
    /// 
    /// <summary>
    /// 各種 SettingsValue の基底クラスとなります。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class ObservableValue : INotifyPropertyChanged
    {
        #region Constructor

        /* ----------------------------------------------------------------- */
        ///
        /// ObservableValue
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <remarks>
        /// このクラスを直接オブジェクト化する事はできません。継承クラスを
        /// 使用して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected ObservableValue() { }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// PropertyChanged
        /// 
        /// <summary>
        /// プロパティが変更された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnPropertyChanged
        /// 
        /// <summary>
        /// PropertyChanged イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        #endregion

        #region Non-virtual protected methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetProperty
        /// 
        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string name = null) =>
            SetProperty(ref field, value, EqualityComparer<T>.Default, name);

        /* ----------------------------------------------------------------- */
        ///
        /// SetProperty
        /// 
        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected bool SetProperty<T>(ref T field, T value,
            IEqualityComparer<T> func, [CallerMemberName] string name = null)
        {
            if (func.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(new PropertyChangedEventArgs(name));
            return true;
        }

        #endregion
    }
}
