using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class GameHistorySystem
{
    private Board board;
    private Stack<Vector2Int> movesStack;
    
    public GameHistorySystem(Board board)
    {
        this.board = board;
        movesStack = new Stack<Vector2Int>();
    }

    public void ClearHistory()
    {
        movesStack.Clear();
    }

    public void WriteMove(Vector2Int cellCoordinates)
    {
        movesStack.Push(cellCoordinates);
    }

    public Vector2Int? GetCoordsAndUndoLastMove()
    {
        if (movesStack.Count == 0) return null;

        var lastMove = movesStack.Pop();
        
        board.SetCellAtPosition(lastMove.x, lastMove.y, board.GetEmptyCellValue);

        return lastMove;
    }
}
