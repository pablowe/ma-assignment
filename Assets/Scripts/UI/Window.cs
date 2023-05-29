using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    public bool isVisibleOnActivation;
    
    public void Show()
    {
        OnWillShow();
        gameObject.SetActive(true);
        OnShown();
    }

    public void Hide()
    {
        OnWillHide();
        gameObject.SetActive(false);
    }

    public virtual void OnWillShow() { }

    public virtual void OnShown() { }

    public virtual void OnWillHide() { }
}
