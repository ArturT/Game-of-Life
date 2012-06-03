using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;

namespace TestGameOfLife
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ShouldCellExistWhenWasAdded()
        {
            Board b = new Board();

            b.AddCell(1, 1);

            Assert.IsTrue(b.IsCellExist(1, 1));
        }

        [TestMethod]
        public void ShouldCellNoneExistWhenWasRemoved()
        {
            Board b = new Board();

            b.AddCell(1, 1);
            b.RemoveCell(1, 1);

            Assert.IsFalse(b.IsCellExist(1, 1));
        }

        [TestMethod]
        public void ShouldOneCellExistWhenAddedTwoTimesTheSameCell()
        {
            Board b = new Board();

            b.AddCell(1, 1);
            b.AddCell(1, 1);

            Assert.AreEqual(1, b.CountCells());
        }

        [TestMethod]
        public void ShouldCellDieWhenIsLonely()
        {
            Board b = new Board();

            b.AddCell(1,1);
            b.NextState();

            Assert.IsFalse(b.IsCellExist(1,1));
        }

        [TestMethod]
        public void ShouldLivingCellStillLiveWhenHasTwoOrThreeNeighbours()
        {
            // test for two neighbours
            Board b = new Board();
            
            b.AddCell(2, 2);
            b.AddCell(3, 2);
            b.AddCell(2, 3);
            b.NextState();

            Assert.IsTrue(b.IsCellExist(2, 2));


            // test for three neighbours
            b = new Board();

            b.AddCell(2, 2);
            b.AddCell(3, 2);
            b.AddCell(2, 3);
            b.AddCell(3, 3);
            b.NextState();

            Assert.IsTrue(b.IsCellExist(2, 2));
        }


        [TestMethod]
        public void ShouldLivingCellDieWhenHasLessThanTwoNeighbours()
        {
            Board b = new Board();

            b.AddCell(2, 2);
            b.AddCell(3, 2);
            b.NextState();

            Assert.IsFalse(b.IsCellExist(2, 2));
        }

        [TestMethod]
        public void ShouldLivingCellDieWhenHasMoreThanThreeNeighbours()
        {
            Board b = new Board();

            b.AddCell(2, 2);
            b.AddCell(3, 2);
            b.AddCell(2, 3);
            b.AddCell(3, 3);
            b.AddCell(1, 3);
            b.NextState();

            Assert.IsFalse(b.IsCellExist(2, 2));
        }

        [TestMethod]
        public void CountCellNeighbours()
        {
            Board b = new Board();

            b.AddCell(2, 2);
            b.AddCell(3, 2);
            
            Assert.AreEqual(2, b.CountNeighbours(3, 3));
        }

        [TestMethod]
        public void CountCells()
        {
            Board b = new Board();

            b.AddCell(-1, 34);
            b.AddCell(2, 2);
            b.AddCell(3, 2);
            b.AddCell(23, -2);

            Assert.AreEqual(4, b.CountCells());
        }
    }
}
