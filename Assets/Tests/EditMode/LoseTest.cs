using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class LoseTest : BoardTest
    {
        private Player player = new Player {playersMark = Mark.O, playerType = PlayerType.LocalPlayer};
        
        [Test]
        public void HorizontalCase()
        {
            var board = new[,]
            {
                {X,X,X},
                {O,_,O},
                {_,_,_}
            };

            Assert.IsTrue(GameManager.TryGetWinningPlayersMark(board) != null && GameManager.TryGetWinningPlayersMark(board) != player.playersMark);
        }
        
        [Test]
        public void VerticalCase()
        {
            var board = new[,]
            {
                {X,O,_},
                {X,_,O},
                {X,O,X}
            };

            Assert.IsTrue(GameManager.TryGetWinningPlayersMark(board) != null && GameManager.TryGetWinningPlayersMark(board) != player.playersMark);
        }
        
        [Test]
        public void DiagonalCase()
        {
            var board = new[,]
            {
                {X,O,_},
                {O,X,O},
                {X,O,X}
            };

            Assert.IsTrue(GameManager.TryGetWinningPlayersMark(board) != null && GameManager.TryGetWinningPlayersMark(board) != player.playersMark);
        }
        
        [Test]
        public void NoWinCase()
        {
            var board = new[,]
            {
                {X,O,X},
                {_,_,O},
                {X,O,X}
            };

            Assert.IsTrue(GameManager.TryGetWinningPlayersMark(board) == null);
        }
    }
}
