using System;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    public bool isVisibleOnActivation;

    private void Start()
    {
        Initialize();
    }

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

    protected virtual void Initialize()
    {
        if (!isVisibleOnActivation) Hide();
    }

    protected virtual void OnWillShow() { }

    protected virtual void OnShown() { }

    protected virtual void OnWillHide() { }
}
