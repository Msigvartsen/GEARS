using GEARSApp;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Static Class for handling all WebRequests and queries between app and database.
/// </summary>
public static class WebRequestController
{
    /// <summary>
    /// Send a post request to the database. 
    /// If Query is successfull, a response in Json format is returned.
    /// Json response is parsed to a Templated object, before running an Action Method.
    /// </summary>
    /// <typeparam name="T">Template object to hold data from query.</typeparam>
    /// <param name="path">Path to php file on server</param>
    /// <param name="form">Form with correct input</param>
    /// <param name="WebResponseHandler">Action Method to run if request is successfull.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Send a post request to the database. 
    /// If Query is successfull, a response in Json format is returned.
    /// Json response is parsed to a Templated object, before running an Action Method.
    /// </summary>
    /// <typeparam name="T">Template object #1 to hold data from query.</typeparam>
    /// <typeparam name="U">Template Object #2</typeparam>
    /// <param name="path">Path to php file on server</param>
    /// <param name="form">Form with correct input</param>
    /// <param name="WebResponseHandler">Action Method to run if request is successfull.</param>
    /// <param name="parameter">Template parameter. Used to send in another argument to Action Method. </param>
    /// <returns></returns>
    public static IEnumerator PostRequest<T,U>(string path, WWWForm form, Action<T,U> WebResponseHandler, U parameter)
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
                WebResponseHandler(obj,parameter);
            }
        }
    }

    /// <summary>
    /// Sends a Get Request to database. 
    /// If Query is successfull, a response in Json format is returned.
    /// Json response is parsed to a Templated object, before running an Action Method.
    /// </summary>
    /// <typeparam name="T">Template object to hold data from query.</typeparam>
    /// <param name="path">Path to php file on server</param>
    /// <param name="WebResponseHandler">Action Method to run if request is successfull.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Helper function to check if the response from database query is successfull.
    /// Usually used in conjunction inside action methods to see if handler.status == true.
    /// </summary>
    /// <param name="handler">Status of handler form query.</param>
    /// <returns></returns>
    public static bool CheckValidResponse(PHPStatusHandler handler)
    {
        if(!handler.statusCode)
        {
            Debug.Log(handler.text);
        }
        return handler.statusCode;
    }
}