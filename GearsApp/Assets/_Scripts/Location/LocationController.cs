﻿using System.Collections;
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

    private Sprite favoriteFilled;
    private Sprite favoriteOutline;

    public Sprite FavoriteFilled { get => favoriteFilled; set => favoriteFilled = value; }
    public Sprite FavoriteOutline { get => favoriteOutline; set => favoriteOutline = value; }

    private void Start()
    {
        favoriteFilled = Resources.Load<Sprite>("_Icons/star_white");
        favoriteOutline = Resources.Load<Sprite>("_Icons/star_outline_white");

        LocationServiceNS.LocationService.CallUserPermission();
        LocationServiceNS.LocationService.StartLocationService();
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
        foreach(Location loc in locationList)
        {
            loc.favorite = false;
        }
    }

    public void UpdateLocation(Location updatedLocation)
    {
        for(int i = 0; i < locationList.Count; i++)
        {
            if(locationList[i].location_ID == updatedLocation.location_ID)
                locationList[i] = updatedLocation;
        }
    }

    public void CallGetFavorites()
    {
        StartCoroutine(GetFavorites());
    }

    IEnumerator Request()
    {
        //using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/locations.php"))
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
                        Uri uri = new Uri(ConstantsNS.Constants.FTPLocationPath + loc.name + "/Images/img.jpg");
                        //Texture2D tex = FTPHandler.DownloadImageFromFTP(uri);
                        //loc.thumbnail = tex;
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

