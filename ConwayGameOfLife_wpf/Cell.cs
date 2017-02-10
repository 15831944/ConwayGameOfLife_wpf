using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife_wpf
{
    public enum CellState { DEAD, ALIVE, NULL }

    public class Cell
    {

        private int X { get; set; }
        private int Y { get; set; }
        private CellState State {get; set;}

        Cell(int x, int y)
        {
            X = x;
            Y = y;
            State = CellState.ALIVE;
        }

    }
}
