using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetProfilePicture : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadProfilePicture);
    }

    public void LoadProfilePicture()
    {
        UserController uc = UserController.GetInstance();
        GameObject profilePicture = GameObject.FindGameObjectWithTag("ProfilePicture");
        string name = GetComponent<RawImage>().name;
        uc._currentUser.media_ID = name;
        string path = ConstantsNS.Constants.FTPPath + "Media/Images/"+ name;
        Uri test = new Uri(path);
        profilePicture.GetComponent<RawImage>().texture = FTPHandler.DownloadImageFromFTP(test);
    }
}
