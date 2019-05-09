using ConstantsNS;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestController
{

    public static IEnumerator PostRequest<T>(WWWForm form, string path)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                WebResponse<T> obj = JsonConvert.DeserializeObject<WebResponse<T>>(req, Constants.JsonSettings);
                Debug.Log(obj.handler.text);

                if (obj.handler.statusCode == true)
                {

                }
                else
                {

                }
            }
        }
    }

    public static IEnumerator GetRequest<T>(string path, Action<WebResponse<T>> WebResponseHandler)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(path))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                WebResponse<T> obj = JsonConvert.DeserializeObject<WebResponse<T>>(req, Constants.JsonSettings);
                if (obj.handler.statusCode == true)
                {
                    WebResponseHandler(obj);
                }
                else
                {
                    Debug.Log("Query Failed -> Status Code false");
                }
            }
        }
    }
}