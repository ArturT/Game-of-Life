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
    class Program
    {
        static void Main(string[] args)
        {
            // new object of console game of life
            BoardConsoleVisualizer b = new BoardConsoleVisualizer(areaSize: 5, timeSleep: 250, displayCurrentCoordinates: false);
            
            // set your own size of board in constructor or override it here
            b.AreaSize = 7;


            // Set cells coordinates here
            
            // toad vertical 
            // http://en.wikipedia.org/wiki/File:Game_of_life_toad.gif
            b.AddCell(1, 1);
            b.AddCell(1, 2);
            b.AddCell(1, 3);
            b.AddCell(2, 2);
            b.AddCell(2, 3);
            b.AddCell(2, 4);

            // tub
            // http://pl.wikipedia.org/wiki/Plik:JdlV_bloc_4.9.gif
            b.AddCell(-5, 0);
            b.AddCell(-5, 2);
            b.AddCell(-6, 1);
            b.AddCell(-4, 1);

            // glider
            // http://en.wikipedia.org/wiki/File:Game_of_life_animated_glider.gif
            b.AddCell(-18, -5);
            b.AddCell(-18, -4);
            b.AddCell(-18, -3);
            b.AddCell(-19, -3);
            b.AddCell(-20, -4);

            // blinker 
            // http://en.wikipedia.org/wiki/File:Game_of_life_blinker.gif
            b.AddCell(10, 0);
            b.AddCell(11, 0);
            b.AddCell(12, 0);

            // blinker 
            b.AddCell(-10, 5);
            b.AddCell(-10, 6);
            b.AddCell(-10, 7);



            // some random cells
            Random random = new Random();
            int randomX, randomY;

            // 100 random cells
            for (int i = 0; i < 100; i++)
            {
                randomX = random.Next(-20, 20);
                randomY = random.Next(-20, 20);

                b.AddCell(randomX, randomY);
            }


            # region RunGameExample1
            //// display board
            //b.DisplayCurrentStateOfBoard();

            //// display coordinates of cells
            //b.PrintCurrentStateCoordinates();

            //// calculate next state of game
            //b.NextState();

            //// and print board :)
            //b.DisplayCurrentStateOfBoard();

            //// pause befor start game 
            //Thread.Sleep(2000);
            # endregion RunGameExample2


            # region RunGameExample2
            // run game in infinity loop
            b.PlayGame();
            # endregion RunGameExample2


            # region RunGameExample3
            // shows amount of cells and attempts to give life at step
            //for (int i = 0; i < 100; i++)
            //{
            //    b.NextState();
            //    Console.WriteLine("Step " + i + ", Cells on board: " + b.CountCells() + ", Attempts to give life: " + b.CounterMethod_GiveLifeToNeighboursIfPossible);
            //}
            # endregion RunGameExample3


            Console.ReadKey();
        }
    }
}
