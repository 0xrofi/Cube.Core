﻿using System;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.SettingsFolder
    /// 
    /// <summary>
    /// アプリケーション/ユーザ設定を保持するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class SettingsFolder<UserSettings> where UserSettings : new()
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Application
        ///
        /// <summary>
        /// アプリケーション固有の設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ApplicationSettings Application
        {
            get { return _app; }
            private set { _app = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// User
        ///
        /// <summary>
        /// ユーザ毎の設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public UserSettings User
        {
            get { return _user; }
            private set { _user = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Publisher
        ///
        /// <summary>
        /// アプリケーションの発行元を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Publisher
        {
            get { return _publisher; }
            private set { _publisher = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Product
        ///
        /// <summary>
        /// アプリケーション名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Product
        {
            get { return _product; }
            private set { _product = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SubkeyName
        ///
        /// <summary>
        /// レジストリ上で各種設定が保存されているサブキー名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SubKeyName
        {
            get { return string.Format(@"Software\{0}\{1}", Publisher, Product); }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// ユーザ設定をレジストリへ保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() { }

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// アプリケーション設定、およびユーザ設定をレジストリから
        /// 読み込みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Load()
        {
            LoadApplicationSettings();
            LoadUserSettings();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LoadApplicationSettings
        ///
        /// <summary>
        /// アプリケーション設定をレジストリから読み込みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void LoadApplicationSettings()
        {
            var root = Microsoft.Win32.Registry.LocalMachine;
            using (var subkey = root.OpenSubKey(SubKeyName, false))
            {
                Application = Settings.Load<ApplicationSettings>(subkey);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LoadUserSettings
        ///
        /// <summary>
        /// ユーザ設定をレジストリから読み込みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void LoadUserSettings()
        {
            var root = Microsoft.Win32.Registry.CurrentUser;
            using (var subkey = root.OpenSubKey(SubKeyName, false))
            {
                User = Settings.Load<UserSettings>(subkey);
            }
        }

        #endregion

        #region Fields
        private ApplicationSettings _app = new ApplicationSettings();
        private UserSettings _user = new UserSettings();
        private string _publisher = string.Empty;
        private string _product = string.Empty;
        #endregion
    }
}
