using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;
using TMPro;

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

    public void CallRegister()
    {
        string message = string.Empty;

        if (!ValidateInput(ref message))
            popupNotification.ShowPopup(message);
        else
            RequestRegisterUser();
    }

    private void RequestRegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("telephonenr", mobileField.text);
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        string path = Constants.PhpPath + "register.php";

        StartCoroutine(WebRequestController.PostRequest<User>(path, form, RegisterUser));
    }

    private void RegisterUser(WebResponse<User> obj)
    {
        if (obj.handler.statusCode == false)
        {
            if (popupNotification != null)
                popupNotification.ShowPopup("User Already Exist!");

            return;
        }

        UserController manager = UserController.GetInstance();
        manager.CurrentUser = obj.objectList[0];
        LocationController.GetInstance().CallGetFavorites();
        LoadingScreen.LoadScene("Main");
    }

    private bool ValidateInput(ref string message)
    {
        if (!Inputcheck.ValidateNumberInput(mobileField))
        {
            message = "Mobile number needs to have 8 digits";
            return false;
        }
        if (!Inputcheck.ValidateTextInput(nameField))
        {
            message = "Username needs to be atleast 4 characters long. (A-Z) - No numbers or special characters";
            return false;
        }
        if (!Inputcheck.ValidateTextInput(passwordField, true))
        {
            message = "Password needs atleast 6 characters, One upper case letter and one number";
            return false;
        }

        return true;
    }
}
