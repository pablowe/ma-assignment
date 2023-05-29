﻿using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    using UnityEngine;

    public class UndoTest : BoardTest
    {
        [Test]
        public void MultipleUndo()
        {
            var board = new[,]
            {
                {X,X,X},
                {O,_,O},
                {_,_,_}
            };

            var moveCoordinatesHistory = new []
            {
                new Vector2Int { x = 0, y = 0 },
                new Vector2Int { x = 1, y = 0 },
                new Vector2Int { x = 0, y = 1 },
                new Vector2Int { x = 1, y = 2 },
                new Vector2Int { x = 0, y = 2 }
            };
            
            var boardMoveHistory = new []
            {
                new [,] 
                {
                    {_,X,X},
                    {O,_,O},
                    {_,_,_}
                },
                new [,] 
                {
                    {_,X,X},
                    {_,_,O},
                    {_,_,_}
                },
                new [,]
                {
                    {_,_,X},
                    {_,_,O},
                    {_,_,_}
                },
                new [,]
                {
                    {_,_,X},
                    {_,_,_},
                    {_,_,_}
                },
                new [,]
                {
                    {_,_,_},
                    {_,_,_},
                    {_,_,_}
                }
            };

            for (int i = 0; i < moveCoordinatesHistory.Length; i++)
            {
                GameManager.UndoMove(moveCoordinatesHistory[i], board);
                Assert.AreEqual(boardMoveHistory[i], board);
            }
        }
    }
}
