using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Mapbox.Utils;

public class LocationController : MonoBehaviour
{
    public List<Location> LocationList { get; set; }
    public Location CurrentLocation { get; set; }
    private static LocationController instance;

    private void Start()
    {
        LocationServiceNS.LocationService.CallUserPermission();
        StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
    }

    public static LocationController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        if (LocationList == null)
        {
            LocationList = new List<Location>();
        }
        DontDestroyOnLoad(gameObject);

        CallLocationRequest();

    }

    private void CallLocationRequest()
    {
        string path = ConstantsNS.Constants.PhpPath + "locations.php";
        StartCoroutine(WebRequestController.GetRequest<Location>(path, InitLocationList));
    }

    private void InitLocationList(WebResponse<Location> res)
    {
        if (res.handler.statusCode == false)
        {
            Debug.Log(res.handler.text);
            return;
        }

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
            LocationList.Add(loc);
        }
    }

    public void CallGetFavorites()
    {
        string path = ConstantsNS.Constants.PhpPath + "favorites.php";
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        StartCoroutine(WebRequestController.PostRequest<Location>(path, form, InitFavorites));
    }

    private void InitFavorites(WebResponse<Location> res)
    {
        foreach (Location loc in res.objectList)
        {
            for (int i = 0; i < LocationList.Count; i++)
            {
                int locationID = LocationList[i].location_ID;
                if (locationID == loc.location_ID)
                {
                    LocationList[i].favorite = true;
                }
            }
        }
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
        foreach (Location loc in LocationList)
        {
            loc.favorite = false;
        }
    }

    public void UpdateLocation(Location updatedLocation)
    {
        for (int i = 0; i < LocationList.Count; i++)
        {
            if (LocationList[i].location_ID == updatedLocation.location_ID)
                LocationList[i] = updatedLocation;
        }
    }
}

