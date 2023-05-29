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
            var board = new[,]
            {
                {X,O,X},
                {O,X,O},
                {O,X,O}
            };

            Assert.IsTrue(GameManager.IsDraw(board));
        }
        
        [Test]
        public void ValidMovesLeft()
        {
            var board = new[,]
            {
                {X,_,_},
                {O,X,O},
                {O,X,O}
            };

            Assert.IsTrue(!GameManager.IsDraw(board));
        }
        
        [Test]
        public void PlayerXWon()
        {
            var board = new[,]
            {
                {X,X,X},
                {O,O,X},
                {O,X,O}
            };

            Assert.IsTrue(!GameManager.IsDraw(board));
        }
        
        [Test]
        public void PlayerOWon()
        {
            var board = new[,]
            {
                {_,X,O},
                {O,O,X},
                {O,X,X}
            };

            Assert.IsTrue(!GameManager.IsDraw(board));
        }
    }
}
