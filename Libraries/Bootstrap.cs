﻿/* ------------------------------------------------------------------------- */
///
/// Bootstrap.cs
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Lifetime;
using Cube.Log;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// Bootstrap
    ///
    /// <summary>
    /// プロセス間通信 (IPC: Inter-Process Communication) によって
    /// プロセスの起動およびアクティブ化を行うためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// 二重起動を抑止したい時に、二重起動する代わりに既に起動している
    /// 同名プロセスをアクティブ化します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Bootstrap : IDisposable
    {
        #region Constructors and the destructor

        /* ----------------------------------------------------------------- */
        ///
        /// Bootstrap
        /// 
        /// <summary>
        /// 静的オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static Bootstrap()
        {
            LifetimeServices.LeaseTime = TimeSpan.Zero;
            LifetimeServices.RenewOnCallTime = TimeSpan.Zero;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bootstrap
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bootstrap(string name)
        {
            Name = name;
            _mutex = new System.Threading.Mutex(false, Name);
            _core.Received += (s, e) => OnActivated(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IpcBootstrap
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~Bootstrap()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// ActivateCommand
        /// 
        /// <summary>
        /// アクティブ化するためのコマンドを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string ActivateCommand = "activate";

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        /// 
        /// <summary>
        /// プロセス間通信の際の識別子となる名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; private set; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Activated
        /// 
        /// <summary>
        /// 他のプロセスからアクティブ化された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler<ValueEventArgs<object>> Activated;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        /// 
        /// <summary>
        /// 同じ名前を持つプロセスが存在するかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists()
        {
            return !_mutex.WaitOne(0, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Activate
        /// 
        /// <summary>
        /// 既に起動しているプロセスをアクティブ化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Activate(object args = null)
            => this.LogException(() =>
        {
            var client = new IpcClientChannel();
            ChannelServices.RegisterChannel(client, true);
            var channel = $"ipc://{Name}/{ActivateCommand}";
            var proxy = Activator.GetObject(typeof(IpcProxy), channel) as IpcProxy;
            if (proxy != null)
            {
                proxy.Send(args);
                this.LogDebug(channel);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        /// 
        /// <summary>
        /// 他のプロセスからメッセージを受け取るための登録を行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Register()
            => this.LogException(() =>
        {
            var server = new IpcServerChannel(Name);
            ChannelServices.RegisterChannel(server, true);
            RemotingServices.Marshal(_core, ActivateCommand, typeof(IpcProxy));
            this.LogDebug($"{Name}/{ActivateCommand} registered");
        });

        #endregion

        #region Methods for IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                _mutex.Close();
            }
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnActivated
        /// 
        /// <summary>
        /// 他のプロセスからアクティブ化された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnActivated(ValueEventArgs<object> e)
        {
            if (Activated != null) Activated(this, e);
        }

        #endregion

        #region Internal class

        public class IpcProxy : MarshalByRefObject
        {
            public event EventHandler<ValueEventArgs<object>> Received;

            public void Send(object args)
            {
                if (Received != null) Received(this, new ValueEventArgs<object>(args));
            }
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        private System.Threading.Mutex _mutex = null;
        private IpcProxy _core = new IpcProxy();
        #endregion
    }
}
