using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// Interface to visualize Board class.
    /// </summary>
    public interface IBoardVisualizer
    {
        /// <summary>
        /// Display current state of board on the screen.
        /// </summary>
        void DisplayCurrentStateOfBoard();

        /// <summary>
        /// Play game in infinity loop
        /// </summary>
        void PlayGame();
    }
}
