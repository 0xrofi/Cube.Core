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
using System.Linq;
using NUnit.Framework;
using Cube.FileSystem;

namespace Cube.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DriveTest
    /// 
    /// <summary>
    /// Drive のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class DriveTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Drive_Detach_Throws
        ///
        /// <summary>
        /// Device.Detach() のテストを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// システムドライブを指定するため VetoException が発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(0)]
        public void Drive_Detach_Throws(int index)
            => Assert.That(
            () => Drive.GetDrives().First().Detach(),
            Throws.TypeOf<VetoException>()
                  .And.Property("Reason").EqualTo(VetoType.IllegalDeviceRequest)
                  .And.Property("Name").Not.Null.Or.Empty
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetDrives_Count
        ///
        /// <summary>
        /// Drive.GetDrives() のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(1)]
        public void GetDrives_Count(int atleast)
            => Assert.That(Drive.GetDrives().Count(), Is.AtLeast(atleast));

        /* ----------------------------------------------------------------- */
        ///
        /// GetDrives_Count
        ///
        /// <summary>
        /// Drive.GetDrives() のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Drive_Properties()
        {
            var drive = Drive.GetDrives().First();
            Assert.That(drive.Index,       Is.EqualTo(0));
            Assert.That(drive.Letter,      Is.EqualTo("C:"));
            Assert.That(drive.Type,        Is.EqualTo(DriveType.HardDisk));
            Assert.That(drive.Format,      Is.EqualTo("NTFS"));
            Assert.That(drive.Interface,   Is.EqualTo("IDE"));
            Assert.That(drive.Size,        Is.AtLeast(100000000UL));
            Assert.That(drive.FreeSpace,   Is.GreaterThan(1).And.LessThan(drive.Size));
            Assert.That(drive.VolumeLabel, Is.Not.Null.Or.Empty);
            Assert.That(drive.Model,       Is.Not.Null.Or.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drive_Found
        ///
        /// <summary>
        /// ドライブレターを基にドライブ情報を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Drive_Found()
        {
            var drive = new Drive('c');
            Assert.That(drive.Letter, Is.EqualTo("C:"));
            Assert.That(drive.Type,   Is.EqualTo(DriveType.HardDisk));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drive_NotFound
        ///
        /// <summary>
        /// 無効なドライブレターを設定した時のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Drive_NotFound()
            => Assert.That(
            () => new Drive("InvalidLettter"),
            Throws.TypeOf<ArgumentException>()
        );
    }
}
