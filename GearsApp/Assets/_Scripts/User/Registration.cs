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
        {
            popupNotification.ShowPopup(message);
        }
        else
        {
            StartCoroutine(Register());
        }
    }

    private bool ValidateInput(ref string message)
    {

        if (!Inputcheck.ValidateNumberInput(mobileField))
        {
            message = "Mobile number needs to have 8 digits";
            return false;
        }
        if(!Inputcheck.ValidateTextInput(nameField))
        {
            message = "Username needs to be atleast 4 characters long. (A-Z) - No numbers or special characters";
            return false;
        }
        if (!Inputcheck.ValidateTextInput(passwordField,true))
        {
            message = "Password needs atleast 6 characters, One upper case letter and one number";
            return false;
        }
        
        return true;
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("telephonenr", mobileField.text);
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/register.php", form))
        string path = Constants.PhpPath + "register.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                WebResponse<User> obj = JsonConvert.DeserializeObject<WebResponse<User>>(req, Constants.JsonSettings);
                Debug.Log(obj.handler.text);
                if (obj.handler.statusCode == true)
                {
                    UserController manager = UserController.GetInstance();
                    manager.CurrentUser = obj.objectList.ToArray()[0];

                    LocationController.GetInstance().CallGetFavorites();
                    LoadingScreen.LoadScene("Main");
                }
                else
                {
                    if (popupNotification != null)
                    {
                        popupNotification.ShowPopup("User Already Exist!");
                    }
                }
            }
        }
    }
}
