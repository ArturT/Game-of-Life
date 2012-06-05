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
        protected HashSet<Cell> _currentState = new HashSet<Cell>();

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
        private int _counterMethod_GiveLifeToNeighboursIfPossible = 0;
        public int CounterMethod_GiveLifeToNeighboursIfPossible { 
            get 
            {
                return _counterMethod_GiveLifeToNeighboursIfPossible;
            }
            private set 
            {
                _counterMethod_GiveLifeToNeighboursIfPossible = value;
            } 
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
