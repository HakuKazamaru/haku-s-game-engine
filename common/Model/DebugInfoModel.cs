using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakusGameEngine.common.Model
{
    /// <summary>
    /// デバッグ情報管理クラス
    /// </summary>
    public class DebugInfoModel
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 描画フレームレート
        /// </summary>
        public uint FPS { get; set; }

        /// <summary>
        /// 内部フレームレート
        /// </summary>
        public double InternalFPS { get; set; }

        /// <summary>
        /// 内部処理計測タイマー
        /// </summary>
        public Stopwatch Timer { get; set; }

        /// <summary>
        /// 内部処理Tick
        /// </summary>
        public long TimerTick { get; set; }

        /// <summary>
        /// 内部処理時間（ミリ秒）
        /// </summary>
        public long TimerMilliseconds { get; set; }

        /// <summary>
        /// 前回描画処理時間
        /// </summary>
        public TimeSpan? LastDrawTime { get; set; }

        /// <summary>
        /// 前回描画処理時間
        /// </summary>
        public double WaitDrawTime { get; set; }

        /// <summary>
        /// キーボード入力状態
        /// </summary>
        public byte[] KeyboardInputStatusCode { get; set; }

        /// <summary>
        /// ジョイパッド1入力状態(コード)
        /// </summary>
        public int JoyPad1InputStatusCode { get; set; }

        /// <summary>
        /// キー入力情報
        /// </summary>
        public List<KeyBindModel> KeyStatus { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DebugInfoModel()
        {
            KeyStatus = new List<KeyBindModel>();
            KeyboardInputStatusCode = new byte[256];
            Timer = new Stopwatch();
        }

        /// <summary>
        /// 内部処理時間計測開始
        /// </summary>
        public void TimerStart()
        {
            logger.Trace("==============================  Start   ==============================");
            Timer = new Stopwatch();
            Timer.Start();
            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// 内部処理時間計測停止
        /// </summary>
        public void TimerStop()
        {
            logger.Trace("==============================  Start   ==============================");
            Timer.Stop();
            TimerTick = Timer.ElapsedTicks;
            TimerMilliseconds = Timer.ElapsedMilliseconds;
            logger.Trace("==============================   End    ==============================");
        }
    }
}
