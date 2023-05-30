﻿using TMPro;

using UnityEngine;

public class GameSummaryWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI summaryTextMeshProUGUI;

    private void Start()
    {
        GameManager.Instance.GameFinished += OnGameFinished;
    }

    public void BackToMainMenuButton()
    {
        SimpleUiManager.Instance.SetView(SimpleUiManager.View.MainMenu);
        Hide();
    }

    private void OnGameFinished(Player winningPlayer)
    {
        summaryTextMeshProUGUI.text = winningPlayer == null ? "Draw" : $"{winningPlayer.playerName} won!";
        
        Show();
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameFinished -= OnGameFinished;
    }
}