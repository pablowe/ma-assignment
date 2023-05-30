using UnityEngine;
using UnityEngine.UI;

public class BackgroundWindow : Window
{
    [SerializeField]
    private Image background;

    private void Start()
    {
	    ServiceLocator.ResolveAndGet<SimpleUiManager>().assetBundleLoaded += OnAssetBundleLoaded;
    }
    
    private void OnDestroy()
    {
	    ServiceLocator.ResolveAndGet<SimpleUiManager>().assetBundleLoaded -= OnAssetBundleLoaded;
    }

    private void OnAssetBundleLoaded(AssetBundle assetBundle)
    {
	    var texture = assetBundle.LoadAsset<Texture2D>("mainBackground");

	    if (texture == null) return;

	    background.sprite = Sprite.Create(texture, 
	                                      new Rect(0, 0, texture.width, texture.height), 
	                                      background.rectTransform.pivot);
    }
    
}
