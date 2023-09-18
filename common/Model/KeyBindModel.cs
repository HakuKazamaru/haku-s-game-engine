using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakusGameEngine.common.Model
{
    /// <summary>
    /// キー割り当て管理クラス
    /// </summary>
    public class KeyBindModel
    {
        /// <summary>
        /// 十字キー：上
        /// </summary>
        public int DPadUpKey { get; set; }
        
        /// <summary>
        /// 十字キー：下
        /// </summary>
        public int DPadDownKey { get; set; }
        
        /// <summary>
        /// 十字キー：左
        /// </summary>
        public int DPadLeftKey { get; set; }
        
        /// <summary>
        /// 十字キー：右
        /// </summary>
        public int DPadRightKey { get; set; }

        /// <summary>
        /// ボタン1（決定）
        /// </summary>
        public int Button1 { get; set; }

        /// <summary>
        /// ボタン2（キャンセル）
        /// </summary>
        public int Button2 { get; set; }

        /// <summary>
        /// ボタン3
        /// </summary>
        public int Button3 { get; set; }

        /// <summary>
        /// ボタン4
        /// </summary>
        public int Button4 { get; set; }

        /// <summary>
        /// スタートボタン
        /// </summary>
        public int StartKey { get; set; }

        /// <summary>
        /// セレクトボタン
        /// </summary>
        public int SelectKey { get; set; }

        /// <summary>
        /// Lボタン1
        /// </summary>
        public int LeftKey1 { get; set; }
        
        /// <summary>
        /// Lボタン2
        /// </summary>
        public int LeftKey2 { get; set; }
        
        /// <summary>
        /// Lボタン3
        /// </summary>
        public int LeftKey3 { get; set; }

        /// <summary>
        /// Rボタン1
        /// </summary>
        public int RightKey1 { get; set; }

        /// <summary>
        /// Rボタン2
        /// </summary>
        public int RightKey2 { get; set; }

        /// <summary>
        /// Rボタン3
        /// </summary>
        public int RightKey3 { get; set; }

    }
}
