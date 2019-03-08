using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BingMaps : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Map());
    }

    // https://dev.virtualearth.net/REST/v1/Imagery/Map/imagerySet/centerPoint/zoomLevel?mapSize={mapSize}&pushpin={pushpin}&mapLayer={mapLayer}&format={format}&mapMetadata={mapMetadata}&key={BingMapsAPIKey}

    IEnumerator Map()
    {
        string apikey = "AlX-3jOqVPhYaOe3SWIaYEVvUwAj_adfgEWeZDcrsLSazMMaUvh0eigR5VSt5lkb";
        string test = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/Bellevue%20Washington?mapLayer=TrafficFlow&key="+apikey;
        string url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/";
        string viewType = "view";
        string lat = "60.793911";
        string lon = "11.07599";
        string center = lat + "," + lon;
        string zoom = "15";
        string size = "512";
        string requestURL = url +
                center + "/" +
                zoom + "?" +
                "mapSize=" + size + "," + size +
                "&key=" + apikey;
        Debug.Log("URL " + requestURL);
        using (WWW req = new WWW(requestURL))
        {
            yield return req;
            GetComponent<RawImage>().texture = new Texture2D(512, 512, TextureFormat.DXT1, false);
            Debug.Log(req.text);
            while (!req.isDone)
                yield return null;
            if (req.error == null)
                req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);
        }
        // Create a texture in DXT1 format
        //using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(test))
        //{
        //    webRequest.downloadHandler = new DownloadHandlerBuffer();
        //    yield return webRequest.SendWebRequest();

        //    if (webRequest.isNetworkError)
        //    {
        //        Debug.Log("Error: " + webRequest.error);
        //    }
        //    else
        //    {
        //        Debug.Log("MAP DATA " + webRequest.downloadHandler.text);
        //        //Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
        //        GetComponent<RawImage>().texture = texture;
        //    }
        //}
    }
}
