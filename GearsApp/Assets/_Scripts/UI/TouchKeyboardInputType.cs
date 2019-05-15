using UnityEngine;

/// <summary>
/// Script to open different keyboard with different layouts.
/// </summary>
public class TouchKeyboardInputType : MonoBehaviour
{
    private string inputText;

    /// <summary>
    /// Open up keyboard with regular text layout.
    /// </summary>
    public void SetKeyboardTypeToText()
    {
        TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.NamePhonePad);
    }
    /// <summary>
    /// Open up keyboard with numbers only.
    /// </summary>
    public void SetKeyboardTypeToNumber()
    {
        TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.NumberPad);
    }
}
