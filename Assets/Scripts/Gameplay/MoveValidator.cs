using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator
{
    private Board board;
    
    public MoveValidator(Board board)
    {
        this.board = board;
    }

    public bool IsMoveValid(Vector2Int cellCoordsToSetMark)
    {
        var testingCell = board.GetCellAtPosition(cellCoordsToSetMark.x, cellCoordsToSetMark.y);
        
        return testingCell == -1;
    }
}
