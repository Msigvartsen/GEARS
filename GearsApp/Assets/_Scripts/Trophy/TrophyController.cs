using ConstantsNS;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrophyController : MonoBehaviour
{
    private static TrophyController _instance;

    public List<CollectedTrophy> CollectedTrophies { get; set; }
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

    public void CallCollectedTrophies()
    {
        StartCoroutine(RequestCollectedTrophies());
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
                        Texture2D tex = Resources.Load<Texture2D>("_Trophies/" + trophy.trophyname);
                        trophy.image = tex;
                        TrophyList.Add(trophy);
                    }
                }
            }
        }
    }

    IEnumerator RequestCollectedTrophies()
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        if (CollectedTrophies == null)
            CollectedTrophies = new List<CollectedTrophy>();

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        string path = ConstantsNS.Constants.PhpPath + "collectedtrophies.php";
        using (UnityWebRequest request = UnityWebRequest.Post(path,form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<CollectedTrophy> res = JsonConvert.DeserializeObject<WebResponse<CollectedTrophy>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: No CollectedTrophies found from database");
                }
                else
                {
                    foreach (CollectedTrophy collectedtrophy in res.objectList)
                    {
                        Debug.Log("Found trophy");
                        CollectedTrophies.Add(collectedtrophy);
                    }
                }
            }
        }
    }
    public void CallAddCollectedTrophy(Trophy collectedTrophy)
    {
        StartCoroutine(AddCollectedTrophy(collectedTrophy));
    }

    IEnumerator AddCollectedTrophy(Trophy collectedTrophy)
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        form.AddField("trophyname", collectedTrophy.trophyname);

        string path = ConstantsNS.Constants.PhpPath + "addcollectedtrophy.php";
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
                PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req, Constants.JsonSettings);

                if (handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: Could not add trophy to collected trophy");
                }
                else
                {
                    GameObject gameObject = GameObject.FindGameObjectWithTag("TrophyListManager");
                    var go = gameObject.GetComponent<TrophyListManager>();
                    
                    CollectedTrophies.Add(new CollectedTrophy { telephonenr = currentUser.telephonenr,
                                                                trophyname = collectedTrophy.trophyname});
                                                                
                    go.UpdateTrophyList(collectedTrophy.trophyname);
                }
            }
        }
    }
}
