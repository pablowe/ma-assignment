using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }

    public static GameSettings gameSettings;

    private static int[,] board;

    private Player player1,
                   player2;

    private Player currentPlayer;

    private bool isGameFinished;

    private Coroutine countdownCoroutine;

    private Coroutine aiDelayedMoveCoroutine;

    private Vector2Int[] movesHistory;

    private static GameManager instance;

    private const int EmptyCellValue = -1;

    private const int MaxValidMoves = 9;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Destroy(gameObject);
        } 
        else 
        {
            instance = this;
        }

        InitializeBoard();
        gameSettings = GetInitialGameSettings();
    }

    public void InitializeGame()
    {
        isGameFinished = false;
        
        player1 = new Player
        {
            playerType = gameSettings.gameMode == GameSettings.GameMode.CvC ? PlayerType.Ai : PlayerType.LocalPlayer, 
            playersMark = Random.Range(0,1f) > 0.5f ? Mark.X : Mark.O
        };
        player1.playerName = $"{Utility.GetDefaultPlayerName(player1.playerType)} (Player 1)";

        player2 = new Player
        {
            playerType = gameSettings.gameMode == GameSettings.GameMode.PvP ? PlayerType.LocalPlayer : PlayerType.Ai,
            playersMark = player1.playersMark == Mark.O ? Mark.X : Mark.O
        };
        player2.playerName = $"{Utility.GetDefaultPlayerName(player2.playerType)} (Player 2)";

        currentPlayer = player1.playersMark == Mark.X ? player1 : player2;
        
        PlayerChanged?.Invoke(currentPlayer);
        
        InitializeBoard();

        movesHistory = new Vector2Int[MaxValidMoves];

        if (aiDelayedMoveCoroutine != null) StopCoroutine(aiDelayedMoveCoroutine);
        
        GameInitialized?.Invoke(gameSettings);

        StartNextRound();
    }

    public void OnBoardCellClicked(Vector2Int? cellCoordinates, bool isAiTurn = false)
    {
        if (isGameFinished || 
            ((currentPlayer.playerType == PlayerType.Ai) && !isAiTurn) ||
            (cellCoordinates == null)) return;

        var castedCellCoordinates = (Vector2Int)cellCoordinates;
        
        if (!GetMoveValidity(castedCellCoordinates))
        {
            return;
        }

        movesHistory[GetValidMovesNumber(board)] = castedCellCoordinates;

        SetMarkOnBoard(castedCellCoordinates);
        PlayerValidMove?.Invoke(castedCellCoordinates, currentPlayer.playersMark);

        if (DidPlayerWin(board, currentPlayer))
        {
            FinishGame(currentPlayer);
            return;
        }
        
        if (IsDraw(board))
        {
            FinishGame(null);
            return;
        }
        
        SwitchCurrentPlayer();
        StartNextRound();
    }

    public static void FindHintCoordinates(out Vector2Int? suggestedCoordinates, int[,] gameBoard)
    {
        suggestedCoordinates = null;
        
        if (GetValidMovesNumber(gameBoard) < MaxValidMoves) suggestedCoordinates = GetRandomEmptyCellCoordinates(gameBoard);
    }

    public void FindHintCoordinatesInCurrentBoard()
    {
        FindHintCoordinates(out var suggestedCoordinates, board);
        
        SuggestedValidMoveFound?.Invoke(suggestedCoordinates);
    }

    public void TryUndoLastMove()
    {
        var validMovesNumber = GetValidMovesNumber(board);
        
        if (validMovesNumber <= 0) return;
        
        if (validMovesNumber < MaxValidMoves) SwitchCurrentPlayer();

        isGameFinished = false;

        var lastMoveCoordinates = movesHistory[validMovesNumber - 1];
        
        UndoMove(lastMoveCoordinates, board);
        
        MoveRevered?.Invoke(lastMoveCoordinates);
        
        StartNextRound();
    }

    public static void UndoMove(Vector2Int lastMoveCoordinates, int[,] gameBoard)
    {
        ClearBoardCell(lastMoveCoordinates, gameBoard);
    }

    public static bool IsDraw(int[,] gameBoard)
    {
        if (gameBoard == null) throw new ArgumentException("Game Board cannot be null");

        return (TryGetWinningPlayersMark(gameBoard) == null) && (GetValidMovesNumber(gameBoard) >= MaxValidMoves);
    }

    public static bool DidPlayerWin(int[,] gameBoard, Player player)
    {
        if ((player == null) || (gameBoard == null)) throw new ArgumentException("Player or Game Board cannot be null");
        
        return TryGetWinningPlayersMark(gameBoard) == player.playersMark;
    }
    
    public static Mark? TryGetWinningPlayersMark(int[,] gameBoard)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((gameBoard[i, 0] != EmptyCellValue) && (gameBoard[i, 0] == gameBoard[i, 1]) && (gameBoard[i, 0] == gameBoard[i, 2])) return (Mark)gameBoard[i, 0];
            if ((gameBoard[0, i] != EmptyCellValue) && (gameBoard[0, i] == gameBoard[1, i]) && (gameBoard[0, i] == gameBoard[2, i])) return (Mark)gameBoard[0, i];
        }
        
        if ((gameBoard[0, 0] != EmptyCellValue) && (gameBoard[0, 0] == gameBoard[1, 1]) && (gameBoard[0, 0] == gameBoard[2, 2])) return (Mark)gameBoard[0, 0];
        if ((gameBoard[0, 2] != EmptyCellValue) && (gameBoard[0, 2] == gameBoard[1, 1]) && (gameBoard[0, 2] == gameBoard[2, 0])) return (Mark)gameBoard[0, 2];

        return null;
    }
    
    private void InitializeBoard()
    {
        if (board == null) board = new int[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++) 
                board[i, j] = EmptyCellValue;
        }
    }

    private static int GetValidMovesNumber(int[,] gameBoard)
    {
        var validMovesNumber = 0;
        
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
                if (gameBoard[i, j] != EmptyCellValue)
                    validMovesNumber++;
        }

        return validMovesNumber;
    }

    private void FinishGame(Player winner)
    {
        isGameFinished = true;

        if (aiDelayedMoveCoroutine != null) StopCoroutine(aiDelayedMoveCoroutine);
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        
        GameFinished?.Invoke(winner);
    }

    private void SwitchCurrentPlayer()
    {
        currentPlayer = GetNextPlayer();
        
        PlayerChanged?.Invoke(currentPlayer);
    }

    private Player GetNextPlayer()
    {
        return currentPlayer == player1 ? player2 : player1;
    }

    private void StartNextRound()
    {
        RestartCountdown();
        
        if (aiDelayedMoveCoroutine != null) StopCoroutine(aiDelayedMoveCoroutine);
        
        if (currentPlayer.playerType == PlayerType.Ai) MakeAiMove();
    }

    private void MakeAiMove()
    {
        aiDelayedMoveCoroutine = StartCoroutine(DelayActionCoroutine(
                                                 0.5f, 
                                                 () =>
                                                 {
                                                     OnBoardCellClicked(GetRandomEmptyCellCoordinates(board), true);
                                                 }
                                             ));
    }

    private static void ClearBoardCell(Vector2Int cellCoordinates, int[,] gameBoard)
    {
        gameBoard[cellCoordinates.x, cellCoordinates.y] = EmptyCellValue;
    }

    private static Vector2Int? GetRandomEmptyCellCoordinates(int[,] gameBoard)
    {
        var emptyCells = new List<Vector2Int>();
        
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++) 
                if (gameBoard[i,j] == EmptyCellValue) emptyCells.Add(new Vector2Int(i,j));
        }

        if (emptyCells.Count == 0) return null;

        return emptyCells[Random.Range(0, emptyCells.Count)];
    }

    private void SetMarkOnBoard(Vector2Int cellCoordinates)
    {
        board[cellCoordinates.x, cellCoordinates.y] = (int)currentPlayer.playersMark;
    }

    private bool GetMoveValidity(Vector2Int cellCoordinates)
    {
        return board[cellCoordinates.x, cellCoordinates.y] == -1;
    }

    private void RestartCountdown()
    {
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(StartNewCountdown());
    }

    private IEnumerator DelayActionCoroutine(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        action?.Invoke();
    }

    private IEnumerator StartNewCountdown()
    {
        var timeLeft = gameSettings.moveTime;
        
        TimeLeftChanged?.Invoke(timeLeft);

        while (timeLeft > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            timeLeft -= 1;
            
            TimeLeftChanged?.Invoke(timeLeft);
        }

        FinishGame(GetNextPlayer());
    }

    private GameSettings GetInitialGameSettings()
    {
        return new GameSettings
        {
            gameMode = GameSettings.GameMode.PvC,
            moveTime = 5
        };
    }

    public Action<int> TimeLeftChanged;
    
    public Action<Vector2Int?> SuggestedValidMoveFound;
    
    public Action<Vector2Int> MoveRevered;

    public Action<Vector2Int, Mark> PlayerValidMove;

    public Action<Player> GameFinished;

    public Action<GameSettings> GameInitialized;

    public Action<Player> PlayerChanged;
}

public class GameSettings
{
    public GameMode gameMode;

    public int moveTime;
    
    public enum GameMode
    {
        PvP,
        PvC,
        CvC
    }
}
