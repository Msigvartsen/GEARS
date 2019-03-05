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


        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
        string apikey = "AIzaSyAvapQU2GAWFaaHDO5Vh6C5shdg-wqnEu8";
        //string path = "https://www.google.com/maps/embed/v1/view?zoom=9&center=60.3296,11.2723&key=" + apikey;
        string path = "https://maps.googleapis.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=" + apikey;

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(path))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Google API" + webRequest.downloadHandler.text);
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                GetComponent<RawImage>().texture = texture;
            }
        }
    }
}
