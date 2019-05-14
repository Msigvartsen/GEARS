using System.Collections.Generic;
using UnityEngine;
using GEARSApp;

public class StationController : MonoBehaviour
{
    public List<Station> StationList { get; set; }
    public Station CurrentStation { get; set; }

    private static StationController instance;

    public static StationController GetInstance()
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

        DontDestroyOnLoad(gameObject);

        CallRequestStations();
    }

    public void CallRequestStations()
    {
        string path = Constants.PhpPath + "stations.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Station>>(path, InitStationList));
    }

    public void CallUserProgressRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        string path = Constants.PhpPath + "userprogress.php";

        StartCoroutine(WebRequestController.PostRequest<WebResponse<Station>>(path, form, SetVisitedStations));
    }

    public void CallUpdateUserProgress()
    {
        if (CurrentStation != null && !CurrentStation.visited)
        {
            UserController userManager = UserController.GetInstance();

            WWWForm form = new WWWForm();
            form.AddField("number", userManager.CurrentUser.telephonenr);
            form.AddField("station_nr", CurrentStation.station_NR);
            form.AddField("location_id", CurrentStation.location_ID);

            CheckForTrophies();

            string path = Constants.PhpPath + "updateuserprogress.php";
            CurrentStation.visited = true;
            StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateStationVisited));
        }
    }

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

    private void CheckForTrophies()
    {
        CheckForAllStationsVisitedAtLocation();
        CheckForFirstStationVisited();
        CheckForFoundTrollModel();
    }

    private void CheckForFoundTrollModel()
    {
        TrophyController trophyController = TrophyController.GetInstance();
        foreach (var model in ModelController.GetInstance().ModelList)
        {
            if(CurrentStation.model_ID == model.model_ID && model.model_name == "Troll")
            {
                trophyController.AddCollectedTrophyByName("Trolled");
            }
        }
    }

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
