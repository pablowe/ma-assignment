using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }

    public static GameSettings gameSettings;

    private Board board;
    private ResultChecker resultChecker;
    private MoveValidator moveValidator;
    private HintSystem hintSystem;

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

        board = new Board();
        resultChecker = new ResultChecker(board);
        moveValidator = new MoveValidator(board);
        hintSystem = new HintSystem(board);
        
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
        
        board.EmptyBoard();

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
        
        if (!moveValidator.IsMoveValid(castedCellCoordinates))
        {
            return;
        }

        movesHistory[board.GetMovesNumber()] = castedCellCoordinates;

        SetMarkOnBoard(castedCellCoordinates);
        PlayerValidMove?.Invoke(castedCellCoordinates, currentPlayer.playersMark);

        if (resultChecker.TryGetWinningPlayersMark() == currentPlayer.playersMark)
        {
            FinishGame(currentPlayer);
            return;
        }
        
        if (resultChecker.IsDraw())
        {
            FinishGame(null);
            return;
        }
        
        SwitchCurrentPlayer();
        StartNextRound();
    }

    public void FindHintCoordinatesInCurrentBoard()
    {
        var suggestedCoordinates = hintSystem.GetHintCoordinates();
        
        SuggestedValidMoveFound?.Invoke(suggestedCoordinates);
    }

    public void TryUndoLastMove()
    {
        var validMovesNumber = board.GetMovesNumber();
        
        if (validMovesNumber <= 0) return;
        
        if (validMovesNumber < MaxValidMoves) SwitchCurrentPlayer();

        isGameFinished = false;

        var lastMoveCoordinates = movesHistory[validMovesNumber - 1];
        
        UndoMove(lastMoveCoordinates, board.GetCurrentData());
        
        MoveRevered?.Invoke(lastMoveCoordinates);
        
        StartNextRound();
    }

    public static void UndoMove(Vector2Int lastMoveCoordinates, int[,] gameBoard)
    {
        ClearBoardCell(lastMoveCoordinates, gameBoard);
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
                                                     OnBoardCellClicked(hintSystem.GetHintCoordinates(), true);
                                                 }
                                             ));
    }

    private static void ClearBoardCell(Vector2Int cellCoordinates, int[,] gameBoard)
    {
        gameBoard[cellCoordinates.x, cellCoordinates.y] = EmptyCellValue;
    }

    private void SetMarkOnBoard(Vector2Int cellCoordinates)
    {
        board.SetCellAtPosition(cellCoordinates.x, cellCoordinates.y, (int)currentPlayer.playersMark);
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
