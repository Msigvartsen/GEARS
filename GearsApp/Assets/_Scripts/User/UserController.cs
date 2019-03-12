﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ConstantsNS;
using Newtonsoft.Json;

public class UserController : MonoBehaviour
{
    public User _currentUser;
    private string _username;
    private static UserController _instance;

    public static UserController GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);

        LocationService.CallUserPermission();
    }

    public void RequestUserData(string username)
    {
        _username = username;
    }

    public void SetCurrentUser(User user)
    {
        _currentUser = user;
    }

    public void LogOut()
    {
        _currentUser = null;
        LoadingScreen.LoadScene("RegistrationAndLogin");
    }

    public void CallDeleteUser()
    {
        StartCoroutine(DeleteUser());
    }

    IEnumerator DeleteUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", _currentUser.telephonenr);
        Debug.Log("NUMBEr: " + _currentUser.telephonenr);
        string path = Constants.PhpPath + "deleteuser.php";
        using (UnityWebRequest request = UnityWebRequest.Post(path, form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req);

                if (handler.statusCode == false)
                {
                    Debug.Log(req);
                }
                else
                {
                    _currentUser = null;
                    LoadingScreen.LoadScene("RegistrationAndLogin");
                }
            }
        }
    }
}