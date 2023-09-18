using Microsoft.Extensions.Configuration;
using NLog.Config;
using System.Security.Permissions;

namespace HakusGameEngine.common.Model
{
    /// <summary>
    /// 設定データ管理クラス
    /// </summary>
    public class SettingModel
    {
        /// <summary>
        /// アプリケーション設定ファイル管理用
        /// </summary>
        public IConfigurationRoot? AppConfig { get; set; }

        /// <summary>
        /// NLog設定
        /// </summary>
        public LoggingConfiguration NLogConfiguration { get; set; }

        /// <summary>
        /// 設定ファイル保存パス
        /// 　(ファイル名付きフルパス)
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// 画面モードの設定
        /// （0:全画面、1:ウィンドウ）
        /// </summary>
        public uint ScreenMode { get; set; }

        /// <summary>
        /// 画面解像度(横)
        /// </summary>
        public uint ScreenSizeX { get; set; }

        /// <summary>
        /// 画面解像度(縦)
        /// </summary>
        public uint ScreenSizeY { get; set; }

        /// <summary>
        /// カラービット
        ///  (True:32,false:16)
        /// </summary>
        public bool ColorMode { get; set; }

        /// <summary>
        /// VSync
        /// </summary>
        public bool VSync { get; set; }

        /// <summary>
        /// フレームレート
        /// </summary>
        public uint FrameRate { get; set; }

        /// <summary>
        /// 全体音量
        /// </summary>
        public uint MasterVolume { get; set; }

        /// <summary>
        /// 背景音楽音量
        /// </summary>
        public uint BgmVolume { get; set; }

        /// <summary>
        /// 効果音音量
        /// </summary>
        public uint SeVolume { get; set; }

        /// <summary>
        /// キー割り当て：キーボード配列
        /// </summary>
        public KeyBindModel KeyboardBind { get; set; }

        /// <summary>
        /// キー割り当て：ジョイパッド配列
        /// </summary>
        public KeyBindModel JoyPadBind { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SettingModel()
        {
            this.NLogConfiguration = new LoggingConfiguration();
            this.ConfigPath = "";
            this.KeyboardBind = new KeyBindModel();
            this.JoyPadBind = new KeyBindModel();
        }

    }
}
