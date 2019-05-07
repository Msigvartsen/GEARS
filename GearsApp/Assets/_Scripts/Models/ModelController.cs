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
    public List<LocationModel> locationModels;
    public List<int> foundModels;
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

        if (locationModels == null)
        {
            locationModels = new List<LocationModel>();
        }

        StartCoroutine(Request());
        StartCoroutine(GetLocationModels());
    }

    public void CallGetFoundModel()
    {
        StartCoroutine(GetFoundModels());
    }

    public void CallUpdateFoundModel(int model_id)
    {
        StartCoroutine(UpdateFoundModels(model_id));
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

    IEnumerator GetLocationModels()
    {
        string path = Constants.PhpPath + "locationmodels.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN LOCATIONMODEL: " + req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<LocationModel> response = JsonConvert.DeserializeObject<WebResponse<LocationModel>>(req);

                if (response.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO LOCATIONMODELS RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + response.handler.text);
                    foreach (LocationModel locationModel in response.objectList)
                    {
                        locationModels.Add(locationModel);
                    }
                }
            }
        }
    }

    IEnumerator GetFoundModels()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", UserController.GetInstance().CurrentUser.telephonenr);

        string path = Constants.PhpPath + "foundmodels.php";
        using (UnityWebRequest request = UnityWebRequest.Post(path, form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN FOUNDMODEL: " + req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<int> response = JsonConvert.DeserializeObject<WebResponse<int>>(req);

                if (response.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO FOUNDMODELS RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + response.handler.text);
                    foreach (int foundModel in response.objectList)
                    {
                        foundModels.Add(foundModel);
                    }
                }
            }
        }
    }

    IEnumerator UpdateFoundModels(int model_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        form.AddField("model_ID", model_id);

        string path = Constants.PhpPath + "updatefoundmodel.php";
        using (UnityWebRequest request = UnityWebRequest.Post(path, form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req);

                if (handler.statusCode == false)
                {
                    Debug.Log(req);
                }
                else
                {
                    Debug.Log("Successful Update" + req);
                    CallGetFoundModel();
                }
            }
        }
    }
}
