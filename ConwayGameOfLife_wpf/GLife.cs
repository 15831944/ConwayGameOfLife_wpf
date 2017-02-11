using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife_wpf
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class GLife
    {
        /// <summary>
        /// Creates a random initial instance of cells
        /// </summary>
        /// <param name="gridSize">Size of grid</param>
        /// <param name="probability">Probability of a cell being alive or dead</param>
        /// <returns></returns>
        public static Dictionary<Coordinates, CellState> GenerateLife(int gridSize, double probability = 0.6)
        {
            Random rand = new Random();
            Dictionary<Coordinates, CellState> dic = new Dictionary<Coordinates, CellState>();
            for(int col = 0; col <= gridSize; col++ )
            {
                for(int row = 0; row <= gridSize; row++)
                {
                    Coordinates coord = new Coordinates(col, row);
                    bool deadOrAlive = rand.NextDouble() < probability; //Generate a random dead or alive cell. 
                    dic.Add(coord, deadOrAlive? CellState.ALIVE : CellState.DEAD);
                }
            }
            return dic;
        }


        public static Dictionary<Coordinates, CellState> NextGen(int gridSize, Dictionary<Coordinates, CellState> dic)
        {
            Dictionary<Coordinates, CellState> newDic = new Dictionary<Coordinates, CellState>();
            for (int col = 0; col <= gridSize; col++)
            {
                for (int row = 0; row <= gridSize; row++)
                {
                    Coordinates coord = new Coordinates(col, row);
                    CellState currentState = CellState.NULL;
                    foreach (KeyValuePair<Coordinates, CellState> cell in dic)
                    {
                        Coordinates cellCoord = cell.Key;
                        if(cellCoord.X == coord.X && cellCoord.Y == coord.Y)
                        {
                            currentState = cell.Value; //get cell value
                            break;
                        }
                    }
                    CellState newState = SetState(dic, coord, currentState, gridSize);
                    newDic.Add(coord, newState);                   
                }
            }
            return newDic;
        }


        /// <summary>
        /// Sets the next gens state of the cell based on its neighbour's state as of Conways Game of Life rules.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="currentCoordinates"></param>
        /// <param name="currentState"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        private static CellState SetState(Dictionary<Coordinates, CellState> dic, Coordinates currentCoordinates, CellState currentState, int gridSize)
        {
            int neighbours = 0;
            for(int i = 1; i <= 8; i++) //rotational check.
            {
                int x = currentCoordinates.X;
                int y = currentCoordinates.Y;
                //check state clockwise starting from immediate left of cell
                GetXnY(i, ref x, ref y);
                CellState neighbourState = CellState.NULL; //schrodinger
                Coordinates coord = new Coordinates(x, y);
                if (coord.X > -1 && coord.Y > -1 && coord.X < gridSize + 1 && coord.Y < gridSize + 1) //Don't bother checking cells out of bounds
                {
                    foreach (KeyValuePair<Coordinates, CellState> cell in dic)
                    {
                        Coordinates cellCoord = cell.Key;
                        if (cellCoord.X == coord.X && cellCoord.Y == coord.Y)
                        {
                            neighbourState = cell.Value;
                            break; //Save time. Next items are redundant
                        }
                    }
                }
                if (neighbourState == CellState.ALIVE)
                {
                    neighbours++;
                }
            }
            if (currentState == CellState.DEAD && neighbours == 3)
            {
                //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction
                return CellState.ALIVE;
            }
            else if (neighbours < 2)
            {
                //Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
                return CellState.DEAD;
            }
            else if(neighbours == 3 || neighbours == 2 )
            {
                //Any live cell with two or three live neighbours lives on to the next generation.
                if (currentState == CellState.ALIVE)
                {
                    return CellState.ALIVE;
                }
                else
                {
                    return CellState.DEAD;
                }
            }
            else
            {
                //Any live cell with more than three live neighbours dies, as if by overpopulation.
                return CellState.DEAD;
            }
        }

        /// <summary>
        /// LOL Will write a better algorithim later.
        /// Gets the coordinates of the adjacent cells from the input coordinates. 
        /// Direction : Clockwise from immediate left
        /// </summary>
        private static void GetXnY(int i, ref int x, ref int y)
        {
            //Please don't laugh
            switch(i)
            {
                case 1:
                    {
                        x -=1;
                        break;
                    }
                case 2:
                    {
                        x -= 1;
                        y -= 1;
                        break;
                    }
                case 3:
                    {
                        y -= 1;
                        break;
                    }
                case 4:
                    {
                        x += 1;
                        y -= 1;
                        break;
                    }
                case 5:
                    {
                        x += 1;
                        break;
                    }
                case 6:
                    {
                        x += 1;
                        y += 1;
                        break;
                    }
                case 7:
                    {
                        y += 1;
                        break;
                    }
                case 8:
                    {
                        x -= 1;
                        y += 1;
                        break;
                    }
            }
        }


        private static bool GetState(Dictionary<Coordinates, CellState> dic, Coordinates coord)
        {
            return dic.ContainsKey(coord);
        }
    }
}
