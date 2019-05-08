using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inputcheck
{
    public static bool ValidateNumberInput(TMPro.TMP_InputField inputField)
    {
        if (inputField.text.Length != 8)
        {
            return false;
        }
        return true;
    }

    public static bool ValidateTextInput(TMPro.TMP_InputField inputField, bool isPassword=false)
    {
        string text = inputField.text;
        if (text.Length >= 6 && CheckForNumberInString(text, isPassword) && CheckForUpperCharactersInString(text) && CheckForWhiteSpace(text))
        {
            return true;
        }
        return false;
    }

    private static bool CheckForUpperCharactersInString(string text)
    {
        foreach (char character in text)
        {
            if (char.IsUpper(character))
            {
                return true;
            }
        }
        return false;
    }

    private static bool CheckForNumberInString(string text, bool isPassword = false)
    {
        foreach (char character in text)
        {
            if (char.IsDigit(character))
            {
                if (isPassword)
                    return true;
                else
                    return false;
            }
        }
        return true;
    }

    private static bool CheckForWhiteSpace(string text)
    {
        foreach (char character in text)
        {
            if (char.IsWhiteSpace(character))
            {
                return false;
            }
        }
        return true;
    }
}

