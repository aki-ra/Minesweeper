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
        /// シフトしたインデックスを返す。
        /// 範囲外の場合は自身を返す。
        /// </summary>
        Func<int, int, int, int> shift = (z, sy, sx) =>
        {
            int y = z / width;
            int x = z % width;
            if((sy + y).Within(0, height - 1) != sy + y) { return z; }  
            if((sx + x).Within(0, width - 1) != sx + x) { return z; }
            return z + sx + sy * width;

        };
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
        public int Width
        {
            get
            {
                return Minesweeper.width;
            }
        }
        public int Height
        {
            get
            {
                return Minesweeper.height;
            }
        }

        #endregion

        #region privateなメソッド

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
                this.map[idx] = 0; //初期化 
                this.map[idx] |= GridInfo.Mine; // 地雷を設置                                  

                this.mineCountMap[shift(idx, -1, -1)]++; // 地雷マスの左上のカウント++ 
                this.mineCountMap[shift(idx, -1,  0)]++; // 地雷マスの上のカウント++ 
                this.mineCountMap[shift(idx, -1,  1)]++; // 地雷マスの右上のカウント++ 
                this.mineCountMap[shift(idx,  0, -1)]++; // 地雷マスの左のカウント++ 
                this.mineCountMap[shift(idx,  0,  1)]++; // 地雷マスの右のカウント++ 
                this.mineCountMap[shift(idx,  1, -1)]++; // 地雷マスの左下のカウント++ 
                this.mineCountMap[shift(idx,  1,  0)]++; // 地雷マスの下のカウント++ 
                this.mineCountMap[shift(idx,  1,  1)]++; // 地雷マスの右下のカウント++                                       
            }
        }

        private void OpenCell(int idx)
        {                             
            #region 開けられないマスチェック
            if (this.map[idx].HasFlag(GridInfo.Mine)) return;
            if (this.map[idx].HasFlag(GridInfo.Clear)) return;
            if (this.map[idx].HasFlag(GridInfo.Flag)) return;
            #endregion

            this.map[idx] |= GridInfo.Clear;

            #region 周辺8マスにひとつも地雷がなければ再帰的に開けていく
            if(this.mineCountMap[idx] == 0)
            {
                OpenCell(shift(idx, -1, -1));
                OpenCell(shift(idx, -1,  0));
                OpenCell(shift(idx, -1,  1));
                OpenCell(shift(idx,  0, -1));
                OpenCell(shift(idx,  0,  1));
                OpenCell(shift(idx,  1, -1));
                OpenCell(shift(idx,  1,  0));
                OpenCell(shift(idx,  1,  1));
            }
            #endregion
        }

        private void GameOverCheck()
        {
            // 間違ったFlagあったらクリアしていない
            if (this.map.Where(x =>
                 x.HasFlag(GridInfo.Flag) &&
                 !x.HasFlag(GridInfo.Mine)
                ).Count() > 0) return;

            // 地雷の数
            int mine = this.map                 
                .Where(x =>x.HasFlag(GridInfo.Mine)).Count();
            // 開いたマスの数
            var clear = this.map
                .Where(x => x.HasFlag(GridInfo.Clear)).Count();
            // 開いたマス+地雷の数 = 全マスならゲームクリア
            if(mine + clear == this.map.Count())
            {
                state |= GameState.GameOver;
            }

            return;
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
            #region ゲーム状態チェック
            if (state.HasFlag(GameState.Dead)) throw new InvalidOperationException();
            if (state.HasFlag(GameState.GameOver)) throw new InvalidOperationException();
            #endregion

            var score = Check(y, x);
            if(score == (int)Score.None)
            {
                return score;
            }
            if (score == (int)Score.Dead)
            {
                state |= GameState.Dead;             // ステータス更新
                this.map[f(y, x)] |= GridInfo.Clear; // マスを開く
                return score;
            }
            if(score == (int)Score.Clear)
            {
                OpenCell(f(y, x));
                GameOverCheck();
                return score;
            }
            
            return 0;
        }

        public int Check(int y, int x)
        {
            int idx = f(y, x);
            if (this.map[idx].HasFlag(GridInfo.Clear)) return (int)Score.None;// すでに開いている
            else if (this.map[idx].HasFlag(GridInfo.Mine)) return (int)Score.Dead;// 地雷を踏みぬいた
            else return (int)Score.Clear; // 開いていないクリーンなマス。
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

            Minesweeper.width = w;
            Minesweeper.height = h;
        }      

        public void RaiseFlag(int y, int x)
        {      
            #region ゲーム状態チェック
            if (state.HasFlag(GameState.Dead)) throw new InvalidOperationException();
            if (state.HasFlag(GameState.GameOver)) throw new InvalidOperationException();
            #endregion

            var idx = f(y, x);

            // 空いている場合何もしない
            if (this.map[idx].HasFlag(GridInfo.Clear)) return;
            // 旗があれば解除し、なければつける。
            if (this.map[idx].HasFlag(GridInfo.Flag))
            {
                this.map[idx] = this.map[idx] & ~GridInfo.Flag;
            }
            this.map[idx] |= GridInfo.Flag;
            GameOverCheck();
            return;
        }

        public int GetGridData(int y, int x)
        {
            var cell = this.map[f(y, x)];
            if (cell.HasFlag(GridInfo.Flag)) return (int)VisibleState.Flag;
            if (cell.HasFlag(GridInfo.Mine) && cell.HasFlag(GridInfo.Clear))
            {
                return (int)VisibleState.Mine;
            }
            if (!cell.HasFlag(GridInfo.Clear)) return (int)VisibleState.Plain;
            //else
            return this.mineCountMap[f(y, x)];
        }

        public List<int> GetGridData(List<int> ys, List<int> xs)
        {
            var ret = new List<int>();
            foreach(var item in ys.Zip(xs, (y, x) => new { Y = y, X = x }))
            {
                ret.Add(GetGridData(item.Y, item.X));
            }
            return ret;
        }

        public List<List<int>> GetAllGridData2d()
        {                     
            var map = new List<List<int>>();                      
            foreach (var x in Enumerable.Range(0, this.Width))
            {
                var row = new List<int>();
                foreach (var y in Enumerable.Range(0, this.Height))
                {
                    row.Add(GetGridData(y, x));   
                }
                map.Add(row);
            }
            return map;
                             
        }
        #endregion
    }
 
           
}
