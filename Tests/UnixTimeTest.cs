﻿/* ------------------------------------------------------------------------- */
///
/// UnixTimeTest.cs
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
using NUnit.Framework;
using Cube.Extensions;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// UnixTimeTest
    /// 
    /// <summary>
    /// UnixTime のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class UnixTimeTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ToDateTime
        /// 
        /// <summary>
        /// 引数に指定された日時をいったん NTP タイムスタンプに変換し、
        /// 再度 DateTime オブジェクトに変換するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(1970, 1,  1, 0,  0, 0, 0)]
        [TestCase(2000, 1,  1, 0,  0, 0, 0)]
        [TestCase(2038, 1, 19, 3, 14, 8, 0)]
        [TestCase(2104, 1,  1, 0,  0, 0, 0)]
        public void ToDateTime(int y, int m, int d, int hh, int mm, int ss, int ms)
        {
            var src = new DateTime(y, m, d, hh, mm, ss, ms, DateTimeKind.Utc);
            Assert.That(
                src.ToUnixTime().ToDateTime(),
                Is.EqualTo(src)
            );
        }
    }
}
