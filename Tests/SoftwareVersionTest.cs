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
using System.Reflection;
using NUnit.Framework;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SoftwareVersionTest
    /// 
    /// <summary>
    /// SoftwareVersion のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class SoftwareVersionTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ToString_Assembly
        ///
        /// <summary>
        /// バージョンを表す文字列を取得するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ToString_Assembly()
        {
            var asm   = Assembly.GetExecutingAssembly();
            var major = asm.GetName().Version.Major;
            var minor = asm.GetName().Version.Minor;
            var arch  = (IntPtr.Size == 4) ? "x86" : "x64";

            var version = new SoftwareVersion(asm);
            version.Digit  = 2;
            version.Prefix = "begin-";
            version.Suffix = "-end";

            Assert.That(version.ToString(true),  Is.EqualTo($"begin-{major}.{minor}-end ({arch})"));
            Assert.That(version.ToString(false), Is.EqualTo($"begin-{major}.{minor}-end"));
            Assert.That(version.ToString(),      Is.EqualTo(version.ToString(false)));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Parse_DoesNotThrow
        ///
        /// <summary>
        /// バージョンを表す文字列を解析するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("1.0")]
        [TestCase("1.0.0")]
        [TestCase("1.0.0.0")]
        [TestCase("1.0.0.0-suffix")]
        [TestCase("v1.0.0.0-suffix")]
        [TestCase("v1.0.0.0-p21")]
        [TestCase("p21-v1.0.0.0-suffix")]
        public void Parse_DoesNotThrow(string src)
            => Assert.DoesNotThrow(() =>
        {
            var version = new SoftwareVersion(src);
            Assert.That(version.ToString(false), Is.EqualTo(src));
        });
    }
}
