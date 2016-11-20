using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    /// <summary>
    /// ゲームの進行状態
    /// </summary>
    public enum GameState
    {                 
        Dead,
        GameOver,
    }

    /// <summary>
    /// マスの状態
    /// </summary>
    [Flags]
    public enum GridInfo
    {                
        None　=0x00,  // 未初期化       
        Flag = 0x01,  // 旗を立てたマス
        Clear = 0x02, // 踏破済みのマス
        Mine = 0x04,  // 地雷マス         
    }

    public enum Score
    {
        None = 0,
        Dead = -10,
        Clear = 1,     
    }

    /// <summary>
    /// 設置値
    /// </summary>
    public static class Settings
    {
        public const int MinHeight = 4;  //マップの最小行数
        public const int MaxHeight = 50;  //マップの最小行数
        public const int MinWidth = 4;  //マップの最小列数
        public const int MaxWidth = 50; //マップの最大列数
        public const int MinMines = 1; //マップの最小地雷数
    }

    public enum VisibleState
    {
        Plain = -1,
        Flag = -2,
        Mine = -3
    }
}
