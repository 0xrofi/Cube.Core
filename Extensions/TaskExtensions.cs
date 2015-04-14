﻿/* ------------------------------------------------------------------------- */
///
/// TaskExtensions.cs
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
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Extensions
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Extensions.TaskExtensions
    /// 
    /// <summary>
    /// Sytem.Threading.Tasks.Task の拡張クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class TaskExtensions
    {
        /* --------------------------------------------------------------------- */
        ///
        /// Timeout
        /// 
        /// <summary>
        /// タスクにタイムアウトを設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static async Task Timeout(this Task task, TimeSpan timeout)
        {
            var delay = Cube.TaskEx.Delay(timeout);
            if (await Cube.TaskEx.WhenAny(task, delay) == delay)
            {
                throw new TimeoutException();
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Timeout
        /// 
        /// <summary>
        /// タスクにタイムアウトを設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan timeout)
        {
            await ((Task)task).Timeout(timeout);
            return await task;
        }
    }
}
