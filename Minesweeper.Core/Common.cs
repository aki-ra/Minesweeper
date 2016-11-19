using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public static class Common
    {
        /// <summary>
        /// 自身が 引数の数値の範囲内にあるかをチェックする。
        /// 範囲外の場合は、範囲内に丸めた値を返す。
        /// </summary>
        /// <param name="me">自身</param>
        /// <param name="min">最小。別に最大でもいい</param>
        /// <param name="max">>最大。別に最小でもいい</param>
        /// <returns></returns>
        public static int Within(this int me, int min, int max)
        {
            if(min > max) return Math.Min(min, Math.Max(max, me));
            else return Math.Min(max, Math.Max(min, me));
        }

        /// <summary>
        /// 重複しない(ユニークな)乱数のリストを返す。
        /// 数に対して乱数の候補範囲が小さいと多分絶望的に遅い。
        /// </summary>
        /// <param name="num">乱数の数</param>
        /// <param name="min">乱数の最小値</param>
        /// <param name="max">乱数の最大値</param>
        /// <param name="seed">シード値(省略可)</param>
        /// <returns>乱数のリスト</returns>
        public static List<int> UniqueRandomInt(int num, int min, int max, int seed = -1)
        {
            if (max - min + 1 <= num) throw new ArgumentException(); //uniqueなのを返せない条件なら例外を出す

            var ret = new List<int>();
            System.Random r = new System.Random(seed);
            if (seed == -1) r = new System.Random(); //seed指定なしなら時間から作る。

            while (ret.Count < num)
            {
                int rnd = r.Next(min, max);
                if (ret.Contains(rnd)) continue;
                ret.Add(rnd);
            }
            return ret;
        }
    }
}
