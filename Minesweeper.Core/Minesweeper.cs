using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace Minesweeper.Core
{
    public class Minesweeper : IMinesweeper
    {
        #region privateなメンバ

        private GameState state; // ゲーム状態
        static int width = Settings.MinWidth; // 列数
        static int height = Settings.MinHeight; // 行数
        GridInfo[] map; //マップ
        int[] mineCountMap; //周辺の地雷数マップ       

        #region デリゲート

        #region flatten
        /// <summary>
        /// (X,Y) => mapのindex
        /// </summary>
        Func<int, int, int> f = (int y, int x) => y * width + x;
        #endregion

        #region 隣接マスのインデックス取得
        /// <summary>
        /// ひとつ左のインデックスを返す。
        /// 範囲外の場合は自身を返す。
        /// </summary>
        Func<int, int> l = (z =>
        {
            int x = z % width;
            if (x > 0) return z - 1;
            else return z;
        });
        /// <summary>
        /// ひとつ右のインデックスを返す。
        /// 範囲外の場合は自身を返す。
        /// </summary>
        Func<int, int> r = (z =>
        {
            int x = z % width;
            if (x < width - 1) return z + 1;
            else return z;
        });
        /// <summary>
        /// ひとつ上のインデックスを返す。
        /// 範囲外の場合は自身を返す。
        /// </summary>
        Func<int, int> u = (z =>
        {
            int y = z / width;
            if (y > 0) return z - width;
            else return z;
        });
        /// <summary>
        /// ひとつ下のインデックスを返す。
        /// 範囲外の場合は自身を返す。
        /// </summary>
        Func<int, int> b = (z =>
        {
            int y = z / width;
            if (y < height - 1) return z + width;
            else return z;
        });
        #endregion
                         
        #endregion

        #region フィールド

        public GameState State
        {
            get
            {
                return this.state;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 地雷マップ初期化
        /// </summary>
        /// <param name="h">height</param>
        /// <param name="w">width</param>
        /// <param name="m">num of mines</param>
        /// <param name="s">seed</param>
        private void InitMap(int h, int w, int m, int s)
        {
            this.map = new GridInfo[h * w];
            this.mineCountMap = new int[h * w];
            foreach (var idx in Common.UniqueRandomInt(m, 0, h * w, s))
            {
                this.map[idx] |= GridInfo.Mine; // 地雷を設置
                this.mineCountMap[u(l(idx))]++; // 地雷マスの左上のカウント++ 
                this.mineCountMap[u(idx)]++; // 地雷マスの上のカウント++ 
                this.mineCountMap[u(r(idx))]++; // 地雷マスの右上のカウント++ 
                this.mineCountMap[l(idx)]++; // 地雷マスの左のカウント++ 
                this.mineCountMap[r(idx)]++; // 地雷マスの右のカウント++ 
                this.mineCountMap[b(l(idx))]++; // 地雷マスの左下のカウント++ 
                this.mineCountMap[b(idx)]++; // 地雷マスの下のカウント++ 
                this.mineCountMap[b(l(idx))]++; // 地雷マスの右下のカウント++ 
                this.mineCountMap[idx] = -1; // 地雷マスのカウントリセット(上の処理の副作用でインクリメントされているので。) 
            }
        }

        #endregion

        #endregion

        #region publicなメソッド

        /// <summary>
        /// コンストラクタ。Start呼び出すだけ。
        /// </summary>
        public Minesweeper()
        {
            Start(Settings.MinHeight, Settings.MinWidth, Settings.MinWidth);
        }
        public int Challenge(int y, int x)
        {
            return 0;
        }

        public int Check(int y, int x)
        {
            return 0;
        }

        /// <summary>
        /// マップを初期化する。
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="mine"></param>
        public void Start(int height, int width, int mine, int seed = -1)
        {
            int h, w, m;
            #region 引数チェック                            
            w = width.Within(Settings.MinWidth, Settings.MaxWidth);
            h = height.Within(Settings.MinHeight, Settings.MaxHeight);
            m = mine.Within(Settings.MinMines, Settings.MaxWidth * Settings.MaxHeight - 1);
            #endregion

            InitMap(h, w, m, seed); //マップを再設定

            this.state = GameState.Alive; // ゲーム状態を生存にセット

            return;
        }      

        public void RaiseFlag(int y, int x)
        {
            return;
        }

        #endregion
    }
 
           
}
