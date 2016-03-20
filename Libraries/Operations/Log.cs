﻿/* ------------------------------------------------------------------------- */
///
/// Log.cs
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
using log4net;

namespace Cube.Log
{
    /* --------------------------------------------------------------------- */
    ///
    /// Log.Operations
    /// 
    /// <summary>
    /// Log オブジェクトに対する操作を定義するための拡張メソッド用クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public static class Operations
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// 初期設定を行います。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Configure()
            => log4net.Config.XmlConfigurator.Configure();

        /* ----------------------------------------------------------------- */
        ///
        /// Debug
        ///
        /// <summary>
        /// デバッグ情報をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Debug(Type type, string message)
            => Logger(type).Debug(message);

        public static void Debug(Type type, string message, Exception err)
            => Logger(type).Debug(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// Info
        ///
        /// <summary>
        /// 情報をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Info(Type type, string message)
            => Logger(type).Info(message);

        public static void Info(Type type, string message, Exception err)
            => Logger(type).Info(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// Warn
        ///
        /// <summary>
        /// 警告をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Warn(Type type, string message)
            => Logger(type).Warn(message);

        public static void Warn(Type type, string message, Exception err)
            => Logger(type).Warn(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// Error
        ///
        /// <summary>
        /// エラーをログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Error(Type type, string message)
            => Logger(type).Error(message);

        public static void Error(Type type, string message, Exception err)
            => Logger(type).Error(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// Fatal
        ///
        /// <summary>
        /// 致命的なエラーをログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Fatal(Type type, string message)
            => Logger(type).Fatal(message);

        public static void Fatal(Type type, string message, Exception err)
            => Logger(type).Fatal(message, err);

        #endregion

        #region Extended methods

        /* ----------------------------------------------------------------- */
        ///
        /// LogDebug
        ///
        /// <summary>
        /// デバッグ情報をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogDebug<T>(this T src, string message)
            => src.Logger().Debug(message);

        public static void LogDebug<T>(this T src, string message, Exception err)
            => src.Logger().Debug(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// LogInfo
        ///
        /// <summary>
        /// 情報をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogInfo<T>(this T src, string message)
            => src.Logger().Info(message);

        public static void LogInfo<T>(this T src, string message, Exception err)
            => src.Logger().Info(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// LogWarn
        ///
        /// <summary>
        /// 警告をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogWarn<T>(this T src, string message)
            => src.Logger().Warn(message);

        public static void LogWarn<T>(this T src, string message, Exception err)
            => src.Logger().Warn(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// LogError
        ///
        /// <summary>
        /// エラーをログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogError<T>(this T src, string message)
            => src.Logger().Error(message);

        public static void LogError<T>(this T src, string message, Exception err)
            => src.Logger().Error(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// LogFatal
        ///
        /// <summary>
        /// 致命的なエラーをログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogFatal<T>(this T src, string message)
            => src.Logger().Fatal(message);

        public static void LogFatal<T>(this T src, string message, Exception err)
            => src.Logger().Fatal(message, err);

        /* ----------------------------------------------------------------- */
        ///
        /// LogException
        ///
        /// <summary>
        /// 実行時に例外が発生した場合、その例外をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static void LogException<T>(this T src, Action action)
        {
            try { action(); }
            catch (Exception err) { src.Logger().Error(err.Message, err); }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Logger
        ///
        /// <summary>
        /// ログ出力用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ILog Logger(Type type)
            => LogManager.GetLogger(type);

        /* ----------------------------------------------------------------- */
        ///
        /// Logger
        ///
        /// <summary>
        /// ログ出力用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ILog Logger<T>(this T src)
            => Logger(src.GetType());

        #endregion
    }
}
