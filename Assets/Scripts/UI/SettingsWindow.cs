using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI currentMoveTimeTextMeshProUGUI;

    [SerializeField]
    private Slider moveTimeSlider;
    
    /// <inheritdoc />
    public override void OnWillHide()
    {
        base.OnWillHide();

        GameManager.gameSettings.moveTime = (int)moveTimeSlider.value;
    }

    public void OnSliderValueChanged(float value)
    {
        currentMoveTimeTextMeshProUGUI.text = $"Time for a move: {(int)moveTimeSlider.value}s";
    }

    public void BackToMainMenuButton()
    {
        Hide();
    }
}
