using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;

using NLog.Config;
using NLog.Extensions.Logging;

using DxLibDLL;

using HakusGameEngine.common.Model;

namespace HakusGameEngine.common.Controller
{
    /// <summary>
    /// 設定情報処理クラス
    /// </summary>
    public class SettingController
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 設定情報管理オブジェクト
        /// </summary>
        private SettingModel setting = new SettingModel();

        /// <summary>
        /// ゲーム名
        /// </summary>
        private string gameName = "Haku's Game Engine";

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="GameName">ゲーム名</param>
        public SettingController(string GameName)
        {
            logger.Trace("==============================  Start   ==============================");

            this.gameName = GameName;

            try
            {
                string settingDir = Directory.GetCurrentDirectory();
                logger.Debug("起動パス：{0}", settingDir);

                // アプリケーション設定ファイル読込
                if (this.CheckUserSettingFileExists())
                {
                    settingDir = GetUserSettingPath();
                    logger.Debug("ユーザー設定値を読み出します。パス:{0}", settingDir);
                }
                else
                {
                    settingDir = Directory.GetCurrentDirectory();
                    logger.Debug("初期値を読み出します。パス:{0}", settingDir);
                }
                this.setting.AppConfig = new ConfigurationBuilder()
                    .SetBasePath(settingDir)
                    .AddJsonFile(path: "appsettings.json")
                    .Build();
                this.setting.ConfigPath = Path.Combine(settingDir, "appsettings.json");
                logger.Debug("アプリケーション設定ファイルを読み込みました。");

                // NLog設定読込
                this.setting.NLogConfiguration = new NLogLoggingConfiguration(this.setting.AppConfig.GetSection("NLog"));
                NLog.LogManager.Configuration = this.setting.NLogConfiguration;
                logger.Debug("NLog設定を読み込みました。パス:{0}", this.setting.ConfigPath);

                // 設定値読み込み
                if (this.LoadAppSettings())
                {
                    logger.Info("ゲーム設定を読み込みました。");
                }
                if (this.LoadVideoSettings())
                {
                    logger.Info("ビデオ設定を読み込みました。");
                }
                if (this.LoadSoundSettings())
                {
                    logger.Info("音声設定を読み込みました。");
                }
                if (this.LoadControllerSettings())
                {
                    logger.Info("ジョイパッド設定を読み込みました。");
                }
                if (this.LoadKeyboardSettings())
                {
                    logger.Info("キーボード設定を読み込みました。");
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "SettingControllerの初期化でエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// NLog設定取得用メソッド
        /// </summary>
        /// <returns></returns>
        public LoggingConfiguration GetNLogSetting()
        {
            return this.setting.NLogConfiguration;
        }

        /// <summary>
        /// ゲーム設定読み取り
        /// </summary>
        /// <returns></returns>
        public bool LoadAppSettings()
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            try
            {
                IConfigurationSection section = this.setting.AppConfig.GetSection("appSettings");

                returnVal = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "LoadAppSettingsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ビデオ設定読み取り
        /// </summary>
        /// <returns></returns>
        public bool LoadVideoSettings()
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string tmpString = "";
                uint tmpUint = 0;
                IConfigurationSection section = this.setting.AppConfig.GetSection("videoSettings");

                // 画面モード読み込み
                tmpString = section["screenMode"];
                logger.Debug("ScreenMode  : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.ScreenMode = tmpUint;
                }
                else
                {
                    this.setting.ScreenMode = ConstModel.FullScreenMode;
                }

                // カラーモード読み込み
                tmpString = section["colorMode"];
                logger.Debug("ColorMode   : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.ColorMode = tmpUint == 32 ? true : false;
                }
                else
                {
                    this.setting.ColorMode = false;
                }

                // VSyncモード読み込み
                tmpString = section["vsync"];
                logger.Debug("VSync       : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.VSync = tmpUint == 1 ? true : false;
                }
                else
                {
                    this.setting.VSync = false;
                }

                // 解像度読み込み
                tmpString = section["resolution"];
                logger.Debug("Resolution  : {0}", tmpString);
                // 横解像度
                if (uint.TryParse(tmpString.ToLower().Split("x")[0], out tmpUint))
                {
                    this.setting.ScreenSizeX = tmpUint;
                }
                else
                {
                    this.setting.ScreenSizeX = 1280;
                }
                // 縦解像度
                if (uint.TryParse(tmpString.ToLower().Split("x")[1], out tmpUint))
                {
                    this.setting.ScreenSizeY = tmpUint;
                }
                else
                {
                    this.setting.ScreenSizeY = 720;
                }

                // フレームレート読み込み
                tmpString = section["framerate"];
                logger.Debug("Framerate   : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.FrameRate = tmpUint;
                }
                else
                {
                    this.setting.FrameRate = 60;
                }

                returnVal = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "LoadVideoSettingsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 音響設定読み取り
        /// </summary>
        /// <returns></returns>
        public bool LoadSoundSettings()
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string tmpString = "";
                uint tmpUint = 0;
                IConfigurationSection section = this.setting.AppConfig.GetSection("soundSettings");

                // マスターボリューム読み込み
                tmpString = section["master"];
                logger.Debug("MasterVolume: {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.MasterVolume = tmpUint;
                }
                else
                {
                    this.setting.MasterVolume = 80;
                }

                // BGMボリューム読み込み
                tmpString = section["bgm"];
                logger.Debug("BGM Volume  : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.BgmVolume = tmpUint;
                }
                else
                {
                    this.setting.BgmVolume = 80;
                }

                // 効果音ボリューム読み込み
                tmpString = section["se"];
                logger.Debug("SE Volume   : {0}", tmpString);
                if (uint.TryParse(tmpString, out tmpUint))
                {
                    this.setting.SeVolume = tmpUint;
                }
                else
                {
                    this.setting.SeVolume = 80;
                }

                returnVal = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "LoadSoundSettingsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// コントローラー設定読み取り
        /// </summary>
        /// <returns></returns>
        public bool LoadControllerSettings()
        {
            logger.Trace("==============================   Call    ==============================");
            return this.LoadKeyBindSettings(false);
        }

        /// <summary>
        /// キーボード設定読み取り
        /// </summary>
        /// <returns></returns>
        public bool LoadKeyboardSettings()
        {
            logger.Trace("==============================   Call    ==============================");
            return this.LoadKeyBindSettings(true);
        }

        /// <summary>
        /// キー配列読み取り共通処理
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool LoadKeyBindSettings(bool mode)
        {
            bool returnVal = false;
            string loadTargetName = mode ? "controllerSettings" : "keyboardSettings";
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string tmpString = "";
                int tmpInt = 0;
                IConfigurationSection section = this.setting.AppConfig.GetSection(loadTargetName);

                // 十字キー：上読み込み
                tmpString = section["up"];
                logger.Debug("DPadUpKey    : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadUpKey = tmpInt;
                    else
                        this.setting.KeyboardBind.DPadUpKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadUpKey = DX.PAD_INPUT_UP;
                    else
                        this.setting.KeyboardBind.DPadUpKey = DX.KEY_INPUT_UP;
                }

                // 十字キー：下読み込み
                tmpString = section["down"];
                logger.Debug("DPadDownKey  : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadDownKey = tmpInt;
                    else
                        this.setting.KeyboardBind.DPadDownKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadDownKey = DX.PAD_INPUT_DOWN;
                    else
                        this.setting.KeyboardBind.DPadDownKey = DX.KEY_INPUT_DOWN;
                }

                // 十字キー：左読み込み
                tmpString = section["left"];
                logger.Debug("DPadLeftKey  : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadLeftKey = tmpInt;
                    else
                        this.setting.KeyboardBind.DPadLeftKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadLeftKey = DX.PAD_INPUT_LEFT;
                    else
                        this.setting.KeyboardBind.DPadLeftKey = DX.KEY_INPUT_LEFT;
                }

                // 十字キー：右読み込み
                tmpString = section["right"];
                logger.Debug("DPadRightKey : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadRightKey = tmpInt;
                    else
                        this.setting.KeyboardBind.DPadRightKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.DPadRightKey = DX.PAD_INPUT_RIGHT;
                    else
                        this.setting.KeyboardBind.DPadRightKey = DX.KEY_INPUT_RIGHT;
                }

                // ボタン１（決定）読み込み
                tmpString = section["button1"];
                logger.Debug("Button1      : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.Button1 = tmpInt;
                    else
                        this.setting.KeyboardBind.Button1 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.Button1 = DX.PAD_INPUT_1;
                    else
                        this.setting.KeyboardBind.Button1 = DX.KEY_INPUT_Z;
                }

                // ボタン２（キャンセル）読み込み
                tmpString = section["button2"];
                logger.Debug("Button2      : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.Button2 = tmpInt;
                    else
                        this.setting.KeyboardBind.Button2 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.Button2 = DX.PAD_INPUT_2;
                    else
                        this.setting.KeyboardBind.Button2 = DX.KEY_INPUT_X;
                }

                // ボタン３読み込み
                tmpString = section["button3"];
                logger.Debug("Button3      : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.Button3 = tmpInt;
                    else
                        this.setting.KeyboardBind.Button3 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.Button3 = DX.PAD_INPUT_3;
                    else
                        this.setting.KeyboardBind.Button3 = DX.KEY_INPUT_C;
                }

                // ボタン４読み込み
                tmpString = section["button4"];
                logger.Debug("Button4      : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.Button4 = tmpInt;
                    else
                        this.setting.KeyboardBind.Button4 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.Button4 = DX.PAD_INPUT_4;
                    else
                        this.setting.KeyboardBind.Button4 = DX.KEY_INPUT_V;
                }

                // スタート読み込み
                tmpString = section["start"];
                logger.Debug("StartKey     : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.StartKey = tmpInt;
                    else
                        this.setting.KeyboardBind.StartKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.StartKey = DX.PAD_INPUT_8;
                    else
                        this.setting.KeyboardBind.StartKey = DX.KEY_INPUT_RETURN;
                }

                // セレクト読み込み
                tmpString = section["select"];
                logger.Debug("SelectKey    : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.SelectKey = tmpInt;
                    else
                        this.setting.KeyboardBind.SelectKey = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.SelectKey = DX.PAD_INPUT_7;
                    else
                        this.setting.KeyboardBind.SelectKey = DX.KEY_INPUT_ESCAPE;
                }

                // Lボタン1読み込み
                tmpString = section["buttonL1"];
                logger.Debug("LeftKey1     : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey1 = tmpInt;
                    else
                        this.setting.KeyboardBind.LeftKey1 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey1 = DX.PAD_INPUT_5;
                    else
                        this.setting.KeyboardBind.LeftKey1 = DX.KEY_INPUT_A;
                }

                // Rボタン1読み込み
                tmpString = section["buttonR1"];
                logger.Debug("RightKey1    : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey1 = tmpInt;
                    else
                        this.setting.KeyboardBind.RightKey1 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey1 = DX.PAD_INPUT_6;
                    else
                        this.setting.KeyboardBind.RightKey1 = DX.KEY_INPUT_S;
                }

                // Lボタン2読み込み
                tmpString = section["buttonL2"];
                logger.Debug("LeftKey2     : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey2 = tmpInt;
                    else
                        this.setting.KeyboardBind.LeftKey2 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey2 = DX.PAD_INPUT_11;
                    else
                        this.setting.KeyboardBind.LeftKey2 = DX.KEY_INPUT_D;
                }

                // Rボタン2読み込み
                tmpString = section["buttonR2"];
                logger.Debug("RightKey2    : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey2 = tmpInt;
                    else
                        this.setting.KeyboardBind.RightKey2 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey2 = DX.PAD_INPUT_12;
                    else
                        this.setting.KeyboardBind.RightKey2 = DX.KEY_INPUT_F;
                }

                // Lボタン3読み込み
                tmpString = section["buttonL3"];
                logger.Debug("LeftKey3     : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey3 = tmpInt;
                    else
                        this.setting.KeyboardBind.LeftKey3 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.LeftKey3 = DX.PAD_INPUT_9;
                    else
                        this.setting.KeyboardBind.LeftKey3 = DX.KEY_INPUT_Q;
                }

                // Rボタン3読み込み
                tmpString = section["buttonR3"];
                logger.Debug("RightKey3    : {0}", tmpString);
                if (int.TryParse(tmpString, out tmpInt))
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey3 = tmpInt;
                    else
                        this.setting.KeyboardBind.RightKey3 = tmpInt;
                }
                else
                {
                    if (mode)
                        this.setting.JoyPadBind.RightKey3 = DX.PAD_INPUT_10;
                    else
                        this.setting.KeyboardBind.RightKey3 = DX.KEY_INPUT_W;
                }

                returnVal = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "LoadKeyBindSettingsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// キー割り当て初期化
        /// </summary>
        /// <param name="mode"></param>
        public void ResetKeyBindSettings(bool mode)
        {
            logger.Trace("==============================  Start   ==============================");

            if (mode)
            {
                // 十字キー：上読み込み
                this.setting.JoyPadBind.DPadUpKey = DX.PAD_INPUT_UP;
                // 十字キー：下読み込み
                this.setting.JoyPadBind.DPadDownKey = DX.PAD_INPUT_DOWN;
                // 十字キー：左読み込み
                this.setting.JoyPadBind.DPadLeftKey = DX.PAD_INPUT_LEFT;
                // 十字キー：右読み込み
                this.setting.JoyPadBind.DPadRightKey = DX.PAD_INPUT_RIGHT;
                // ボタン１（決定）読み込み
                this.setting.JoyPadBind.Button1 = DX.PAD_INPUT_1;
                // ボタン２（キャンセル）読み込み
                this.setting.JoyPadBind.Button2 = DX.PAD_INPUT_2;
                // ボタン３読み込み
                this.setting.JoyPadBind.Button3 = DX.PAD_INPUT_3;
                // ボタン４読み込み
                this.setting.JoyPadBind.Button4 = DX.PAD_INPUT_4;
                // スタート読み込み
                this.setting.JoyPadBind.StartKey = DX.PAD_INPUT_8;
                // セレクト読み込み
                this.setting.JoyPadBind.SelectKey = DX.PAD_INPUT_7;
                // Lボタン1読み込み
                this.setting.JoyPadBind.LeftKey1 = DX.PAD_INPUT_5;
                // Rボタン1読み込み
                this.setting.JoyPadBind.RightKey1 = DX.PAD_INPUT_6;
                // Lボタン2読み込み
                this.setting.JoyPadBind.LeftKey2 = DX.PAD_INPUT_11;
                // Rボタン2読み込み
                this.setting.JoyPadBind.RightKey2 = DX.PAD_INPUT_12;
                // Lボタン3読み込み
                this.setting.JoyPadBind.LeftKey3 = DX.PAD_INPUT_9;
                // Rボタン3読み込み
                this.setting.KeyboardBind.RightKey3 = DX.KEY_INPUT_W;
            }
            else
            {
                // 十字キー：上読み込み
                this.setting.KeyboardBind.DPadUpKey = DX.PAD_INPUT_UP;
                // 十字キー：下読み込み
                this.setting.KeyboardBind.DPadDownKey = DX.PAD_INPUT_DOWN;
                // 十字キー：左読み込み
                this.setting.KeyboardBind.DPadLeftKey = DX.PAD_INPUT_LEFT;
                // 十字キー：右読み込み
                this.setting.KeyboardBind.DPadRightKey = DX.PAD_INPUT_RIGHT;
                // ボタン１（決定）読み込み
                this.setting.KeyboardBind.Button1 = DX.KEY_INPUT_Z;
                // ボタン２（キャンセル）読み込み
                this.setting.KeyboardBind.Button2 = DX.KEY_INPUT_X;
                // ボタン３読み込み
                this.setting.KeyboardBind.Button3 = DX.KEY_INPUT_C;
                // ボタン４読み込み
                this.setting.KeyboardBind.Button4 = DX.KEY_INPUT_V;
                // スタート読み込み
                this.setting.KeyboardBind.StartKey = DX.KEY_INPUT_RETURN;
                // セレクト読み込み
                this.setting.KeyboardBind.SelectKey = DX.KEY_INPUT_ESCAPE;
                // Lボタン1読み込み
                this.setting.KeyboardBind.LeftKey1 = DX.KEY_INPUT_A;
                // Rボタン1読み込み
                this.setting.KeyboardBind.RightKey1 = DX.KEY_INPUT_S;
                // Lボタン2読み込み
                this.setting.KeyboardBind.LeftKey2 = DX.KEY_INPUT_D;
                // Rボタン2読み込み
                this.setting.KeyboardBind.RightKey2 = DX.KEY_INPUT_F;
                // Lボタン3読み込み
                this.setting.KeyboardBind.LeftKey3 = DX.KEY_INPUT_Q;
                // Rボタン3読み込み
                this.setting.JoyPadBind.RightKey3 = DX.PAD_INPUT_10;
            }

            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// ユーザー設定ファイルの存在確認
        /// </summary>
        /// <returns></returns>
        public bool CheckUserSettingFileExists()
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string filePath = GetUserSettingPath();
                logger.Info("ユーザー設定ファイル確認先：{0}", filePath);
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CheckUserSettingFileExistsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ユーザー設定ファイルのパス取得
        /// </summary>
        /// <returns></returns>
        private string GetUserSettingPath()
        {
            logger.Trace("==============================   Call    ==============================");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), this.gameName, "appsettings.json");
        }

        /// <summary>
        /// 設定情報取得メソッド
        /// </summary>
        /// <returns></returns>
        public SettingModel GetConfig()
        {
            logger.Trace("==============================   Call    ==============================");
            SettingModel returnVal = this.setting;
            return returnVal;
        }
    }
}
