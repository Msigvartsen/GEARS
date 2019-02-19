using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
            int.TryParse(req, out int errorcode);
            if (errorcode == 0)
            {
                Debug.Log(req);
            }
            else
            {
                req = "{\"Items\":" + req + "}";
                Model[] models = JsonHelper.FromJson<Model>(req);
                foreach (Model model in models)
                {
                    modelList.Add(model);
                    print(model.model_name + "ModelManager");
                }
            }
        }
    }
}
