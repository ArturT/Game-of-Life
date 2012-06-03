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
            // new object of game
            Board b = new Board();
            
            // set your own size of board
            b.AreaSize = 5;


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


            
            //// display board
            //b.PrintCurrentBoard();

            //// display coordinates of cells
            //b.PrintCurrentStateCoordinates();

            //// calculate next state of game
            //b.NextState();

            //// and print board :)
            //b.PrintCurrentBoard();

            //// pause befor start game 
            //Thread.Sleep(2000);
            


            // run game in infinity loop
            b.PlayGame(timeSleep: 100, displayCurrentCoordinates: false);
            

            Console.ReadKey();
        }
    }
}
