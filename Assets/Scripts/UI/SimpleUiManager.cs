using System;
using System.IO;
using System.Linq;

using UnityEngine;

public class SimpleUiManager : MonoBehaviour
{
    public GameWindow gameWindow;

    public MainMenuWindow mainMenuWindow;

    public GameSummaryWindow gameSummaryWindow;

    public SettingsWindow settingsWindow;
    public static SimpleUiManager Instance { get { return instance; } }
    
    private static SimpleUiManager instance;

    private View currentView = View.MainMenu;

    public enum View
    {
        Game,
        MainMenu
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            instance = this;
        }
    }

    public void SetView(View viewToShow)
    {
        if (currentView == viewToShow) return;

        HideWindow(currentView);
        ShowWindow(viewToShow);

        currentView = viewToShow;
    }

    public void TryLoadAssetBundle(string assetBundleName)
    {
        var assetBundlePath = Path.Combine(Application.streamingAssetsPath, assetBundleName);
        
        var loadedAssetBundles = AssetBundle.GetAllLoadedAssetBundles();

        if (loadedAssetBundles != null &&
            AssetBundle.GetAllLoadedAssetBundles().Any(x => x.name == assetBundleName)) return;

        if (!System.IO.File.Exists(assetBundlePath)) return;
        
        var loadedAssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        
        if (loadedAssetBundle != null) assetBundleLoaded?.Invoke(loadedAssetBundle);
    }

    private void HideWindow(View viewToHide)
    {
        switch (viewToHide)
        {
            case View.Game: 
                gameWindow.Hide();
                break;
            case View.MainMenu: 
                mainMenuWindow.Hide();
                break;
            default: throw new ArgumentOutOfRangeException(nameof(viewToHide), viewToHide, null);
        }
    }

    private void ShowWindow(View viewToShow)
    {
        switch (viewToShow)
        {
            case View.Game: 
                gameWindow.Show();
                break;
            case View.MainMenu: 
                mainMenuWindow.Show();
                break;
            default: throw new ArgumentOutOfRangeException(nameof(viewToShow), viewToShow, null);
        }
    }

    public Action<AssetBundle> assetBundleLoaded;
}
