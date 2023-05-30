using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WinTest : BoardTest
    {
        [Test]
        public void HorizontalCase()
        {
            var data = new[,]
            {
                {X,X,X},
                {O,_,O},
                {_,_,_}
            };

            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);
            
            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == Mark.X);
        }
        
        [Test]
        public void VerticalCase()
        {
            var data = new[,]
            {
                {X,O,_},
                {X,_,O},
                {X,O,X}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == Mark.X);
        }
        
        [Test]
        public void DiagonalCase()
        {
            var data = new[,]
            {
                {X,O,_},
                {O,X,O},
                {X,O,X}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == Mark.X);
        }
        
        [Test]
        public void NoWinCase()
        {
            var data = new[,]
            {
                {X,O,X},
                {_,_,O},
                {X,O,X}
            };
            
            var board = new Board();
            board.SetBoardData(data);
            
            var resultChecker = new ResultChecker(board);

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == null);
        }
    }
}
