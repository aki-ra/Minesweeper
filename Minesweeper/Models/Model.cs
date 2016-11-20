using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using Minesweeper.Core;

namespace Minesweeper.Models
{
    public class Model : NotificationObject, IMinesweeper
    {           
        private Core.Minesweeper ms;   

        public Model()
        {
            if (ms == null) ms = new Core.Minesweeper();
        }

        public int Height
        {
            get
            {
                return ((IMinesweeper)ms).Height;
            }
        }

        public GameState State
        {
            get
            {
                return ((IMinesweeper)ms).State;
            }
        }

        public int Width
        {
            get
            {
                return ((IMinesweeper)ms).Width;
            }
        }

        public int Challenge(int y, int x)
        {
            var ret = ((IMinesweeper)ms).Challenge(y, x);
            RaisePropertyChanged("State");
            RaisePropertyChanged("Grid");
            return ret;
        }

        public int Check(int y, int x)
        {
            return ((IMinesweeper)ms).Check(y, x);
        }

        public List<List<int>> GetAllGridData2d()
        {
            return ((IMinesweeper)ms).GetAllGridData2d();
        }

        public List<int> GetGridData(List<int> ys, List<int> xs)
        {
            return ((IMinesweeper)ms).GetGridData(ys, xs);
        }

        public int GetGridData(int y, int x)
        {
            return ((IMinesweeper)ms).GetGridData(y, x);
        }

        public void RaiseFlag(int y, int x)
        {
            ((IMinesweeper)ms).RaiseFlag(y, x);
            RaisePropertyChanged("State");
            RaisePropertyChanged("Grid");
        }

        public void Start(int height, int width, int mines, int seed)
        {
            ((IMinesweeper)ms).Start(height, width, mines, seed);
            RaisePropertyChanged("State");
            RaisePropertyChanged("Grid");
        }

        
    }
}
