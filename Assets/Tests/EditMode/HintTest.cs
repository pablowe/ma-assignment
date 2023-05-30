using NUnit.Framework;

namespace Tests
{
    using UnityEngine;

    public class HintTest : BoardTest
    {
        [Test]
        public void NoValidMoves()
        {
            var data = new[,]
            {
                {X,O,X},
                {O,X,O},
                {O,X,O}
            };
            
            var board = new Board();
            board.SetBoardData(data);

            var hintSystem = new HintSystem(board);

            Assert.AreEqual(null, hintSystem.GetHintCoordinates());
        }
        
        [Test]
        public void OnlyOneValidMove()
        {
            var data = new[,]
            {
                {X,O,X},
                {O,_,O},
                {O,X,O}
            };

            var board = new Board();
            board.SetBoardData(data);

            var hintSystem = new HintSystem(board);
            
            Assert.AreEqual(new Vector2Int{x = 1, y = 1}, hintSystem.GetHintCoordinates());
        }
        
        [Test]
        public void MultipleValidMoves()
        {
            var data = new[,]
            {
                {_,O,_},
                {O,_,O},
                {X,X,_}
            };

            var board = new Board();
            board.SetBoardData(data);

            var hintSystem = new HintSystem(board);

            var hintCoordinates = hintSystem.GetHintCoordinates();

            Assert.True((hintCoordinates != null) && board.GetCellAtPosition(hintCoordinates.Value.x, hintCoordinates.Value.y) == _);
        }
    }
}
