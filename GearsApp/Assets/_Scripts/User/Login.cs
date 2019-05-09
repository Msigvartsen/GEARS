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
        StartCoroutine(WebRequestController.PostRequest<User>(path, form, WebRequestHandler));
        //StartCoroutine(UserLogin());
    }

    IEnumerator UserLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("password", passwordField.text);
        
        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
        string path = Constants.PhpPath + "login.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
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
                    StationController.GetInstance().CallUserProgressRequest();
                    TrophyController.GetInstance().CallCollectedTrophies();
                    LoadingScreen.LoadScene("Main");
                    ModelController.GetInstance().CallGetFoundModel();
                }
                else
                {
                    Debug.Log("Error: Login Failed -> Wrong Username or Password");
                    if (popupNotification != null)
                    {
                        popupNotification.ShowPopup(obj.handler.text);
                    }
                }
            }
        }
    }

    private void WebRequestHandler(WebResponse<User> obj)
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


