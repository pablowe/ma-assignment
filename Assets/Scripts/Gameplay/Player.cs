public class Player
{
    public Mark playersMark;

    public PlayerType playerType;

    public string playerName;
}

public enum Mark
{
    X,
    O
}

public enum PlayerType
{
    LocalPlayer,
    Ai
}
