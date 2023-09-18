using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DxLibDLL;

using HakusGameEngine.common.Model;

namespace HakusGameEngine.common.Function
{
    internal static class VideoFunc
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ビデオモードの初期化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool InitializeVideMode(SettingModel config)
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            // 画面モード初期設定
            if (!SetPreVideoMode(config))
            {
                logger.Fatal("画面モードの初期設定に失敗しました。");
                return returnVal;
            }

            // DXライブラリー初期化
            if (DX.DxLib_Init() == -1)
            {
                logger.Error("DXライブラリーの初期化に失敗しました。");
                return returnVal;
            }
            logger.Info("DXライブラリを初期化しました。");

            // 画面モード設定
            if (!SetVideoMode(config))
            {
                logger.Fatal("画面モードの設定に失敗しました。");
                return returnVal;
            }

            // 描画先画面を裏画面にセット
            if (DX.SetDrawScreen(DX.DX_SCREEN_BACK) == 0)
                logger.Debug("描画先をバッファーに設定しました。");
            else
            {
                logger.Error("描画先の切替に失敗しました。");
                return returnVal;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 画面モードの設定変更
        /// 　(DxLib_Init実行前分)
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool SetPreVideoMode(SettingModel config)
        {
            bool returnVal = false;
            int tmpIntValue = 0;
            logger.Trace("==============================  Start   ==============================");

            // 読み込まれた設定に基づいて画面モードを設定
            if (config.ScreenMode == ConstModel.WindowMode)
            {
                // 画面モードをセット
                switch (DX.ChangeWindowMode(DX.TRUE))
                {
                    case DX.DX_CHANGESCREEN_OK:
                        {
                            logger.Debug("画面モードをウィンドウに設定しました。");
                            break;
                        }
                    case DX.DX_CHANGESCREEN_DEFAULT:
                        {
                            logger.Warn("画面モードの変更に失敗しましたが前回の画面モードを引き継ぎました。");
                            break;
                        }
                    default:
                        {
                            logger.Error("画面モードの変更に失敗しました。");
                            returnVal = false;
                            return returnVal;
                        }
                }
            }
            else
            {
                // 画面モードをセット
                switch (DX.ChangeWindowMode(DX.FALSE))
                {
                    case DX.DX_CHANGESCREEN_OK:
                        {
                            logger.Debug("画面モードを全画面に設定しました。");
                            break;
                        }
                    case DX.DX_CHANGESCREEN_DEFAULT:
                        {
                            logger.Warn("画面モードの変更に失敗しましたが前回の画面モードを引き継ぎました。");
                            break;
                        }
                    default:
                        {
                            logger.Error("画面モードの変更に失敗しました。");
                            returnVal = false;
                            return returnVal;
                        }
                }

                // スケーリング基準モードを設定
                if (DX.SetFullScreenResolutionMode(DX.DX_FSRESOLUTIONMODE_DESKTOP) == 0)
                    logger.Debug("スケーリング基準モードをデスクトップ解像度基準に設定しました。");
                else
                {
                    logger.Error("スケーリング基準モードの設定に失敗しました。");
                    return returnVal;
                }

            }

            // 画面解像度をセット
            tmpIntValue = config.ColorMode ? 32 : 16;
            switch (DX.SetGraphMode((int)config.ScreenSizeX, (int)config.ScreenSizeY, tmpIntValue))
            {
                case DX.DX_CHANGESCREEN_OK:
                    {
                        logger.Debug("解像度を{0}x{1} {2}bitに設定しました。", config.ScreenSizeX, config.ScreenSizeY, tmpIntValue);
                        break;
                    }
                case DX.DX_CHANGESCREEN_DEFAULT:
                    {
                        logger.Warn("解像度の変更に失敗しましたが前回の解像度を引き継ぎました。");
                        break;
                    }
                default:
                    {
                        logger.Error("解像度の変更に失敗しました。");
                        returnVal = false;
                        return returnVal;
                    }
            }

            // VSyncモードをセット
            if (DX.SetWaitVSyncFlag(config.VSync ? DX.TRUE : DX.FALSE) == 0)
                logger.Debug("VSyncを{0}に設定しました。", config.VSync);
            else
            {
                logger.Error("VSyncの設定に失敗しました。");
                returnVal = false;
                return returnVal;
            }

            returnVal = true;

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 画面モードの設定変更
        ///  (DxLib_Init実行後分)
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool SetVideoMode(SettingModel config)
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            if (config.ScreenMode != ConstModel.WindowMode)
            {
                // スケーリングモードを設定
                if (DX.SetFullScreenScalingMode(DX.DX_FSSCALINGMODE_BILINEAR) == 0)
                    logger.Debug("スケーリングモードをバイリニアモードに設定しました。");
                else
                {
                    logger.Error("スケーリング基準モードの設定に失敗しました。");
                    return returnVal;
                }
            }

            if (DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST) == 0)
                logger.Debug("描画モードをネアレストネイバーに設定しました。");
            else
            {
                logger.Error("スケーリング基準モードの設定に失敗しました。");
                return returnVal;
            }

            returnVal = true;

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 画面描画処理
        /// </summary>
        /// <param name="config"></param>
        /// <param name="debugInfoModel"></param>
        public static void DrawScreen(SettingModel config, ref DebugInfoModel debugInfoModel)
        {
            bool needDraw = false;
            logger.Trace("==============================  Start   ==============================");

            // 内部FPS計算
            CalcInternalFps(ref debugInfoModel);

            // FPS計算
            needDraw = CalcFps(config, ref debugInfoModel);
            if (needDraw || config.VSync)
            {
                // 画面を初期化する
                DX.ClearDrawScreen();

                // デバッグ情報の表示
                DebugFunction.DebugInfoPrint(debugInfoModel);

                // 裏画面の内容を表画面に反映させる
                DX.ScreenFlip();
            }
            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// 描画FPS計算処理
        /// </summary>
        /// <param name="config"></param>
        /// <param name="debugInfoModel"></param>
        /// <returns></returns>
        public static bool CalcFps(SettingModel config, ref DebugInfoModel debugInfoModel)
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            if (debugInfoModel.LastDrawTime is null)
            {
                debugInfoModel.LastDrawTime = DateTime.Now.TimeOfDay;
                debugInfoModel.FPS = 0;
                debugInfoModel.WaitDrawTime = -1;
                returnVal = true;
            }
            else
            {
                debugInfoModel.WaitDrawTime = (DateTime.Now.TimeOfDay - debugInfoModel.LastDrawTime).Value.TotalMilliseconds;
                debugInfoModel.FPS = (uint)(1000 / debugInfoModel.WaitDrawTime);
                returnVal = config.FrameRate >= debugInfoModel.FPS ? true : false;
                if (returnVal)
                {
                    debugInfoModel.LastDrawTime = DateTime.Now.TimeOfDay;
                }
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 内部FPS計算処理
        /// </summary>
        /// <param name="debugInfoModel">デバッグ情報</param>
        public static void CalcInternalFps(ref DebugInfoModel debugInfoModel)
        {
            double returnVal = 0;
            logger.Trace("==============================  Start   ==============================");

            // 処理時間計測タイマー停止
            debugInfoModel.TimerStop();

            try
            {
                returnVal = 1000 / ((double)debugInfoModel.TimerTick / (double)Stopwatch.Frequency);
            }
            catch (Exception ex)
            {
                returnVal = -1;
            }

            debugInfoModel.InternalFPS = returnVal;

            // 処理時間計測タイマー開始
            debugInfoModel.TimerStart();

            logger.Trace("==============================   End    ==============================");
            return;
        }

    }
}
