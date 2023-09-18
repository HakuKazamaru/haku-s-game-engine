using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DxLibDLL;
using HakusGameEngine.common.Model;

namespace HakusGameEngine.common.Function
{
    /// <summary>
    /// デバッグ関連機能
    /// </summary>
    internal static class DebugFunction
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// デバッグ情報表示機能
        /// </summary>
        /// <param name="debugInfoModel"></param>
        public static void DebugInfoPrint(DebugInfoModel debugInfoModel)
        {
            logger.Trace("==============================  Start   ==============================");

            // 画面クリア
            DX.clsDx();

            DX.putsDx("======================================================================");
            DX.putsDx("・デバッグ情報");

            // FPS表示
            DX.putsDx("======================================================================");
            DX.putsDx(string.Format("ＦＰＳ\t\t\t:[{0}]", debugInfoModel.FPS));
            DX.putsDx(string.Format("内部ＦＰＳ\t\t:[{0}]", debugInfoModel.InternalFPS));
            DX.putsDx(string.Format("内部周波数\t\t:[{0}]", Stopwatch.Frequency));
            DX.putsDx(string.Format("内部ＴＩＣＫ\t\t:[{0}]", debugInfoModel.TimerTick));
            DX.putsDx(string.Format("内部処理時間\t\t:[{0}]", debugInfoModel.TimerMilliseconds));
            DX.putsDx(string.Format("描画待ち時間\t\t:[{0}]", debugInfoModel.WaitDrawTime));
            DX.putsDx(string.Format("前回描画時間\t\t:[{0}]", debugInfoModel.LastDrawTime));

            // 入力情報表示
            DX.putsDx("======================================================================");
            DX.putsDx("入力情報");
            DX.putsDx("----------------------------------------------------------------------");
            DX.putsDx(string.Format("キーボード\t\t：[{0}]:{1}",
                debugInfoModel.KeyboardInputStatusCode,
                InputFunction.KeyBindModelToString(debugInfoModel.KeyStatus[0])));
            DX.putsDx("----------------------------------------------------------------------");
            DX.putsDx(string.Format("ジョイパッド①\t：[{0}]:{1}", 
                debugInfoModel.JoyPad1InputStatusCode, 
                InputFunction.KeyBindModelToString(debugInfoModel.KeyStatus[1])));

            logger.Trace("==============================   End    ==============================");
        }
    }
}
