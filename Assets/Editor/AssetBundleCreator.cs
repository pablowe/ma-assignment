using UnityEditor;
using UnityEngine;

public class AssetBundleCreator : EditorWindow
{
    private static AssetBundleSettings assetBundleSettings;

    private static string SavePath => Application.dataPath + "/StreamingAssets";

    [MenuItem("Game Editors/Asset Bundle Creator")]
    private static void ShowWindow()
    {
        TryInitialize();
        
        var window = GetWindow<AssetBundleCreator>();
        window.titleContent = new GUIContent("Asset Bundle Creator");

        window.Show();
    }
    
    private void OnGUI()
    {
        TryInitialize();
        
        if (assetBundleSettings == null) assetBundleSettings = new AssetBundleSettings();
        
        assetBundleSettings.assetBundleName = GetTextField(assetBundleSettings.assetBundleName, "Name");

        assetBundleSettings.xSymbol = GetTextureField(assetBundleSettings.xSymbol, "X Symbol");
        assetBundleSettings.oSymbol = GetTextureField(assetBundleSettings.oSymbol, "O Symbol");
        assetBundleSettings.background = GetTextureField(assetBundleSettings.background, "Background");
        
        SetCreateAssetBundleSection();
    }

    private void SetCreateAssetBundleSection()
    {
        if (GUILayout.Button("Build")) BuildAssetBundle();
    }

    private static Texture2D GetTextureField(Texture2D value, string name)
    {
        return (Texture2D)EditorGUILayout.ObjectField(name, value, typeof(Texture2D), false);
    }
    
    private static string GetTextField(string text, string name)
    {
        return EditorGUILayout.TextField(name, text);
    }

    private void BuildAssetBundle()
    {
        if (assetBundleSettings == null ||
            assetBundleSettings.xSymbol == null ||
            assetBundleSettings.oSymbol == null ||
            assetBundleSettings.background == null ||
            string.IsNullOrEmpty(assetBundleSettings.assetBundleName))
        {
            EditorUtility.DisplayDialog(
                "Error",
                "All of the fields must be filled to build Asset Bundle", 
                "Ok");
            
            return;
        }
        
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        buildMap[0].assetBundleName = assetBundleSettings.assetBundleName;

        string[] uiAssetNames = new string[3];
        uiAssetNames[0] = AssetDatabase.GetAssetPath(assetBundleSettings.xSymbol);
        uiAssetNames[1] = AssetDatabase.GetAssetPath(assetBundleSettings.oSymbol);
        uiAssetNames[2] = AssetDatabase.GetAssetPath(assetBundleSettings.background);
        
        string[] uiAssetAddressableNames = new string[3];
        uiAssetAddressableNames[0] = "xMark";
        uiAssetAddressableNames[1] = "oMark";
        uiAssetAddressableNames[2] = "mainBackground";

        buildMap[0].assetNames = uiAssetNames;
        buildMap[0].addressableNames = uiAssetAddressableNames;
        
        BuildPipeline.BuildAssetBundles(SavePath, buildMap, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }

    private static void TryInitialize()
    {
        if (assetBundleSettings == null) assetBundleSettings = new AssetBundleSettings();
    }

    private class AssetBundleSettings
    {
        public string assetBundleName;

        public Texture2D xSymbol;

        public Texture2D oSymbol;

        public Texture2D background;
    }
}