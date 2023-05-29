using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class GameWindow : Window
{
    [SerializeField]
    private List<BoardCell> boardCells;

    [SerializeField]
    private TextMeshProUGUI timeLeftTextMeshProUGUI;

    [SerializeField]
    private TextMeshProUGUI currentPlayerNameTextMeshProUGUI;

    [SerializeField]
    private Image currentPlayerMarkImage;

    [SerializeField]
    private Button hintButton,
                   undoButton;

    [SerializeField]
    private Sprite xMark,
                   oMark;
    
    /// <inheritdoc />
    public override void OnWillShow()
    {
        base.OnWillShow();

        InitializeBoard();

        GameManager.Instance.TimeLeftChanged += OnTimeLeftChanged;
        GameManager.Instance.GameInitialized += OnGameInitialized;
        GameManager.Instance.PlayerChanged += OnPlayerChanged;

        GameManager.Instance.InitializeGame();
    }

    /// <inheritdoc />
    public override void OnWillHide()
    {
        base.OnWillHide();

        DeinitializeBoard();
        
        GameManager.Instance.TimeLeftChanged -= OnTimeLeftChanged;
        GameManager.Instance.GameInitialized -= OnGameInitialized;
        GameManager.Instance.PlayerChanged -= OnPlayerChanged;
    }

    public void UndoButton()
    {
        GameManager.Instance.TryUndoLastMove();
    }

    public void HintButton()
    {
        GameManager.Instance.FindHintCoordinatesInCurrentBoard();
    }

    public void RestartGameButton()
    {
        GameManager.Instance.InitializeGame();
    }
    
    private void Awake()
    {
        SimpleUiManager.Instance.assetBundleLoaded += OnAssetBundleLoaded;
    }

    private void OnDestroy()
    {
        SimpleUiManager.Instance.assetBundleLoaded -= OnAssetBundleLoaded;
    }

    private void OnTimeLeftChanged(int timeLeft)
    {
        timeLeftTextMeshProUGUI.text = $"Time left: {timeLeft}s";
    }

    private void OnGameInitialized(GameSettings gameSettings)
    {
        undoButton.interactable = hintButton.interactable = gameSettings.gameMode != GameSettings.GameMode.PvP;
        
        foreach (var boardCell in boardCells) boardCell.ResetCell();
    }

    private void OnPlayerChanged(Player player)
    {
        currentPlayerMarkImage.sprite = player.playersMark == Mark.O ? oMark : xMark;
        currentPlayerNameTextMeshProUGUI.text = player.playerName;
    }

    private void InitializeBoard()
    {
        foreach (var boardCell in boardCells) boardCell.Initialize(xMark, oMark);
    }
    
    private void DeinitializeBoard()
    {
        foreach (var boardCell in boardCells) boardCell.Deinitialize();
    }
    
    private void OnAssetBundleLoaded(AssetBundle assetBundle)
    {
        var xMarkTexture = assetBundle.LoadAsset<Texture2D>("xMark");
        var oMarkTexture = assetBundle.LoadAsset<Texture2D>("oMark");

        if (xMarkTexture != null)
        {
            xMark = Sprite.Create(xMarkTexture, 
                                  new Rect(0, 0, xMarkTexture.width, xMarkTexture.height), 
                                  Vector2.zero);
        }

        if (oMarkTexture != null)
        {
            oMark = Sprite.Create(oMarkTexture, 
                                  new Rect(0, 0, oMarkTexture.width, oMarkTexture.height), 
                                  Vector2.zero);
        }
    }
}
