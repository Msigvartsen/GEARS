using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net;
using System;

public class ModelManager : MonoBehaviour
{
    public List<Model> modelList;
    private static ModelManager _instance;

    public static ModelManager GetInstance()
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
        if (modelList == null)
        {
            modelList = new List<Model>();
        }
        StartCoroutine(Models());
        //StartCoroutine(Request());
    }

    IEnumerator Models()
    {
        string text = string.Empty;

        TextAsset resourceFile = Resources.Load("models") as TextAsset;

        text = resourceFile.text.ToString();

        WebResponse<Model> response = JsonConvert.DeserializeObject<WebResponse<Model>>(text);

        if (response.handler.statusCode == false)
        {
            Debug.Log("ERROR: NO MODELS RETRIEVED FROM DATABASE");
        }
        else
        {
            foreach (Model model in response.objectList)
            {
                modelList.Add(model);
                Debug.Log("Models = " + model.model_name);
            }
        }

        yield return text;
    }

    IEnumerator Request()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/models.php"))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;
            Uri test = new Uri("ftp://ftp.bardrg.com/GEARS/PHPScripts/models.php");

            if (DisplayFileFromServer(test))
            {
                Debug.Log("FOUND SERVER");
            }

            Debug.Log(req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<Model> response = JsonConvert.DeserializeObject<WebResponse<Model>>(req);

                if (response.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO MODELS RETRIEVED FROM DATABASE");
                }
                else
                {
                    foreach (Model model in response.objectList)
                    {
                        modelList.Add(model);
                        Debug.Log("Models = " + model.model_name);
                    }
                }
            }
        }
    }

    public static bool DisplayFileFromServer(Uri serverUri)
    {
        //string serverUri = "ftp://ftp.bardrg.com/GEARS/PHPScripts/models.php";
        
        // The serverUri parameter should start with the ftp:// scheme.
        if (serverUri.Scheme != Uri.UriSchemeFtp)
        {
            return false;
        }
        // Get the object used to communicate with the server.
        WebClient request = new WebClient();

        // This example assumes the FTP site uses anonymous logon.
        request.Credentials = new NetworkCredential("bardrg.com_gearsa", "zg5M2o8S8bDkE9iI");
        try
        {
            byte[] newFileData = request.DownloadData(serverUri.ToString());
            string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
            Debug.Log("File string: " + fileString);
        }
        catch (WebException e)
        {
            Debug.Log(e.ToString());
        }
        return true;
    }
}
