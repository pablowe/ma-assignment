using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DrawTest : BoardTest
    {
        [Test]
        public void MaxValidMoves()
        {
            var data = new[,]
            {
                {X,O,X},
                {O,X,O},
                {O,X,O}
            };

            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(resultChecker.IsDraw());
        }
        
        [Test]
        public void ValidMovesLeft()
        {
            var data = new[,]
            {
                {X,_,_},
                {O,X,O},
                {O,X,O}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(!resultChecker.IsDraw());
        }
        
        [Test]
        public void PlayerXWon()
        {
            var data = new[,]
            {
                {X,X,X},
                {O,O,X},
                {O,X,O}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(!resultChecker.IsDraw());
        }
        
        [Test]
        public void PlayerOWon()
        {
            var data = new[,]
            {
                {_,X,O},
                {O,O,X},
                {O,X,X}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(!resultChecker.IsDraw());
        }
    }
}
