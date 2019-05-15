using UnityEngine;
using GEARSApp;
using TMPro;

/// <summary>
/// Registration script handles registration forms for new users and validates input before running
/// WebRequest to update database.
/// </summary>
public class Registration : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField mobileField;
    [SerializeField]
    private TMP_InputField nameField;
    [SerializeField]
    private TMP_InputField passwordField;

    [SerializeField]
    private PopupNotification popupNotification;

    /// <summary>
    /// Register New User.
    /// Checks if the input is valid. If not: Display an popup message describing what needs to be fixed.
    /// If true: Run the WebRequest to database
    /// </summary>
    public void CallRegister()
    {
        string message = string.Empty;

        if (!ValidateInput(ref message))
            popupNotification.ShowPopup(message);
        else
            RequestRegisterUser();
    }

    /// <summary>
    /// Creates an request form for registering new user and runs PHP script on Webserver.
    /// </summary>
    private void RequestRegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("telephonenr", mobileField.text);
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        string path = Constants.PhpPath + "register.php";

        StartCoroutine(WebRequestController.PostRequest<WebResponse<User>>(path, form, RegisterUser));
    }

    /// <summary>
    /// Action method used when calling WebRequestController. If an error occurs: display a popup.
    /// </summary>
    /// <param name="obj">Input received from within the WebRequestController function (Get/Post)</param>
    private void RegisterUser(WebResponse<User> obj)
    {
        if(!WebRequestController.CheckValidResponse(obj.handler))
        {
            if (popupNotification != null)
                popupNotification.ShowPopup(obj.handler.text);

            return;
        }

        UserController manager = UserController.GetInstance();
        manager.CurrentUser = obj.objectList[0];
        LocationController.GetInstance().CallGetFavorites();
        LoadingScreen.LoadScene(Constants.MainScene);
    }

    /// <summary>
    /// Checks all inputforms and validates the input.
    /// </summary>
    /// <param name="message">Message to display in notification</param>
    /// <returns>Returns true if all tests pass</returns>
    private bool ValidateInput(ref string message)
    {
        if (!Inputcheck.ValidateNumberInput(mobileField))
        {
            message = "Mobile number needs to have 8 digits";
            return false;
        }

        int usernameRequiredLength = 4;
        if (!Inputcheck.ValidateTextInput(nameField,usernameRequiredLength))
        {
            message = "Username needs to be atleast " + usernameRequiredLength + " characters long with one Captial letter. (A-Z) - No numbers or special characters";
            return false;
        }

        int passwordRequiredLength = 6;
        if (!Inputcheck.ValidateTextInput(passwordField,6,true))
        {
            message = "Password needs atleast " + passwordRequiredLength + " characters, One upper case letter and one number";
            return false;
        }

        return true;
    }
}
