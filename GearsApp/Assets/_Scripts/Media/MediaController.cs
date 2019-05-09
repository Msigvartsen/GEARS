using System.Collections.Generic;
using UnityEngine;

public class MediaController : MonoBehaviour
{
    public List<Media> MediaList { get; set; }
    private static MediaController instance;

    public static MediaController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        CallRequestImages();
    }

    public void CallRequestImages()
    {
        string path = ConstantsNS.Constants.PhpPath + "getimages.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Media>>(path, InitMediaList));
    }

    private void InitMediaList(WebResponse<Media> res)
    {
        if(res.handler.statusCode == false)
        {
            Debug.Log(res.handler.text);
            return;
        }

        MediaList = new List<Media>();
        foreach (Media media in res.objectList)
        {
            string mediapath = "_Media/" + media.mediatype + "/" + media.medianame;
            Texture2D image = Resources.Load<Texture2D>(mediapath);
            media.image = image;
            MediaList.Add(media);
        }
    }
}
