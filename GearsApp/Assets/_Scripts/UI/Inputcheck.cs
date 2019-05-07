using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inputcheck
{
    public static bool ValidateInput(TMPro.TMP_InputField inputField)
    {
        if(inputField.text.Length != 8)
        {
            return false;
        }
        return true;
    }
}
