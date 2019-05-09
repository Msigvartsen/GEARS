using ConstantsNS;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestController
{

    public static IEnumerator PostRequest<T>(string path, WWWForm form, Action<T> WebResponseHandler)
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
                T obj = JsonConvert.DeserializeObject<T>(req, Constants.JsonSettings);
                WebResponseHandler(obj);
            }
        }
    }

    public static IEnumerator GetRequest<T>(string path, Action<T> WebResponseHandler)
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
                T obj = JsonConvert.DeserializeObject<T>(req, Constants.JsonSettings);
                WebResponseHandler(obj);
            }
        }
    }

    public static bool CheckResponse(PHPStatusHandler handler)
    {
        if(!handler.statusCode)
        {
            Debug.Log(handler.text);
        }
        return handler.statusCode;
    }
}