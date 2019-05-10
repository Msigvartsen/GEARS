
/// <summary>
/// InputCheck
/// Static class to run Inputchecks on registration form.
/// </summary>
public static class Inputcheck
{
    /// <summary>
    /// Checks if the number is 8 digits long to fit Norwegian mobile numbers.
    /// </summary>
    /// <param name="inputField"></param>
    /// <returns>Returns true if test pass.</returns>
    public static bool ValidateNumberInput(TMPro.TMP_InputField inputField)
    {
        if (inputField.text.Length != 8)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the Username and password has correct input.
    /// if isPassword = false - The function checks the username. Username should not have numbers in them, while passwords needs it.
    /// Set isPassword to true, if a number is needed to pass the test.
    /// </summary>
    /// <param name="inputField"></param>
    /// <param name="isPassword"></param>
    /// <returns>Returns true if all tests pass.</returns>
    public static bool ValidateTextInput(TMPro.TMP_InputField inputField, int length, bool isPassword=false)
    {
        string text = inputField.text;
        if (text.Length >= length && CheckForNumberInString(text, isPassword) && CheckForUpperCharactersInString(text) && CheckForWhiteSpace(text))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the string has upper letters.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Return true if a character is an upper letter.</returns>
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

    /// <summary>
    /// Check if the string has a number in it. If isPassword is set to true the function returns true
    /// if the string has a letter in it.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="isPassword"></param>
    /// <returns>Returns true if all tests pass.</returns>
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

    /// <summary>
    /// Checks if a string contains white space. 
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Returns true if string has no white space.</returns>
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

