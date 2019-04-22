using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ConstantsNS;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class UserController : MonoBehaviour
{
    private static UserController _instance;
    private string _username;
    public User CurrentUser { get; set; }
    [SerializeField]
    private int experienceCapPerLevel = 100;

    //To load correct page when changing scene
    public string PreviousPage { get; set; }
    
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

        PreviousPage = "Main";
        LocationServiceNS.LocationService.CallUserPermission();
    }

    public void RequestUserData(string username)
    {
        _username = username;
    }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }

    public void UpdateUserExperience(int experience)
    {
        CurrentUser.experience += experience;
        if(CurrentUser.experience >= experienceCapPerLevel)
        {
            UpdateUserLevel();
        }
    }

    public void UpdateUserLevel()
    {
        if(CurrentUser.experience >= experienceCapPerLevel)
        {
            CurrentUser.level++;
            CurrentUser.experience -= experienceCapPerLevel; //resets experience bar
        }
    }


    public void LogOut()
    {
        CurrentUser = null;
        LocationController.GetInstance().ResetFavorites();
        LoadingScreen.LoadScene("RegistrationAndLogin");
    }

    public void CallUpdateUserPicture(int mediaID)
    {
        CurrentUser.media_ID = mediaID;
        StartCoroutine(UpdateUserPicture());
    }

    public void CallDeleteUser()
    {
        StartCoroutine(DeleteUser());
    }


    IEnumerator UpdateUserPicture()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        form.AddField("media_ID", CurrentUser.media_ID);
        string path = Constants.PhpPath + "updateuserpicture.php";
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
                    Debug.Log("Successful Update" + req);
                }
            }
        }
    }

    IEnumerator DeleteUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        Debug.Log("NUMBEr: " + CurrentUser.telephonenr);
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
                    LogOut();
                }
            }
        }
    }
}
