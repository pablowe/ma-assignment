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
            if ((board.GetCellValue(i, 0) != emptyCellValue) && 
                (board.GetCellValue(i, 0) == board.GetCellValue(i, 1)) && 
                (board.GetCellValue(i, 0) == board.GetCellValue(i, 2))) 
                return (Mark)board.GetCellValue(i, 0);
            
            if ((board.GetCellValue(0, i) != emptyCellValue) && 
                (board.GetCellValue(0, i) == board.GetCellValue(1, i)) && 
                (board.GetCellValue(0, i) == board.GetCellValue(2, i))) 
                return (Mark)board.GetCellValue(0, i);
        }

        if ((board.GetCellValue(0, 0) != emptyCellValue) &&
            (board.GetCellValue(0, 0) == board.GetCellValue(1, 1)) &&
            (board.GetCellValue(0, 0) == board.GetCellValue(2, 2))) 
            return (Mark)board.GetCellValue(0, 0);
        
        if ((board.GetCellValue(0, 2) != emptyCellValue) && 
            (board.GetCellValue(0, 2) == board.GetCellValue(1, 1)) && 
            (board.GetCellValue(0, 2) == board.GetCellValue(2, 0))) 
            return (Mark)board.GetCellValue(0, 2);

        return null;
    }
}
