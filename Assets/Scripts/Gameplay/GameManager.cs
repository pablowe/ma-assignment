using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameSettings gameSettings;

    private Board board;
    private ResultChecker resultChecker;
    private MoveValidator moveValidator;
    private HintSystem hintSystem;
    private GameHistorySystem gameHistorySystem;
    private BoardMoveSystem boardMoveSystem;

    private Player player1,
                   player2;

    private Player currentPlayer;
    private bool isGameFinished;

    private Coroutine countdownCoroutine;

    private CancellationTokenSource aiMoveDelayTaskCancellationTokenSource = new CancellationTokenSource();

    private void Awake()
    {
        ServiceLocator.Register(this);

        board = new Board();
        resultChecker = new ResultChecker(board);
        moveValidator = new MoveValidator(board);
        hintSystem = new HintSystem(board);
        gameHistorySystem = new GameHistorySystem(board);
        boardMoveSystem = new BoardMoveSystem(board, moveValidator, gameHistorySystem);
        
        gameSettings = GetInitialGameSettings();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void InitializeGame()
    {
        isGameFinished = false;
        
        board.EmptyBoard();
        gameHistorySystem.ClearHistory();
        
        aiMoveDelayTaskCancellationTokenSource.Cancel();

        CreatePlayers();

        currentPlayer = player1.playersMark == Mark.X ? player1 : player2;
        
        PlayerChanged?.Invoke(currentPlayer);

        GameInitialized?.Invoke(gameSettings);

        StartNextRound();
    }

    public void OnBoardCellClicked(Vector2Int? cellCoordinates, bool isAiTurn = false)
    {
        if (isGameFinished) return;
        if (currentPlayer.playerType == PlayerType.Ai && !isAiTurn) return;
        if (cellCoordinates == null) return;

        var castedCellCoordinates = (Vector2Int)cellCoordinates;
        var didMove = boardMoveSystem.TryProceedMove(castedCellCoordinates, currentPlayer);
        
        if (!didMove) return;
        
        PlayerValidMove?.Invoke(castedCellCoordinates, currentPlayer.playersMark);

        CheckResultsAndUpdateGameState();
    }

    public void FindHintCoordinatesInCurrentBoard()
    {
        var suggestedCoordinates = hintSystem.GetHintCoordinates();
        
        SuggestedValidMoveFound?.Invoke(suggestedCoordinates);
    }

    public void TryUndoLastMove()
    {
        var revertedMovesCoord = gameHistorySystem.GetCoordsAndUndoLastMove();

        if (revertedMovesCoord == null) return;
        
        SwitchCurrentPlayer();
        isGameFinished = false;

        MoveReverted?.Invoke((Vector2Int)revertedMovesCoord);
        
        aiMoveDelayTaskCancellationTokenSource.Cancel();
        StartNextRound();
    }

    private void CreatePlayers()
    {
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
    }

    private void CheckResultsAndUpdateGameState()
    {
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

    private void FinishGame(Player winner)
    {
        isGameFinished = true;

        aiMoveDelayTaskCancellationTokenSource.Cancel();
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        
        GameFinished?.Invoke(winner);
    }
    
    private void StartNextRound()
    {
        RestartCountdown();
        aiMoveDelayTaskCancellationTokenSource.Cancel();
        
        if (currentPlayer.playerType == PlayerType.Ai) MakeAiMoveWithDelay();
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

    private async void MakeAiMoveWithDelay()
    {
        aiMoveDelayTaskCancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(500, aiMoveDelayTaskCancellationTokenSource.Token);
            OnBoardCellClicked(hintSystem.GetHintCoordinates(), true);
        }
        catch (TaskCanceledException) { }
    }

    private void RestartCountdown()
    {
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(StartNewCountdown());
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
    
    public Action<Vector2Int> MoveReverted;

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
