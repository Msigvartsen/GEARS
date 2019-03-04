using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using ConstantsNS;

public class LocationsManager : MonoBehaviour
{
    public List<Location> locationList;
    public List<Location> favoriteLocationList;
    private static LocationsManager _instance;
    public static LocationsManager GetInstance()
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
        if(locationList == null)
        {
            locationList = new List<Location>();
        }
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(Locations());
        StartCoroutine(Request());
        StartCoroutine(GetFavorites());
    }



    IEnumerator Request()
    {
        //using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/locations.php"))
        string path = Constants.PhpPath + "locations.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN LOCATION" + req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Location: " + req);
                WebResponse<Location> res = JsonConvert.DeserializeObject<WebResponse<Location>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO LOCATIONS RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + res.handler.text);
                    foreach (Location loc in res.objectList)
                    {
                        locationList.Add(loc);
                        Debug.Log("Locs = " + loc.name);
                    }
                }
            }
        }
    }

    IEnumerator GetFavorites()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserManager.GetInstance()._currentUser.telephonenr);
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
                        foreach(Location location in locationList)
                        {
                            if(location.name == loc.name)
                            {
                                location.favorite = true;
                            }
                        }
                       // favoriteLocationList.Add(loc);
                        Debug.Log("CREATE FAV. LOCATION LIST ITEM)");
                    }
                }
            }
        }
    }
    //IEnumerator Locations()
    //{
    //    string text = string.Empty;

    //    TextAsset resourceFile = Resources.Load("locations") as TextAsset;

    //    text = resourceFile.text.ToString();

    //    WebResponse<Location> response = JsonConvert.DeserializeObject<WebResponse<Location>>(text);

    //    if (response.handler.statusCode == false)
    //    {
    //        Debug.Log("ERROR: NO MODELS RETRIEVED FROM DATABASE");
    //    }
    //    else
    //    {
    //        foreach (Location loc in response.objectList)
    //        {
    //            locationList.Add(loc);
    //            Debug.Log("Locations = " + loc.name);
    //        }
    //    }

    //    yield return text;
    //}

}
