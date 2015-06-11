﻿/* ------------------------------------------------------------------------- */
///
/// Device.cs
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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.FileSystem.Device
    /// 
    /// <summary>
    /// デバイスに関する情報を保持するクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class Device
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Device
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Device(Drive drive)
        {
            Initialize(drive);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        /// 
        /// <summary>
        /// デバイスのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        /// 
        /// <summary>
        /// デバイス番号を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Index { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Handle
        /// 
        /// <summary>
        /// ハンドル (DeviceInstance) を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Handle { get; private set; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        /// 
        /// <summary>
        /// プロパティを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Initialize(Drive drive)
        {
            var guid = GetClassGuid(drive);
            if (guid == Guid.Empty) return;

            var handle = IntPtr.Zero;

            try
            {
                handle = GetDeviceInfo(guid);
                for (uint i = 0; true; ++i)
                {
                    var data = new SP_DEVICE_INTERFACE_DATA();
                    if (!SetupDiEnumDeviceInterfaces(handle, IntPtr.Zero, ref guid, i, data))
                    {
                        var code = Marshal.GetLastWin32Error();
                        if (code == 259 /* ERROR_NO_MORE_ITEMS */) break;
                        throw new Win32Exception(code);
                    }

                    var devinfo = new SP_DEVINFO_DATA();
                    var path = GetDevicePath(handle, data, devinfo);
                    var index = Cube.FileSystem.DeviceNumber.Get(path);
                    if (drive.Index == index)
                    {
                        Path = path;
                        Index = index;
                        Handle = devinfo.devInst;
                        break;
                    }
                }
            }
            finally { if (handle != IntPtr.Zero) SetupDiDestroyDeviceInfoList(handle); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceInfo
        /// 
        /// <summary>
        /// デバイス情報にアクセスするためのハンドルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IntPtr GetDeviceInfo(Guid guid)
        {
            const uint DIGCF_PRESENT         = 0x00000002;
            const uint DIGCF_DEVICEINTERFACE = 0x00000010;

            var dest = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (dest.ToInt32() == -1) throw new Win32Exception(Marshal.GetLastWin32Error());

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetClassGuid
        /// 
        /// <summary>
        /// 利用クラスに対応する GUID オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Guid GetClassGuid(Drive drive)
        {
            const string GUID_DEVINTERFACE_DISK   = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
            const string GUID_DEVINTERFACE_FLOPPY = "53f56311-b6bf-11d0-94f2-00a0c91efb8b";
            const string GUID_DEVINTERFACE_CDROM  = "53f56308-b6bf-11d0-94f2-00a0c91efb8b";

            switch (drive.Type)
            {
                case System.IO.DriveType.Removable:
                    return drive.MediaType == MediaType.FloppyDisk ?
                           new Guid(GUID_DEVINTERFACE_FLOPPY) :
                           new Guid(GUID_DEVINTERFACE_DISK);
                case System.IO.DriveType.Fixed:
                    return new Guid(GUID_DEVINTERFACE_DISK);
                case System.IO.DriveType.CDRom:
                    return new Guid(GUID_DEVINTERFACE_CDROM);
                default:
                    return Guid.Empty;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDevicePath
        /// 
        /// <summary>
        /// デバイスのパスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: x64 時の処理を（動作してはいるが）再考する。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDevicePath(IntPtr handle, SP_DEVICE_INTERFACE_DATA data, SP_DEVINFO_DATA devinfo)
        {
            var buffer = IntPtr.Zero;

            try
            {
                var size = GetRequiredSize(handle, data, devinfo);
                var detail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                if (IntPtr.Size == 8) detail.cbSize = 8; // x64
                else detail.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                buffer = Marshal.AllocHGlobal((int)size);
                Marshal.StructureToPtr(detail, buffer, false);

                var status = SetupDiGetDeviceInterfaceDetail(handle, data, buffer, size, ref size, devinfo);
                if (!status) throw new Win32Exception(Marshal.GetLastWin32Error());

                var pos = (IntPtr)((int)buffer + Marshal.SizeOf(typeof(int)));
                return Marshal.PtrToStringAuto(pos);
            }
            finally { if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRequiredSize
        /// 
        /// <summary>
        /// SetupDiGetDeviceInterfaceDetail を実行するために必要なバッファ
        /// サイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private uint GetRequiredSize(IntPtr handle, SP_DEVICE_INTERFACE_DATA data, SP_DEVINFO_DATA devinfo)
        {
            var dest = 0u;
            if (!SetupDiGetDeviceInterfaceDetail(handle, data, IntPtr.Zero, 0, ref dest, devinfo))
            {
                var code = Marshal.GetLastWin32Error();
                if (code != 122 /* ERROR_INSUFFICIENT_BUFFER */) throw new Win32Exception(code);
            }
            return dest;
        }

        #endregion

        #region Win32 APIs

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiGetClassDevs
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff551069.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator,
            IntPtr hwndParent, uint flags);

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiDestroyDeviceInfoList
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff550996.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport("setupapi.dll")]
        private static extern uint SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiEnumDeviceInterfaces
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff550996.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData,
            ref Guid interfaceClassGuid, uint memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiGetDeviceInterfaceDetail
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff551120.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize,
            ref uint requiredSize, SP_DEVINFO_DATA deviceInfoData);

        #endregion

        #region Classes or structures for Win32 APIs

        /* ----------------------------------------------------------------- */
        ///
        /// SP_DEVINFO_DATA
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552344.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [StructLayout(LayoutKind.Sequential)]
        internal class SP_DEVINFO_DATA
        {
            public uint cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            public Guid classGuid = Guid.Empty;
            public uint devInst = 0;
            public IntPtr reserved = IntPtr.Zero;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SP_DEVICE_INTERFACE_DATA
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552342.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [StructLayout(LayoutKind.Sequential)]
        internal class SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
            public Guid interfaceClassGuid = Guid.Empty;
            public uint flags = 0;
            public IntPtr reserved = IntPtr.Zero;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SP_DEVICE_INTERFACE_DETAIL_DATA
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552343.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            public short devicePath;
        }

        #endregion
    }
}
