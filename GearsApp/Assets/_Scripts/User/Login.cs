using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;
using TMPro;

public class Login : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField usernameField;
    [SerializeField]
    private TMP_InputField passwordField;

    [SerializeField]
    private PopupNotification popupNotification;


    public void CallLogin()
    {
        string path = Constants.PhpPath + "login.php";
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("password", passwordField.text);
        StartCoroutine(WebRequestController.PostRequest<WebResponse<User>>(path, form, InitLogin));
    }

    private void InitLogin(WebResponse<User> obj)
    {
        if(obj.handler.statusCode == false)
        {
            if (popupNotification != null)
            {
                popupNotification.ShowPopup(obj.handler.text);
            }
            return;
        }

        UserController manager = UserController.GetInstance();
        manager.CurrentUser = obj.objectList.ToArray()[0];

        LocationController.GetInstance().CallGetFavorites();
        StationController.GetInstance().CallUserProgressRequest();
        TrophyController.GetInstance().CallCollectedTrophies();
        ModelController.GetInstance().CallGetFoundModel();
        LoadingScreen.LoadScene("Main");
    }
}


