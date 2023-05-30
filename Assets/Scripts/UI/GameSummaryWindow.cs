using TMPro;

using UnityEngine;

public class GameSummaryWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI summaryTextMeshProUGUI;

    /// <inheritdoc />
    protected override void Initialize()
    {
        ServiceLocator.ResolveAndGet<GameManager>().GameFinished += OnGameFinished;
        base.Initialize();
    }

    public void BackToMainMenuButton()
    {
        ServiceLocator.ResolveAndGet<SimpleUiManager>().SetView(SimpleUiManager.View.MainMenu);
        Hide();
    }

    private void OnGameFinished(Player winningPlayer)
    {
        summaryTextMeshProUGUI.text = winningPlayer == null ? "Draw" : $"{winningPlayer.playerName} won!";
        
        Show();
    }

    private void OnDestroy()
    {
        ServiceLocator.ResolveAndGet<GameManager>().GameFinished -= OnGameFinished;
    }
}
