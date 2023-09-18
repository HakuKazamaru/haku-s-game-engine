using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using DxLibDLL;

using HakusGameEngine.common.Controller;
using HakusGameEngine.common.Function;
using HakusGameEngine.common.Model;

namespace HakusGameEngine
{
    /// <summary>
    /// プログラムクラス
    ///   メインロジック定義用
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 設定読込
        /// </summary>
        public static SettingController config = new SettingController("Haku's Game Engine");

        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// デバッグ情報
        /// </summary>
        private static DebugInfoModel debugInfo = new DebugInfoModel();

        /// <summary>
        /// メインスレッド
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // NLogの設定反映
            NLog.LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");

            // イニシャライズ処理
            if (!VideoFunc.InitializeVideMode(config.GetConfig()))
            {
                // ゲームメインループ
                while (DX.ProcessMessage() == 0)
                {

                    // キー入力状態管理用
                    List<KeyBindModel> keyStatus = InputFunction.GetInputKeyAllCode(config.GetConfig(), ref debugInfo);

                    // 一時的：エスケープで終了
                    if (keyStatus[0].StartKey != 0 || keyStatus[1].StartKey != 0)
                    {
                        break;
                    }

                    // 画面描画
                    VideoFunc.DrawScreen(config.GetConfig(), ref debugInfo);
                }

                // DXライブラリー解放
                DX.DxLib_End();
                logger.Info("DXライブラリを解放しました。");
            }
            else
            {
                logger.Fatal("ゲームの起動に失敗しました。");
                MessageBox.Show("ゲームの起動に失敗しました。", "DirectX初期化エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            logger.Info("==============================   End    ==============================");
        }


    }
}