// Artur Trzop (c) 2012
// Coderetreat PK
// Cracow University of Technology, Faculty of Mechanical Engineering

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Structure of cell. Contains coordinates X and Y.
    /// </summary>
    public struct Cell
    {
        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            Cell c = (Cell)obj;

            if (X == c.X && Y == c.Y)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", X, Y).GetHashCode();
        }
    }

    /// <summary>
    /// Board. Place where game happens.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Current state of cells on board.
        /// </summary>
        private HashSet<Cell> _currentState = new HashSet<Cell>();

        /// <summary>
        /// Temporary HashSet used to checking if new cell should be born.
        /// </summary>
        private HashSet<Cell> _tmpCheckedCell = new HashSet<Cell>();

        /// <summary>
        /// Temporary queue used to collect all cells which will be born in next state.
        /// </summary>
        private Queue<Cell> _nextStateGiveLife = new Queue<Cell>();

        /// <summary>
        /// Temporary queue used to collect all cells which will be kill in next state.
        /// </summary>
        private Queue<Cell> _nextStateKillLife = new Queue<Cell>();

        /// <summary>
        /// Temporary cell used to many situations.
        /// </summary>
        private Cell tmpCell = new Cell();

        /// <summary>
        /// Counter of method run at one stage.
        /// </summary>
        public int CounterMethod_GiveLifeToNeighboursIfPossible { get; private set; }

        /// <summary>
        /// Size of board in vertical. Horizontal size is 4 times bigger than AreaSize.
        /// </summary>
        public int AreaSize 
        {
            get;
            set;
        }


        /// <summary>
        /// Constructor. Set default AreaSize = 5.
        /// </summary>
        public Board()
        {
            AreaSize = 5;
            CounterMethod_GiveLifeToNeighboursIfPossible = 0;
        }

        /// <summary>
        /// Add new cell to board if it isn't there.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public void AddCell(int x, int y)
        {
            tmpCell.X = x;
            tmpCell.Y = y;

            // A HashSet is a collection that contains no duplicate elements   
            _currentState.Add(tmpCell);
        }

        /// <summary>
        /// Remove cell from board.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public void RemoveCell(int x, int y)
        {
            tmpCell.X = x;
            tmpCell.Y = y;

            _currentState.Remove(tmpCell);
        }
               
        /// <summary>
        /// Check is cell exist on board.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>bool</returns>
        public bool IsCellExist(int x, int y)
        {
            return _currentState.Contains(new Cell() { X = x, Y = y });
        }

        /// <summary>
        /// Return current amount of cells on board.
        /// </summary>
        /// <returns>int</returns>
        public int CountCells()
        {
            return _currentState.Count;
        }

        /// <summary>
        /// Write on console information about coordinates of all cells.
        /// </summary>
        public void PrintCurrentStateCoordinates()
        {
            Console.WriteLine();

            foreach(Cell cell in _currentState)
            {
                Console.WriteLine("X: " + cell.X + ", Y: " + cell.Y);
            }

            // Why use PadLeft was described in PrintCurrentBoard method.
            Console.WriteLine("Cells on board: " + Convert.ToString(_currentState.Count).PadLeft(10, '0') + "\n");
        }

        
        /// <summary>
        /// Write on console all cells in visual way.
        /// </summary>
        /// <param name="areaSize">Size of board in vertical. Horizontal size is 4 times bigger. If not set then will be use this.AreaSize</param>
        public void PrintCurrentBoard(int? areaSize = null)
        {
            if (areaSize == null)
                areaSize = this.AreaSize;

            Console.WriteLine();

            for (int i = (int)-areaSize; i < areaSize*2; i++)
            {
                for (int j = (int)-areaSize*4; j < areaSize*4; j++)
                {
                    if (IsCellExist(j, i))
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write("-");                    
                    }
                }
                Console.WriteLine();
            }

            // Using PadLeft is helping when we rewrite on the console buffor.
            // (reason of troubles: Console.SetCursorPosition(1, 0) in PlayGame method)
            // PadLeft rescue us from different number of signs displaying on screen.
            Console.WriteLine("Cells on board: " + Convert.ToString(_currentState.Count).PadLeft(10, '0') + "\n");
        }

        /// <summary>
        /// Calculate next state of game and set it as current state.
        /// </summary>
        public void NextState()
        {
            // set to zero every time when we run NextState method
            CounterMethod_GiveLifeToNeighboursIfPossible = 0;

            int tmpNeighbours = 0;
            foreach (Cell cell in _currentState)
            {
                tmpNeighbours = CountNeighbours(cell.X, cell.Y);
                if (tmpNeighbours < 2 || tmpNeighbours > 3)
                    // add cell to kill queue
                    AddCellToNextStateKillLife(cell);
               
                // checking if dead neighbours can be born
                GiveLifeToNeighboursIfPossible(cell.X, cell.Y);

                // clear temp HashSet used in above method
                _tmpCheckedCell.Clear();
            }

            // kill cells
            while (_nextStateKillLife.Count > 0)
            {
                tmpCell = _nextStateKillLife.Dequeue();
                RemoveCell(tmpCell.X, tmpCell.Y);
            }

            // born cells            
            while (_nextStateGiveLife.Count > 0)
            {
                tmpCell = _nextStateGiveLife.Dequeue();
                AddCell(tmpCell.X, tmpCell.Y);
            }
        }

        /// <summary>
        /// Play game in infinity loop
        /// </summary>
        /// <param name="timeSleep">Milliseconds between next refresh of game state.</param>
        /// <param name="displayCurrentCoordinates">Default false</param>
        public void PlayGame(int timeSleep = 100, bool displayCurrentCoordinates = false)
        {
            // board after set all cells
            PrintCurrentBoard();
            
            if (displayCurrentCoordinates)
                PrintCurrentStateCoordinates();

            Thread.Sleep(timeSleep);
            while (true)
            {
                //Console.Clear();
                Console.SetCursorPosition(1, 0);

                NextState();                
                PrintCurrentBoard();
            
                if (displayCurrentCoordinates)
                    PrintCurrentStateCoordinates();
                
                Thread.Sleep(timeSleep);                
            }
        }
        
        /// <summary>
        /// Check if dead neighbours can be born. Only dead cells which had 3 living neighbours can be born.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        private void GiveLifeToNeighboursIfPossible(int x, int y)
        {
            CounterMethod_GiveLifeToNeighboursIfPossible++;

            // 1,1      2,1     3,1
            // 1,2      2,2     3,2
            // 1,3      2,3     3,3            
            int tmpX = 0;
            int tmpY = 0;
            
            for (int i = 0; i < 8; i++)
            {                
                switch(i)
                {
                    case 0:
                        tmpX = x - 1;
                        tmpY = y - 1;
                        break;
                    case 1:
                        tmpX = x;
                        tmpY = y - 1;
                        break;
                    case 2:
                        tmpX = x + 1;
                        tmpY = y - 1;
                        break;
                    case 3:
                        tmpX = x - 1;
                        tmpY = y;
                        break;
                    case 4:
                        tmpX = x + 1;
                        tmpY = y;
                        break;
                    case 5:
                        tmpX = x - 1;
                        tmpY = y + 1;
                        break;
                    case 6:
                        tmpX = x;
                        tmpY = y + 1;
                        break;
                    case 7:
                        tmpX = x + 1;
                        tmpY = y + 1;
                        break;
                    default:
                        throw new Exception("Wrong variable i.");                        
                }
                
                // checking is cell already checked and negative of cell because checking if only dead cell can be born
                if (!_tmpCheckedCell.Contains(new Cell() { X = tmpX, Y = tmpY }) && !IsCellExist(tmpX, tmpY) && CountNeighbours(tmpX, tmpY) == 3)
                { 
                    // born dead cell
                    AddCellToNextStateGiveLife(new Cell() { X = tmpX, Y = tmpY });
                }

                // mark tmpX tmpY as cell that we already checked
                _tmpCheckedCell.Add(new Cell() { X = tmpX, Y = tmpY });
            }
        }

        /// <summary>
        /// Count neighbours of cell.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>int</returns>
        public int CountNeighbours(int x, int y)
        {
            // 1,1      2,1     3,1
            // 1,2      2,2     3,2
            // 1,3      2,3     3,3
            int neighbours = 0;

            if (IsCellExist(x - 1, y - 1))
                neighbours++;

            if (IsCellExist(x, y - 1))
                neighbours++;

            if (IsCellExist(x + 1, y - 1))
                neighbours++;

            if (IsCellExist(x - 1, y))
                neighbours++;

            if (IsCellExist(x + 1, y))
                neighbours++;

            if (IsCellExist(x - 1, y + 1))
                neighbours++;

            if (IsCellExist(x, y + 1))
                neighbours++;

            if (IsCellExist(x + 1, y + 1))
                neighbours++;

            return neighbours;
        }
        
        /// <summary>
        /// Adding cell to cells queue. This cells will be kill in next state of game.
        /// </summary>
        /// <param name="cell">Cell object</param>
        private void AddCellToNextStateKillLife(Cell cell)
        {
            // if not contain cell then add cell to queue
            if (!_nextStateKillLife.Contains(new Cell() { X = cell.X, Y = cell.Y }))
                _nextStateKillLife.Enqueue(cell);
        }

        /// <summary>
        /// Adding cell to cells queue. This cells will be born in next state of game.
        /// </summary>
        /// <param name="cell">Cell object</param>
        private void AddCellToNextStateGiveLife(Cell cell)
        {
            // if not contain cell then add cell to queue
            if (!_nextStateGiveLife.Contains(new Cell() { X = cell.X, Y = cell.Y }))
                _nextStateGiveLife.Enqueue(cell);
        }
    }
}
