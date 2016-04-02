﻿/* ------------------------------------------------------------------------- */
///
/// FileHandlerTest.cs
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
using IoEx = System.IO;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileHandlerTest
    /// 
    /// <summary>
    /// FileHandler のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class FileHandlerTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Move_Overwrite
        ///
        /// <summary>
        /// 上書き移動のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.txt")]
        public void Move_Overwrite(string filename)
            => Assert.DoesNotThrow(() =>
        {
            var op = new Cube.FileSystem.FileHandler();
            op.Failed += (s, e) => Assert.Fail($"{e.Key}: {e.Value}");

            var name = IoEx.Path.GetFileNameWithoutExtension(filename);
            var ext  = IoEx.Path.GetExtension(filename);
            var src  = IoEx.Path.Combine(Results, filename);
            var dest = IoEx.Path.Combine(Results, $"{name}-Move{ext}");

            op.Copy(IoEx.Path.Combine(Examples, filename), src, false);
            op.Copy(src, dest, false);
            op.Move(src, dest, true);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Move_Failed
        ///
        /// <summary>
        /// 移動操作に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Move_Failed()
        {
            var failed = false;
            var op = new Cube.FileSystem.FileHandler();
            op.Failed += (s, e) =>
            {
                failed   = true;
                e.Cancel = true;
            };

            var src = IoEx.Path.Combine(Results, "FileNotFound.txt");
            var dest = IoEx.Path.Combine(Results, "Moved.txt");
            op.Move(src, dest, true);

            Assert.That(failed, Is.True);
        }

        #endregion
    }
}
