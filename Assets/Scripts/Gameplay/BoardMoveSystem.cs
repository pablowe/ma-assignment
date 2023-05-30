using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMoveSystem
{
	private Board board;
	private MoveValidator moveValidator;
    private GameHistorySystem gameHistorySystem;
    
    public BoardMoveSystem(Board board, MoveValidator moveValidator, GameHistorySystem gameHistorySystem)
    {
	    this.board = board;
	    this.moveValidator = moveValidator;
	    this.gameHistorySystem = gameHistorySystem;
    }

    public bool TryProceedMove(Vector2Int moveCoordinates, Player currentPlayer)
    {
	    if (!moveValidator.IsMoveValid(moveCoordinates))
	    {
		    return false;
	    }

	    gameHistorySystem.WriteMove(moveCoordinates);

	    board.SetCellAtPosition(moveCoordinates.x, moveCoordinates.y, (int)currentPlayer.playersMark);

	    return true;
    }
}
