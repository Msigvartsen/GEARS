using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Mapbox.Utils;
using System;

public class LocationController : MonoBehaviour
{
    public List<Location> locationList;
    public List<Location> favoriteLocationList;
    private static LocationController _instance;

    public Location CurrentLocation;

    private void Start()
    {
        LocationServiceNS.LocationService.CallUserPermission();
        StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
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
        if (locationList == null)
        {
            locationList = new List<Location>();
        }
        DontDestroyOnLoad(gameObject);

        StartCoroutine(Request());
    }

    public Vector2d GetLatitudeLongitudeFromLocation(Location loc)
    {
        if (loc != null)
            return new Vector2d(loc.latitude, loc.longitude);
        else
        {
            Debug.Log("Error Retreiving Latitude and Longitude from Location, return 0, 0");
            return new Vector2d(0, 0);
        }

    }

    public void ResetFavorites()
    {
        foreach (Location loc in locationList)
        {
            loc.favorite = false;
        }
    }

    public void UpdateLocation(Location updatedLocation)
    {
        for (int i = 0; i < locationList.Count; i++)
        {
            if (locationList[i].location_ID == updatedLocation.location_ID)
                locationList[i] = updatedLocation;
        }
    }

    public void CallGetFavorites()
    {
        StartCoroutine(GetFavorites());
    }

    IEnumerator Request()
    {
        string path = ConstantsNS.Constants.PhpPath + "locations.php";
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
                Debug.Log("Location: " + req);
                WebResponse<Location> res = JsonConvert.DeserializeObject<WebResponse<Location>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO LOCATIONS RETRIEVED FROM DATABASE");
                }
                else
                {
                    foreach (Location loc in res.objectList)
                    {
                        object[] obj = Resources.LoadAll("_Locations/" + loc.name + "/Images");
                        loc.images = new Texture2D[obj.Length];
                        for (int i = 0; i < obj.Length; i++)
                        {
                            loc.images[i] = (Texture2D)obj[i];
                            if (loc.images[i].name == "thumbnail")
                                loc.thumbnail = loc.images[i];
                        }
                        locationList.Add(loc);
                    }
                }
            }
        }
    }

    IEnumerator GetFavorites()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        string path = ConstantsNS.Constants.PhpPath + "favorites.php";
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
                WebResponse<Location> res = JsonConvert.DeserializeObject<WebResponse<Location>>(req, ConstantsNS.Constants.JsonSettings);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO FAVORITES RETRIEVED FROM DATABASE");
                }
                else
                {
                    foreach (Location loc in res.objectList)
                    {
                        for (int i = 0; i < locationList.Count; i++)
                        {
                            int locationID = locationList[i].location_ID;
                            if (locationID == loc.location_ID)
                            {
                                locationList[i].favorite = true;
                            }
                            //else
                            //{
                            //    locationList[i].favorite = false;
                            //}
                        }
                        //foreach(Location location in locationList)
                        //{
                        //    if(location.location_ID == loc.location_ID)
                        //    {
                        //        location.favorite = true;
                        //        Debug.Log("Location: " + location.name + " = " + location.favorite);
                        //    }
                        //    else
                        //    {
                        //        location.favorite = false;
                        //    }
                    }
                }
            }
        }
    }
}

