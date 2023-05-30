using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class LoseTest : BoardTest
    {
        private Player otherPlayer = new Player {playersMark = Mark.X, playerType = PlayerType.LocalPlayer};
        
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

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == otherPlayer.playersMark);
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

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == otherPlayer.playersMark);
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

            Assert.IsTrue(resultChecker.TryGetWinningPlayersMark() == otherPlayer.playersMark);
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
