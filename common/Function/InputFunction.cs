using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DxLibDLL;

using HakusGameEngine.common.Model;

namespace HakusGameEngine.common.Function
{
    /// <summary>
    /// 入力処理クラス
    /// </summary>
    internal static class InputFunction
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// キーボード、ジョイパッドの入力状態を取得する
        /// </summary>
        /// <param name="config">設定データ</param>
        /// <param name="debug">デバッグ情報</param>
        /// <param name="debug">デバッグモード</param>
        /// <returns>キー入力状態</returns>
        public static List<KeyBindModel> GetInputKeyAllCode(SettingModel config, ref DebugInfoModel debugInfoModel, bool debug = false)
        {
            List<KeyBindModel> returnVal = new List<KeyBindModel>();
            logger.Trace("==============================  Start   ==============================");

            // キーボード
            returnVal.Add(GetInputKeyAllCodeByKeyboard(config, ref debugInfoModel, debug));

            // ジョイパッド1
            returnVal.Add(GetInputKeyAllCodeByJoyPad1(config, ref debugInfoModel, debug));

            debugInfoModel.KeyStatus = returnVal;

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// キーボードの入力状態を取得する
        /// </summary>
        /// <param name="config">設定データ</param>
        /// <param name="debug">デバッグ情報</param>
        /// <param name="debug">デバッグ</param>
        /// <returns>キー入力状態</returns>
        public static KeyBindModel GetInputKeyAllCodeByKeyboard(SettingModel config, ref DebugInfoModel debugInfoModel, bool debug = false)
        {
            byte[] keyCode = new byte[256];
            KeyBindModel returnVal = new KeyBindModel();
            logger.Trace("==============================  Start   ==============================");

            DX.GetHitKeyStateAll(keyCode);
            debugInfoModel.KeyboardInputStatusCode = keyCode;

            // 十字キー上
            if (keyCode[config.KeyboardBind.DPadUpKey] != 0)
            {
                returnVal.DPadUpKey = 1;
            }
            else
            {
                returnVal.DPadUpKey = 0;
            }

            // 十字キー下
            if (keyCode[config.KeyboardBind.DPadDownKey] != 0)
            {
                returnVal.DPadDownKey = 1;
            }
            else
            {
                returnVal.DPadDownKey = 0;
            }

            // 十字キー左
            if (keyCode[config.KeyboardBind.DPadLeftKey] != 0)
            {
                returnVal.DPadLeftKey = 1;
            }
            else
            {
                returnVal.DPadLeftKey = 0;
            }

            // 十字キー右
            if (keyCode[config.KeyboardBind.DPadRightKey] != 0)
            {
                returnVal.DPadRightKey = 1;
            }
            else
            {
                returnVal.DPadRightKey = 0;
            }

            // ボタン1
            if (keyCode[config.KeyboardBind.Button1] != 0)
            {
                returnVal.Button1 = 1;
            }
            else
            {
                returnVal.Button1 = 0;
            }

            // ボタン2
            if (keyCode[config.KeyboardBind.Button2] != 0)
            {
                returnVal.Button2 = 1;
            }
            else
            {
                returnVal.Button2 = 0;
            }

            // ボタン3
            if (keyCode[config.KeyboardBind.Button3] != 0)
            {
                returnVal.Button3 = 1;
            }
            else
            {
                returnVal.Button3 = 0;
            }

            // ボタン4
            if (keyCode[config.KeyboardBind.Button4] != 0)
            {
                returnVal.Button4 = 1;
            }
            else
            {
                returnVal.Button4 = 0;
            }

            // スタートボタン
            if (keyCode[config.KeyboardBind.StartKey] != 0)
            {
                returnVal.StartKey = 1;
            }
            else
            {
                returnVal.StartKey = 0;
            }

            // セレクトボタン
            if (keyCode[config.KeyboardBind.SelectKey] != 0)
            {
                returnVal.SelectKey = 1;
            }
            else
            {
                returnVal.SelectKey = 0;
            }

            // Lボタン1
            if (keyCode[config.KeyboardBind.LeftKey1] != 0)
            {
                returnVal.LeftKey1 = 1;
            }
            else
            {
                returnVal.LeftKey1 = 0;
            }

            // Lボタン2
            if (keyCode[config.KeyboardBind.LeftKey2] != 0)
            {
                returnVal.LeftKey2 = 1;
            }
            else
            {
                returnVal.LeftKey2 = 0;
            }

            // Lボタン3
            if (keyCode[config.KeyboardBind.LeftKey3] != 0)
            {
                returnVal.LeftKey3 = 1;
            }
            else
            {
                returnVal.LeftKey3 = 0;
            }

            // Rボタン1
            if (keyCode[config.KeyboardBind.RightKey1] != 0)
            {
                returnVal.RightKey1 = 1;
            }
            else
            {
                returnVal.RightKey1 = 0;
            }

            // Rボタン2
            if (keyCode[config.KeyboardBind.RightKey2] != 0)
            {
                returnVal.RightKey2 = 1;
            }
            else
            {
                returnVal.RightKey2 = 0;
            }

            // Rボタン3
            if (keyCode[config.KeyboardBind.RightKey3] != 0)
            {
                returnVal.RightKey3 = 1;
            }
            else
            {
                returnVal.RightKey3 = 0;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ジョイパッド（1P）の入力状態を取得する
        /// </summary>
        /// <param name="config">設定データ</param>
        /// <param name="debug">デバッグ情報</param>
        /// <param name="debug">デバッグ</param>
        /// <returns>キー入力状態</returns>
        public static KeyBindModel GetInputKeyAllCodeByJoyPad1(SettingModel config, ref DebugInfoModel debugInfoModel, bool debug = false)
        {
            int intKeyCode = 0;
            KeyBindModel returnVal = new KeyBindModel();
            logger.Trace("==============================  Start   ==============================");

            intKeyCode = DX.GetJoypadInputState(DX.DX_INPUT_PAD1);
            debugInfoModel.JoyPad1InputStatusCode = intKeyCode;

            // 十字キー上
            if ((intKeyCode & config.JoyPadBind.DPadUpKey) != 0)
            {
                returnVal.DPadUpKey = 1;
            }
            else
            {
                returnVal.DPadUpKey = 0;
            }

            // 十字キー下
            if ((intKeyCode & config.JoyPadBind.DPadDownKey) != 0)
            {
                returnVal.DPadDownKey = 1;
            }
            else
            {
                returnVal.DPadDownKey = 0;
            }

            // 十字キー左
            if ((intKeyCode & config.JoyPadBind.DPadLeftKey) != 0)
            {
                returnVal.DPadLeftKey = 1;
            }
            else
            {
                returnVal.DPadLeftKey = 0;
            }

            // 十字キー右
            if ((intKeyCode & config.JoyPadBind.DPadRightKey) != 0)
            {
                returnVal.DPadRightKey = 1;
            }
            else
            {
                returnVal.DPadRightKey = 0;
            }

            // ボタン1
            if ((intKeyCode & config.JoyPadBind.Button1) != 0)
            {
                returnVal.Button1 = 1;
            }
            else
            {
                returnVal.Button1 = 0;
            }

            // ボタン2
            if ((intKeyCode & config.JoyPadBind.Button2) != 0)
            {
                returnVal.Button2 = 1;
            }
            else
            {
                returnVal.Button2 = 0;
            }

            // ボタン3
            if ((intKeyCode & config.JoyPadBind.Button3) != 0)
            {
                returnVal.Button3 = 1;
            }
            else
            {
                returnVal.Button3 = 0;
            }

            // ボタン4
            if ((intKeyCode & config.JoyPadBind.Button4) != 0)
            {
                returnVal.Button4 = 1;
            }
            else
            {
                returnVal.Button4 = 0;
            }

            // スタートボタン
            if ((intKeyCode & config.JoyPadBind.StartKey) != 0)
            {
                returnVal.StartKey = 1;
            }
            else
            {
                returnVal.StartKey = 0;
            }

            // セレクトボタン
            if ((intKeyCode & config.JoyPadBind.SelectKey) != 0)
            {
                returnVal.SelectKey = 1;
            }
            else
            {
                returnVal.SelectKey = 0;
            }

            // Lボタン1
            if ((intKeyCode & config.JoyPadBind.LeftKey1) != 0)
            {
                returnVal.LeftKey1 = 1;
            }
            else
            {
                returnVal.LeftKey1 = 0;
            }

            // Lボタン2
            if ((intKeyCode & config.JoyPadBind.LeftKey2) != 0)
            {
                returnVal.LeftKey2 = 1;
            }
            else
            {
                returnVal.LeftKey2 = 0;
            }

            // Lボタン3
            if ((intKeyCode & config.JoyPadBind.LeftKey3) != 0)
            {
                returnVal.LeftKey3 = 1;
            }
            else
            {
                returnVal.LeftKey3 = 0;
            }

            // Rボタン1
            if ((intKeyCode & config.JoyPadBind.RightKey1) != 0)
            {
                returnVal.RightKey1 = 1;
            }
            else
            {
                returnVal.RightKey1 = 0;
            }

            // Rボタン2
            if ((intKeyCode & config.JoyPadBind.RightKey2) != 0)
            {
                returnVal.RightKey2 = 1;
            }
            else
            {
                returnVal.RightKey2 = 0;
            }

            // Rボタン3
            if ((intKeyCode & config.JoyPadBind.RightKey3) != 0)
            {
                returnVal.RightKey3 = 1;
            }
            else
            {
                returnVal.RightKey3 = 0;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// キーバインド情報の文字列化（入力状態）
        /// </summary>
        /// <param name="keyBindModel"></param>
        /// <returns></returns>
        public static string KeyBindModelToString(KeyBindModel keyBindModel)
        {
            string returnVal = "";
            logger.Trace("==============================  Start   ==============================");

            if (keyBindModel.DPadUpKey != 0)
            {
                returnVal += "↑";
            }

            if (keyBindModel.DPadDownKey != 0)
            {
                returnVal += "↓";
            }

            if (keyBindModel.DPadLeftKey != 0)
            {
                returnVal += "←";
            }

            if (keyBindModel.DPadRightKey != 0)
            {
                returnVal += "→";
            }

            if (keyBindModel.Button1 != 0)
            {
                returnVal += "B1";
            }

            if (keyBindModel.Button2 != 0)
            {
                returnVal += "B2";
            }

            if (keyBindModel.Button3 != 0)
            {
                returnVal += "B3";
            }

            if (keyBindModel.Button4 != 0)
            {
                returnVal += "B4";
            }

            if (keyBindModel.StartKey != 0)
            {
                returnVal += "▶";
            }

            if (keyBindModel.SelectKey != 0)
            {
                returnVal += "■";
            }

            if (keyBindModel.LeftKey1 != 0)
            {
                returnVal += "L1";
            }

            if (keyBindModel.LeftKey2 != 0)
            {
                returnVal += "L2";
            }

            if (keyBindModel.LeftKey3 != 0)
            {
                returnVal += "L3";
            }

            if (keyBindModel.RightKey1 != 0)
            {
                returnVal += "R1";
            }

            if (keyBindModel.RightKey2 != 0)
            {
                returnVal += "R2";
            }

            if (keyBindModel.RightKey3 != 0)
            {
                returnVal += "R3";
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

    }
}
