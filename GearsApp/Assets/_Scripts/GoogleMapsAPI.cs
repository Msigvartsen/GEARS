using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleMapsAPI : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Map());
    }

    IEnumerator Map()
    {
        //https://www.google.com/maps/embed/v1/MODE?key=YOUR_API_KEY&parameters
        string url = "https://www.google.com/maps/embed/v1/";
        string viewType = "view";
        string apikey = "AIzaSyAvapQU2GAWFaaHDO5Vh6C5shdg-wqnEu8";
        string lat = "61.997638";
        string lon = "20.888123";
        string center = lat + ", " + lon;
        string maptype = "satellite";
        string zoom = "12";

        string requestURL = url + viewType +
                "?key=" + apikey +
                "&center=" + center +
                "&zoom=" + zoom +
                "&maptype=" + maptype;


        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
        //string path = "https://www.google.com/maps/embed/v1/view?zoom=9&center=60.3296,11.2723&key=" + apikey;
        string path = "https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=" + apikey;


        WWW req = new WWW(requestURL);
        Debug.Log("String: " + requestURL);
        //Debug.Log("Google API" + webRequest.downloadHandler.text);
        GetComponent<RawImage>().texture = new Texture2D(512, 512, TextureFormat.DXT1, false);
        while (!req.isDone)
            yield return null;
        if (req.error == null)
            req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);

        //using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(requestURL))
        //{
        //    webRequest.downloadHandler = new DownloadHandlerBuffer();
        //    yield return webRequest.SendWebRequest();

        //    if (webRequest.isNetworkError)
        //    {
        //        Debug.Log("Error: " + webRequest.error);
        //    }
        //    else
        //    {
        //        WWW req = new WWW(requestURL);
        //        Debug.Log("String: " + requestURL);
        //        Debug.Log("Google API" + webRequest.downloadHandler.text);
        //        GetComponent<RawImage>().texture = new Texture2D(512, 512, TextureFormat.DXT1, false);
        //        while (!req.isDone)
        //            yield return null;
        //        if (req.error == null)
        //            req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);
        //        //Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
        //        //GetComponent<RawImage>().texture = texture;
        //    }
        //}
    }
}
