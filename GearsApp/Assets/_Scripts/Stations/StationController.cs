using System.Collections.Generic;
using UnityEngine;
using ConstantsNS;

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

            string path = Constants.PhpPath + "updateuserprogress.php";
            StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateStationVisited));
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
            return;

        CurrentStation.visited = true;
    }
}
