using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour
{
    public UserModel _currentUser;
    private string _username;
    private static UserManager _instance;

    public static UserManager GetInstance()
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
    }

    public void RequestUserData(string username)
    {
        _username = username;
        Debug.Log("NAME BEFORE REQUEST : " + _username);
        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _username);
        Debug.Log("FORM: " + _username);
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/gears/userdata.php", form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }

            Debug.Log(req);
            if (int.TryParse(req, out int errorcode) && errorcode == 0)
            {
                Debug.Log("Error: Pulling User Data from SQL Failed");
            }
            else
            {
                req = "{\"Items\":" + req + "}";
                //string test = req.Trim(new char[] { '[', ']' });
                
                Debug.Log(req);
                // _currentUser = JsonUtility.FromJson<UserModel>(req);
                //UserModel[] users = JsonHelper.FromJson<UserModel>(req);
                //Debug.Log("LENGTH OF USER ARRA YSHOULD BE 1: " + users.Length);
            }
        }
    }

    public void SetCurrentUser(UserModel user)
    {
        _currentUser = user;
    }
}
