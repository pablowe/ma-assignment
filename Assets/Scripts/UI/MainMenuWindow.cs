using TMPro;

using UnityEngine;

public class MainMenuWindow : Window
{
    [SerializeField]
    private GameObject modesSection;

    [SerializeField]
    private TextMeshProUGUI selectedModeText;

    [SerializeField]
    private TMP_InputField assetBundleNameInputField;

    public void SelectModeButton()
    {
        modesSection.SetActive(true);
    }

    public void ModeButton(int mode)
    {
        var gameMode = Utility.GetGameModeFromIndex(mode);
        
        GameManager.gameSettings.gameMode = gameMode;
        selectedModeText.text = Utility.GetGameModeName(gameMode);
        modesSection.SetActive(false);
    }

    public void StartGameButton()
    {
        SimpleUiManager.Instance.SetView(SimpleUiManager.View.Game);
    }

    public void ReSkinButton()
    {
        SimpleUiManager.Instance.TryLoadAssetBundle(assetBundleNameInputField.text);
    }

    public void SettingsWindow()
    {
        SimpleUiManager.Instance.settingsWindow.Show();
    }
}
