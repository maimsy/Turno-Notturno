using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingUI : MonoBehaviour
{
    [SerializeField] GameObject hintIcon;
    [SerializeField] GameObject cursorIcon;
    [SerializeField] Text cluesRemaining;
    [SerializeField] Text tooltip;
    
    public void EnableHintCursor(bool value)
    {
        if (hintIcon) hintIcon.SetActive(value);
        if (cursorIcon) cursorIcon.SetActive(!value);
    }

    public void HideCursor()
    {
        if (hintIcon) hintIcon.SetActive(false);
        if (cursorIcon) cursorIcon.SetActive(false);
        if (cluesRemaining) cluesRemaining.text = "";
    }

    public void SetCluesRemainingText(String str)
    {
        if (cluesRemaining) cluesRemaining.text = str;
    }

    public void SetTooltip(String str)
    {
        if (tooltip) tooltip.text = str;
    }
}
