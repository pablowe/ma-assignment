using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintSystem
{
    private Board board;
    
    public HintSystem(Board board)
    {
        this.board = board;
    }
    
    public Vector2Int? GetHintCoordinates()
    {
        if (board.GetMovesNumber() >= board.GetSize()) return null;

        return GetRandomEmptyCellCoordinates();
    }
    
    private Vector2Int GetRandomEmptyCellCoordinates()
    {
        var emptyCells = new List<Vector2Int>();
        
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (board.GetCellAtPosition(i,j) != board.GetEmptyCellValue) continue;
                
                emptyCells.Add(new Vector2Int(i,j));
            }
        }

        return emptyCells[Random.Range(0, emptyCells.Count)];
    }
}
