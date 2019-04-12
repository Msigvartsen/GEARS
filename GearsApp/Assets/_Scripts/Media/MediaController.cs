using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class MediaController : MonoBehaviour
{
    private static MediaController _instance;
    public List<Media> mediaList;

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
        string path = ConstantsNS.Constants.PhpPath + "getimages.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Media: " + req);
                WebResponse<Media> res = JsonConvert.DeserializeObject<WebResponse<Media>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO MEDIA RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + res.handler.text);
                    foreach (Media media in res.objectList)
                    {
                        Debug.Log("Media name = " + media.medianame);
                        Uri uri = new Uri(ConstantsNS.Constants.FTPPath + "Media/" + media.mediatype + "/" + media.medianame);
                        //Texture2D image = FTPHandler.DownloadImageFromFTP(uri);
                        //media.image = image;
                        mediaList.Add(media);
                    }
                }
            }
        }
    }
}
