using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaController : MonoBehaviour
{
    private static MediaController _instance;
    public List<Texture2D> imageList;

    public static MediaController GetInstance()
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
        StartCoroutine(RequestImages());
    }

    IEnumerator RequestImages()
    {
        Debug.Log("Request Images...");
        string path = ConstantsNS.Constants.FTPPath + "/Media/Images/";
        Uri uri = new Uri(path);
        imageList = FTPHandler.DownloadAllImagesFromFTP(uri);
        yield return imageList;
    }
}
