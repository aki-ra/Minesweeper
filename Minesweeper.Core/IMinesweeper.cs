using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    /// <summary>
    /// Minesweeperのインターフェイス定義。
    /// publicなメンバはここですべて定義する。
    /// </summary>
    interface IMinesweeper
    {
        #region フィールド

        /// <summary>
        /// 現在のゲームの状態を返す
        /// </summary>
        GameState State { get; }

        #endregion

        #region メソッド

        /// <summary>
        /// マスを開ける
        /// </summary>     
        /// <param name="y">y座標(行)</param>
        /// <param name="x">x座標(列)</param>
        /// <returns>
        /// 開けることが出来たマスの数。(1～)
        /// -1 : 失敗(→GameStateがDeadになる)
        /// </returns>
        int Challenge(int y, int x);

        /// <summary>
        /// マスを開けた場合、何マス開放できるか調べる。
        /// 実際にマスは開けない。(チート・学習用)
        /// </summary>          
        /// <param name="y">y座標(行)</param>
        /// <param name="x">x座標(列)</param>
        /// <returns>
        /// 開けることが出来るマスの数。(1～)
        /// -1 : 失敗
        /// ※ GameStateは変わらない。
        /// </returns>
        int Check(int y, int x);

        /// <summary>
        /// 盤面をリセットする。
        /// </summary>
        /// <param name="height">縦のマス数。Settings.MinWidth以上Settings.MaxWidth以下</param>
        /// <param name="width">横のマス数。Settings.MinHeight以上Settings.MaxHeight以下</param>
        /// <param name="mines">地雷の数。マイナスまたは総マス数以上の場合、総マス数-1に勝手に修正します。</param>
        /// <param name="seed">地雷設置のシード値。省略するとシステム時間を使う</param>
        void Start(int height, int width, int mines, int seed);

        /// <summary>
        /// 旗の上げ下げ
        /// Todo: 下げれるのにRaiseはおかしいだろ。名称要検討
        /// </summary>
        /// <param name="y">y座標(行)</param>
        /// <param name="x">x座標(列)</param>
        void RaiseFlag(int y, int x);

        #endregion
    }


}
