using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChecker
{
    private Board board;

    public ResultChecker(Board board)
    {
        this.board = board;
    }
    
    public bool IsDraw()
    {
        if (board == null) throw new ArgumentException("Game Board cannot be null");

        return TryGetWinningPlayersMark() == null && board.GetMovesNumber() >= board.GetSize();
    }

    public Mark? TryGetWinningPlayersMark()
    {
        var emptyCellValue = board.GetEmptyCellValue;
        for (int i = 0; i < 3; i++)
        {
            if ((board.GetCellAtPosition(i, 0) != emptyCellValue) && 
                (board.GetCellAtPosition(i, 0) == board.GetCellAtPosition(i, 1)) && 
                (board.GetCellAtPosition(i, 0) == board.GetCellAtPosition(i, 2))) 
                return (Mark)board.GetCellAtPosition(i, 0);
            
            if ((board.GetCellAtPosition(0, i) != emptyCellValue) && 
                (board.GetCellAtPosition(0, i) == board.GetCellAtPosition(1, i)) && 
                (board.GetCellAtPosition(0, i) == board.GetCellAtPosition(2, i))) 
                return (Mark)board.GetCellAtPosition(0, i);
        }

        if ((board.GetCellAtPosition(0, 0) != emptyCellValue) &&
            (board.GetCellAtPosition(0, 0) == board.GetCellAtPosition(1, 1)) &&
            (board.GetCellAtPosition(0, 0) == board.GetCellAtPosition(2, 2))) 
            return (Mark)board.GetCellAtPosition(0, 0);
        
        if ((board.GetCellAtPosition(0, 2) != emptyCellValue) && 
            (board.GetCellAtPosition(0, 2) == board.GetCellAtPosition(1, 1)) && 
            (board.GetCellAtPosition(0, 2) == board.GetCellAtPosition(2, 0))) 
            return (Mark)board.GetCellAtPosition(0, 2);

        return null;
    }
}
