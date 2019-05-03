using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net;
using System;
using ConstantsNS;

public class ModelController : MonoBehaviour
{
    public List<Model> modelList;
    private static ModelController _instance;

    public static ModelController GetInstance()
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
        //StartCoroutine(Models());
        StartCoroutine(Request());
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
        //using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/models.php"))
        string path = Constants.PhpPath + "models.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN MODEL: " + req);
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
                    Debug.Log("Code:" + response.handler.text);
                    foreach (Model model in response.objectList)
                    {
                        modelList.Add(model);
                    }
                }
            }
        }
    }
}
