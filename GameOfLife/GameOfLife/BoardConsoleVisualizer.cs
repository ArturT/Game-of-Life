using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    class BoardConsoleVisualizer : Board, IBoardVisualizer
    {
        /// <summary>
        /// Size of board in vertical. Horizontal size is 4 times bigger than AreaSize.
        /// </summary>
        public int AreaSize { get; set; }

        /// <summary>
        /// Milliseconds between next refresh of game state.
        /// </summary>
        public int TimeSleep;

        /// <summary>
        /// If true then write on console information about coordinates of all cells after each state of loop in PlayGame method
        /// </summary>
        public bool DisplayCurrentCoordinates;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="areaSize">Size of board in vertical. Horizontal size is 4 times bigger. If not set then will be use this.AreaSize</param>
        /// <param name="timeSleep">Milliseconds between next refresh of game state.</param>
        /// <param name="displayCurrentCoordinates">Default false</param>
        public BoardConsoleVisualizer(int? areaSize = null, int timeSleep = 250, bool displayCurrentCoordinates = false) 
        {
            if(areaSize == null)
                AreaSize = 5;  
            else
                AreaSize = (int)areaSize;

            if (timeSleep > 0)
                TimeSleep = timeSleep;
            else
                throw new ArgumentException("timeSleep must be bigger then zero.");

            DisplayCurrentCoordinates = displayCurrentCoordinates;
        }

        /// <summary>
        /// Play game in infinity loop
        /// </summary>       
        public void PlayGame()
        {
            // board after set all cells
            DisplayCurrentStateOfBoard();

            if (DisplayCurrentCoordinates)
                PrintCurrentStateCoordinates();

            Thread.Sleep(TimeSleep);
            while (true)
            {
                //Console.Clear();
                Console.SetCursorPosition(1, 0);

                NextState();
                DisplayCurrentStateOfBoard();

                if (DisplayCurrentCoordinates)
                    PrintCurrentStateCoordinates();

                Thread.Sleep(TimeSleep);
            }
        }

        /// <summary>
        /// Write on console all cells in visual way.
        /// </summary>        
        public void DisplayCurrentStateOfBoard()
        {
            Console.WriteLine();

            for (int i = (int)-AreaSize; i < AreaSize * 2; i++)
            {
                for (int j = (int)-AreaSize * 4; j < AreaSize * 4; j++)
                {
                    if (IsCellExist(j, i))
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write(" ");
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
        /// Write on console information about coordinates of all cells.
        /// </summary>
        public void PrintCurrentStateCoordinates()
        {
            Console.WriteLine();

            foreach (Cell cell in _currentState)
            {
                Console.WriteLine("X: " + cell.X + ", Y: " + cell.Y);
            }

            // Why use PadLeft was described in PrintCurrentBoard method.
            Console.WriteLine("Cells on board: " + Convert.ToString(_currentState.Count).PadLeft(10, '0') + "\n");
        }
    }
}
