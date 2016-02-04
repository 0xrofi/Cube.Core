﻿/* ------------------------------------------------------------------------- */
///
/// AssemblyReaderTest.cs
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
using System.Drawing;
using System.Reflection;
using NUnit.Framework;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssemblyReaderTest
    /// 
    /// <summary>
    /// AssemblyReader のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class AssemblyReaderTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Properties
        ///
        /// <summary>
        /// 各種プロパティのテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Properties

        [Test]
        public void Location_ExecutingAssembly_IsNotNullOrEmpty()
        {
            Assert.That(
                Create().Location,
                Is.Not.Null.Or.Empty
            );
        }

        [TestCase("Cube.Core testing project")]
        public void Title_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Title,
                Is.EqualTo(expected)
            );
        }

        [TestCase("NUnit framework を用いて Cube.Core プロジェクトをテストします。")]
        public void Description_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Description,
                Is.EqualTo(expected)
            );
        }

        [TestCase("CubeSoft")]
        public void Company_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Company,
                Is.EqualTo(expected)
            );
        }

        [TestCase("Cube.Core.Tests")]
        public void Product_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Product,
                Is.EqualTo(expected)
            );
        }

        [TestCase("Copyright © 2010 CubeSoft, Inc.")]
        public void Copyright_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Copyright,
                Is.EqualTo(expected)
            );
        }

        [TestCase("ここに商標を設定します。")]
        public void Trademark_ExecutingAssembly(string expected)
        {
            Assert.That(
                Create().Trademark,
                Is.EqualTo(expected)
            );
        }

        [Test]
        public void Configuration_ExecutingAssembly_IsEmpty()
        {
            Assert.That(
                Create().Configuration,
                Is.Empty
            );
        }

        [Test]
        public void Culture_ExecutingAssembly_IsEmpty()
        {
            Assert.That(
                Create().Culture,
                Is.Empty
            );
        }

        [TestCase(1, 2, 0, 0)]
        public void Version_ExecutingAssembly_IsAtLeast(int major, int minor, int build, int revision)
        {
            Assert.That(
                Create().Version,
                Is.AtLeast(new Version(major, minor, build, revision))
            );
        }

        [TestCase(1, 2, 0, 0)]
        public void FileVersion_ExecutingAssembly_IsAtLeast(int major, int minor, int build, int revision)
        {
            Assert.That(
                Create().FileVersion,
                Is.AtLeast(new Version(major, minor, build, revision))
            );
        }

        [TestCase(16, 16)]
        public void Icon_ExecutingAssembly(int width, int height)
        {
            Assert.That(
                Create().Icon.Size,
                Is.EqualTo(new Size(width, height))
            );
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// AssemblyReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AssemblyReader Create()
        {
            return new AssemblyReader(Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}
