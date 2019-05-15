using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;

/// <summary>
/// Singleton class. Keeps track of all locations retreived from database, as well as the the current location visited.
/// </summary>
public class LocationController : MonoBehaviour
{
    public List<Location> LocationList { get; set; }
    public Location CurrentLocation { get; set; }
    private static LocationController instance;

    /// <summary>
    /// Returns singleton instance of class.
    /// </summary>
    /// <returns></returns>
    public static LocationController GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Is called when the script instance is being loaded.
    /// Creates singleton object if it does not exist.
    /// Creates new List of locations and runs request to database when app starts.
    /// </summary>
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

    /// <summary>
    /// Start Coroutine with GetRequest to retreive list of locations.
    /// </summary>
    private void CallLocationRequest()
    {
        string path = GEARSApp.Constants.PhpPath + "locations.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Location>>(path, InitLocationList));
    }

    /// <summary>
    /// Checks response from database call. Sets up correct images to location object,
    /// before adding new location to Locationlist.
    /// </summary>
    /// <param name="res">Response from Database call.</param>
    private void InitLocationList(WebResponse<Location> res)
    {
        if (!WebRequestController.CheckValidResponse(res.handler))
            return;

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

    /// <summary>
    /// Start Coroutine with PostRequest to retrieve all locations in favorites connected to the user.
    /// </summary>
    public void CallGetFavorites()
    {
        string path = GEARSApp.Constants.PhpPath + "favorites.php";
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        StartCoroutine(WebRequestController.PostRequest<WebResponse<Location>>(path, form, InitFavorites));
    }

    /// <summary>
    /// Checks response from Database, before it checks each location favorite against all Locations.
    /// If they match, set Location.favorite = true.
    /// </summary>
    /// <param name="res">Response from database call.</param>
    private void InitFavorites(WebResponse<Location> res)
    {
        if (!WebRequestController.CheckValidResponse(res.handler))
            return;

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

    /// <summary>
    /// Get Latitude and Longitude from location sent in.
    /// </summary>
    /// <param name="loc"></param>
    /// <returns>Returns Vector2d with latitude and longitude.</returns>
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

    /// <summary>
    /// Reset all favorite locations.
    /// </summary>
    public void ResetFavorites()
    {
        foreach (Location loc in LocationList)
        {
            loc.favorite = false;
        }
    }

    /// <summary>
    /// Updates changes to location in location list. Used if there is a copy of the location that has been updated.
    /// </summary>
    /// <param name="updatedLocation">Updated location, that needs to update LocationList</param>
    public void UpdateLocation(Location updatedLocation)
    {
        for (int i = 0; i < LocationList.Count; i++)
        {
            if (LocationList[i].location_ID == updatedLocation.location_ID)
                LocationList[i] = updatedLocation;
        }
    }
}
