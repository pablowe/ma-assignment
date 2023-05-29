using NUnit.Framework;

namespace Tests
{
    using UnityEngine;

    public class HintTest : BoardTest
    {
        [Test]
        public void NoValidMoves()
        {
            var board = new[,]
            {
                {X,O,X},
                {O,X,O},
                {O,X,O}
            };

            GameManager.FindHintCoordinates(out var coordinates, board);
            
            Assert.AreEqual(null, coordinates);
        }
        
        [Test]
        public void OnlyOneValidMove()
        {
            var board = new[,]
            {
                {X,O,X},
                {O,_,O},
                {O,X,O}
            };

            GameManager.FindHintCoordinates(out var coordinates, board);
            
            Assert.AreEqual(new Vector2Int{x = 1, y = 1}, coordinates);
        }
        
        [Test]
        public void MultipleValidMoves()
        {
            var board = new[,]
            {
                {_,O,_},
                {O,_,O},
                {X,X,_}
            };

            GameManager.FindHintCoordinates(out var coordinates, board);

            Assert.True((coordinates != null) && board[coordinates.Value.x, coordinates.Value.y] == _);
        }
    }
}
