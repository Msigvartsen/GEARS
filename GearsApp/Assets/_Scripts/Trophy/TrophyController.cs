using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrophyController : MonoBehaviour
{
    private static TrophyController _instance;
    public List<Trophy> TrophyList { get; set; }
    public static TrophyController GetInstance()
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
        if (TrophyList == null)
            TrophyList = new List<Trophy>();

        DontDestroyOnLoad(gameObject);

        StartCoroutine(RequestTrophies());
    }

    IEnumerator RequestTrophies()
    {
        string path = ConstantsNS.Constants.PhpPath + "trophies.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<Trophy> res = JsonConvert.DeserializeObject<WebResponse<Trophy>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: No Trophies retrieved from Database");
                }
                else
                {
                    foreach (Trophy trophy in res.objectList)
                    {
                        Texture2D tex = Resources.Load<Texture2D>("_Trophies/" + trophy.name);
                        trophy.image = tex;
                        Debug.Log("ADDING TO TROPHY LIST " + trophy.name);
                        TrophyList.Add(trophy);
                    }
                }
            }
        }
    }
}
