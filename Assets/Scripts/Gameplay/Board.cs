using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	private int[,] boardData;
	
	private const int EmptyCellValue = -1;

	public Board()
	{
		EmptyBoard();
	}

	public int[,] GetCurrentData()
	{
		return boardData;
	}

	public void SetBoardData(int[,] data)
	{
		boardData = data;
	}

	public int GetCellAtPosition(int x, int y)
	{
		return boardData[x, y];
	}

	public void SetCellAtPosition(int x, int y, int valueToSet)
	{
		boardData[x, y] = valueToSet;
	}

	public int GetMovesNumber()
	{
		var validMovesNumber = 0;
        
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				if (boardData[i, j] != EmptyCellValue) 
					validMovesNumber++;
			}
		}

		return validMovesNumber;
	}

	public int GetSize()
	{
		return boardData.GetLength(0) * boardData.GetLength(1);
	}

	public int GetEmptyCellValue => EmptyCellValue;

	public void EmptyBoard()
	{
		boardData = new int[3, 3];
		
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++) 
				boardData[i, j] = EmptyCellValue;
		}
	}
}
