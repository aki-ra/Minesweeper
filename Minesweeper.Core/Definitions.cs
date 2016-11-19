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
        Alive,
        Dead,
        Clear,
    }

    /// <summary>
    /// マスの状態
    /// </summary>
    [Flags]
    public enum GridInfo
    {                       
        Flag,  // 旗を立てたマス
        Clear, // 踏破済みのマス
        Mine,  // 地雷マス         
    }
}
