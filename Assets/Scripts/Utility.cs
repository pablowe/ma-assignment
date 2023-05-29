using System;

public static class Utility
{
	public static GameSettings.GameMode GetGameModeFromIndex(int index)
	{
		return (GameSettings.GameMode)index;
	}

	public static string GetGameModeName(GameSettings.GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameSettings.GameMode.PvP:
				return "Player vs Player";
			case GameSettings.GameMode.PvC: 
				return "Player vs Computer";
			case GameSettings.GameMode.CvC: 
				return "Computer vs Computer";
			default: throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
		}
	}

	public static string GetDefaultPlayerName(PlayerType playerType)
	{
		switch (playerType)
		{
			case PlayerType.LocalPlayer: 
				return "Player";
			case PlayerType.Ai: 
				return "Computer";
			default: throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
		}
	}
}
