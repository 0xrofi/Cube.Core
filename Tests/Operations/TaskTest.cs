﻿/* ------------------------------------------------------------------------- */
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
using System.Threading.Tasks;
using NUnit.Framework;
using Cube.Tasks;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// TaskTest
    /// 
    /// <summary>
    /// Cube.Tasks.Oprations のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class TaskTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Forget
        ///
        /// <summary>
        /// Task の Fire&Forget をテストします。
        /// </summary>
        /// 
        /// <remarks>
        /// 例外発生時にはログに出力されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Forget()
        {
            Task.Run(() => throw new InvalidOperationException()).Forget();
            Assert.Pass();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Timeout
        ///
        /// <summary>
        /// TimeoutException が発生するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Timeout()
            => Assert.That(
                async () =>
                {
                    await Task.Run(() => { while (true) { } })
                              .Timeout(TimeSpan.FromMilliseconds(50));
                },
                Throws.TypeOf<TimeoutException>()
            );

        /* ----------------------------------------------------------------- */
        ///
        /// Timeout_NotThrow
        ///
        /// <summary>
        /// Timeout(TimeSpan) 実行後、TimeoutException が発生せずに処理が
        /// 終了するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(5u, ExpectedResult = 5)]
        public async Task<long> Timeout_NotThrow(uint n)
            => await Task.Run(() => Fibonacci(n))
                         .Timeout(TimeSpan.FromSeconds(100));

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Fibonacci
        ///
        /// <summary>
        /// Timeout をテストするためのダミー処理です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private long Fibonacci(uint n)
            => n == 0 ? 0 :
               n == 1 ? 1 :
               Fibonacci(n - 1) + Fibonacci(n - 2);

        #endregion
    }
}
