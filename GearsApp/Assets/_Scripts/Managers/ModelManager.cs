using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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

        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/models.php"))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;
            
            Debug.Log(req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                int.TryParse(req, out int errorcode);
                if (errorcode == 0)
                {
                    Debug.Log(req);
                }
                else
                {
                    WebResponse<Model> response = JsonConvert.DeserializeObject<WebResponse<Model>>(req);

                    if(response.handler.statusCode == false)
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
    }
}
