using System.Collections.Generic;
using UnityEngine;
using GEARSApp;

/// <summary>
/// Singleton class. Keeps track of every station retrieved from the database, as well at which station the user is currently at (if any).
/// </summary>
public class StationController : MonoBehaviour
{
    public List<Station> StationList { get; set; }
    public Station CurrentStation { get; set; }

    private static StationController instance;

    /// <summary>
    /// Returns singleton instance of class.
    /// </summary>
    /// <returns>Returns current instance.</returns>
    public static StationController GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Called when the script instance is loaded.
    /// Creates singleton object and a new List of stations, requested from database when the app is starting.
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

        DontDestroyOnLoad(gameObject);

        CallRequestStations();
    }

    /// <summary>
    /// Starts Coroutine to retrieve list of stations.
    /// </summary>
    public void CallRequestStations()
    {
        string path = Constants.PhpPath + "stations.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Station>>(path, InitStationList));
    }

    /// <summary>
    /// Starts Coroutine to retrieve which stations the user has visited/completed.
    /// </summary>
    public void CallUserProgressRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        string path = Constants.PhpPath + "userprogress.php";

        StartCoroutine(WebRequestController.PostRequest<WebResponse<Station>>(path, form, SetVisitedStations));
    }

    /// <summary>
    /// Starts Coroutine that updates database with user progress.
    /// This occurs when the user has scanned a new station.
    /// </summary>
    public void CallUpdateUserProgress()
    {
        if (CurrentStation != null && !CurrentStation.visited)
        {
            UserController userManager = UserController.GetInstance();

            WWWForm form = new WWWForm();
            form.AddField("number", userManager.CurrentUser.telephonenr);
            form.AddField("station_nr", CurrentStation.station_NR);
            form.AddField("location_id", CurrentStation.location_ID);


            string path = Constants.PhpPath + "updateuserprogress.php";

            CheckForFirstStationVisited();
            CurrentStation.visited = true;
            CheckForAllStationsVisitedAtLocation();
            StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateStationVisited));
        }
    }

    /// <summary>
    /// Checks if the user has scanned any stations.
    /// Used to grant achievement/trophy.
    /// </summary>
    private void CheckForFirstStationVisited()
    {
        TrophyController trophyController = TrophyController.GetInstance();
        foreach (var station in StationList)
        {
            if (station.visited)
                return;
        }
        trophyController.AddCollectedTrophyByName("First Contact");
    }

    /// <summary>
    /// Checks if the user is eligible for different station related trophies.
    /// </summary>
    private void CheckForTrophies()
    {
        CheckForFirstStationVisited();
        CheckForAllStationsVisitedAtLocation();
    }

    /// <summary>
    /// Check if the user has completed a location.
    /// This is done by scanning all the stations at a location.
    /// </summary>
    private void CheckForAllStationsVisitedAtLocation()
    {
        TrophyController trophyController = TrophyController.GetInstance();
        int numberOfStations = 0;
        int numberOfVisitedstations = 0;
        foreach (var station in StationList)
        {
            if (station.location_ID == LocationController.GetInstance().CurrentLocation.location_ID)
            {
                numberOfStations++;
                if (station.visited)
                {
                    numberOfVisitedstations++;
                }
            }
        }
        if (numberOfVisitedstations == numberOfStations)
        {
            trophyController.AddCollectedTrophyByName("Adventurer");
        }
    }

    /// <summary>
    /// Checks response from database call. Adds station to StationList.
    /// </summary>
    /// <param name="response">Response from database call.</param>
    private void InitStationList(WebResponse<Station> response)
    {
        if (!WebRequestController.CheckValidResponse(response.handler))
            return;

        if (StationList == null)
            StationList = new List<Station>();

        foreach (Station stat in response.objectList)
        {
            StationList.Add(stat);
        }
    }

    /// <summary>
    /// Sets the stations that the user has visited to be unlocked in the app.
    /// </summary>
    /// <param name="response"></param>
    private void SetVisitedStations(WebResponse<Station> response)
    {
        if (!WebRequestController.CheckValidResponse(response.handler) || response.objectList == null)
            return;

        foreach (Station stat in response.objectList)
        {
            foreach (Station station in StationList)
            {
                if (station.station_NR == stat.station_NR && station.location_ID == stat.location_ID)
                {
                    station.visited = true;
                }
            }
        }
    }

    /// <summary>
    /// Updates database when the user has found a new station and scanned it.
    /// </summary>
    /// <param name="handler"></param>
    private void UpdateStationVisited(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
        {
            CurrentStation.visited = false;
            Debug.Log("Error updating Visited station");
            return;
        }
    }
}
