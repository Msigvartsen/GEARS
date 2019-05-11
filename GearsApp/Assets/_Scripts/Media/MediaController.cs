using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Class. Requests, and keeps track of all media from database.
/// </summary>
public class MediaController : MonoBehaviour
{
    public List<Media> MediaList { get; set; }
    private static MediaController instance;

    /// <summary>
    /// Get instance of singleton.
    /// </summary>
    /// <returns>Instance of Singleton.</returns>
    public static MediaController GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Runs when app i starting. Creates singleton object.
    /// Makes call to database.
    /// </summary>
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

    /// <summary>
    /// Start Coroutine with request to database.
    /// </summary>
    public void CallRequestImages()
    {
        string path = ConstantsNS.Constants.PhpPath + "getimages.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Media>>(path, InitMediaList));
    }

    /// <summary>
    /// Action Method - Ran from Coroutine in WebRequestController.
    /// </summary>
    /// <param name="res">Response from Database</param>
    private void InitMediaList(WebResponse<Media> res)
    {
        if (!WebRequestController.CheckResponse(res.handler))
            return;

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
