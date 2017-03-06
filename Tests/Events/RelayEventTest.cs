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
using NUnit.Framework;

namespace Cube.Tests.Events
{
    /* --------------------------------------------------------------------- */
    ///
    /// RelayEventTest
    /// 
    /// <summary>
    /// RelayEvent のテスト用クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class RelayEventTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Publish_Subscribe
        ///
        /// <summary>
        /// Publish/Subscribe のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Publish_Subscribe()
        {
            var count = 0;
            var ev = new RelayEvent();
            ev.Subscribe(() => ++count);
            ev.Publish();
            ev.Publish();
            ev.Publish();
            Assert.That(count, Is.EqualTo(3));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Publish_Subscribe
        ///
        /// <summary>
        /// Publish/Subscribe のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(1)]
        [TestCase("pi")]
        [TestCase(true)]
        public void Publish_Subscribe<T>(T value)
        {
            var result = default(T);
            var ev = new RelayEvent<T>();
            ev.Subscribe(x => { result = x; });
            ev.Publish(value);
            Assert.That(result, Is.EqualTo(value));
        }
    }
}
