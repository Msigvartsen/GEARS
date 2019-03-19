using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Net;


public class BingMaps : MonoBehaviour
{
    public string apikey;
    [Header("Map Location")]
    public double latitude;
    public double longitude;
    public int zoom;
    public int size;
    public bool map = false;

    void Start()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            GetComponentInChildren<Text>().text = Input.location.lastData.latitude.ToString();
        }
        else
        {
            GetComponentInChildren<Text>().text = "Hello";
        }
        
        StartCoroutine(RequestMap());
    }

    IEnumerator RequestMap()
    {
        string path = ConstantsNS.Constants.PhpPath + "test.html";
        Application.OpenURL(path);
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                GetComponent<RawImage>().texture = new Texture2D(size, size, TextureFormat.DXT1, false);
                GetComponent<RawImage>().texture = texture;
            }
        }
    }

    IEnumerator Map()
    {
        map = false;
        Uri uri = new Uri("ftp://ftp.bardrg.com/GEARS/Keys/apikeybing.txt");
        apikey = FTPHandler.DownloadTextFromFTP(uri);
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;  
        //string test = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/Bellevue%20Washington?mapLayer=TrafficFlow&key="+apikey;
        string url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/";
        string center = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," + longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string requestURL = url +
                center + "/" +
                zoom.ToString() + "?" +
                "mapSize=" + "1440" + "," + "2960" +
                "&key=" + apikey;

        Debug.Log("URL " + requestURL);
        using (WWW req = new WWW(requestURL))
        {
            yield return req;
            GetComponent<RawImage>().texture = new Texture2D(size, size, TextureFormat.DXT1, false);
            Debug.Log(req.text);
            while (!req.isDone)
                yield return null;
            if (req.error == null)
                req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);
        }

        //Uri uri = new Uri("ftp://ftp.bardrg.com/GEARS/Locations/Inland University/Images/hinn.jpg");
        //Texture2D tex = FTPHandler.DownloadImageFromFTP(uri);
        //GetComponent<RawImage>().texture = tex;
        //Debug.Log("New Image");
        //yield return tex;
    }

}
