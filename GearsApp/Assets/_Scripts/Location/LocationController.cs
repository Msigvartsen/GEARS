using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using ConstantsNS;

public class LocationController : MonoBehaviour
{
    public List<Location> locationList;
    public List<Location> favoriteLocationList;
    private static LocationController _instance;

    public Location CurrentLocation;

    private Sprite favoriteFilled;
    private Sprite favoriteOutline;

    public Sprite FavoriteFilled { get => favoriteFilled; set => favoriteFilled = value; }
    public Sprite FavoriteOutline { get => favoriteOutline; set => favoriteOutline = value; }

    private void Start()
    {
        favoriteFilled = Resources.Load<Sprite>("_Icons/star_white");
        favoriteOutline = Resources.Load<Sprite>("_Icons/star_outline_white");
    }

    public static LocationController GetInstance()
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

        StartCoroutine(Request());
        //StartCoroutine(GetFavorites());
    }

    public void CallGetFavorites()
    {
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
        form.AddField("number", UserController.GetInstance()._currentUser.telephonenr);
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
                    foreach (Location loc in res.objectList)
                    {
                        foreach(Location location in locationList)
                        {
                            if(location.location_ID == loc.location_ID)
                            {
                                location.favorite = true;
                            }
                            else
                            {
                                location.favorite = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
