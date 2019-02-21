﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Login : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField usernameField;
    public InputField passwordField;

    [Header("Buttons")]
    public Button loginButton;

    public void CallLogin()
    {
        StartCoroutine(UserLogin());
    }

    IEnumerator UserLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
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
                Debug.Log("REQUEST: " + req);
                //PHPStatusHandler obj = JsonConvert.DeserializeObject<PHPStatusHandler>(req);
                WebResponse<UserModel> obj = JsonConvert.DeserializeObject<WebResponse<UserModel>>(req);
                if(obj.handler.statusCode == true)
                {
                    Debug.Log("Hooray! Welcome" + req);
                    UserManager.GetInstance().RequestUserData(req.ToString());
                    LoadingScreen.LoadSceneByIndex(1);
                }
                else
                {
                    Debug.Log("Error: Login Failed -> Wrong Username or Password");
                }
            }
        }
    }
}


