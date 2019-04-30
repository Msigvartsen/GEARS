using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchKeyboardInputType : MonoBehaviour
{
    private string inputText;

    private void Start()
    {
        //TouchScreenKeyboard.hideInput = true;
    }
    public void SetKeyboardTypeToText()
    {
        TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.NamePhonePad);
    }
    public void SetKeyboardTypeToNumber()
    {
        TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.NumberPad);
    }
}
