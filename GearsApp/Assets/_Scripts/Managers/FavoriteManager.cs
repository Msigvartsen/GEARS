using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstantsNS;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class FavoriteManager : MonoBehaviour
{
    List<Location> _favoriteLocations;
    FavoriteManager _instance;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(GetFavorites());
    }

    IEnumerator GetFavorites()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", UserManager.GetInstance()._currentUser.username);
        string path = Constants.PhpPath + "favorites.php";
        using (UnityWebRequest request = UnityWebRequest.Post(path, form))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN FAVORITES" + req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                WebResponse<Location> res = JsonConvert.DeserializeObject<WebResponse<Location>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO FAVORITES RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + res.handler.text);
                    foreach (Location loc in res.objectList)
                    {
                        //locationList.Add(loc);
                        //Debug.Log("Locs = " + loc.name);
                        Debug.Log("CREATE FAV. LOCATION LIST ITEM)");
                    }
                }
            }
        }
    }
}
