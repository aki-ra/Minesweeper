using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public class Minesweeper : IMinesweeper
    {
        private GameState state;

        public GameState State
        {
            get
            {
                return this.state;
            }
        }

        public int Challenge(int y, int x)
        {
            return 0;
        }

        public int Check(int y, int x)
        {
            return 0;
        }

        public void Start(int y, int x, int mine)
        {
            return;
        }

        public void RaiseFlag(int y, int x)
        {
            return;
        }
    }
}
