using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using ConstantsNS;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    public List<Station> stationList;
    private static StationController _instance;

    public Station CurrentStation;

    private void Start()
    {

    }

    public static StationController GetInstance()
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
        if (stationList == null)
        {
            stationList = new List<Station>();
        }
        DontDestroyOnLoad(gameObject);

        StartCoroutine(Request());
    }

    public void CallUserProgressRequest()
    {
        StartCoroutine(UserProgressRequest());
    }

    IEnumerator Request()
    {
        //using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/stations.php"))
        string path = Constants.PhpPath + "stations.php";
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            string req = request.downloadHandler.text;

            Debug.Log("REQUESTED IN STATION" + req);
            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Station: " + req);
                WebResponse<Station> res = JsonConvert.DeserializeObject<WebResponse<Station>>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO STATIONS RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + res.handler.text);
                    foreach (Station stat in res.objectList)
                    {
                        stationList.Add(stat);
                        Debug.Log("Station nr = " + stat.station_NR + " with location_ID = " + stat.location_ID);
                    }
                }
            }
        }
    }

    IEnumerator UserProgressRequest()
    {
        {
            WWWForm form = new WWWForm();
            form.AddField("number", UserController.GetInstance()._currentUser.telephonenr);
            string path = Constants.PhpPath + "userprogress.php";
            using (UnityWebRequest request = UnityWebRequest.Post(path, form))
            {
                yield return request.SendWebRequest();
                string req = request.downloadHandler.text;

                Debug.Log("REQUESTED IN STATION_USERPROGRESS" + req);
                if (request.isNetworkError)
                {
                    Debug.Log("Error: " + request.error);
                }
                else
                {
                    WebResponse<Station> res = JsonConvert.DeserializeObject<WebResponse<Station>>(req);

                    if (res.handler.statusCode == false)
                    {
                        Debug.Log(req + ": ERROR: NO USERPROGRESS RETRIEVED FROM DATABASE");
                    }
                    else
                    {
                        foreach (Station stat in res.objectList)
                        {
                            foreach (Station station in stationList)
                            {
                                if (station.station_NR == stat.station_NR && station.location_ID == stat.location_ID)
                                {
                                    station.visited = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void CallUpdateUserProgress()
    {
        StartCoroutine(UpdateUserProgress());
    }

    IEnumerator UpdateUserProgress()
    {
        StationController stationManager = StationController.GetInstance();
        Station station;

        if (stationManager.CurrentStation != null)
        {
            station = StationController.GetInstance().CurrentStation;

            if (!station.visited)
            {
                UserController userManager = UserController.GetInstance();
                string path;

                WWWForm form = new WWWForm();
                form.AddField("number", userManager._currentUser.telephonenr);
                form.AddField("station_nr", station.station_NR);
                form.AddField("location_id", station.location_ID);

                path = Constants.PhpPath + "updateuserprogress.php";

                using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
                {
                    webRequest.downloadHandler = new DownloadHandlerBuffer();
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError)
                    {
                        Debug.Log("Error: " + webRequest.error);
                    }
                    else
                    {
                        string req = webRequest.downloadHandler.text;
                        Debug.Log("USERPROGRESS: + " + req);
                        PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req, Constants.JsonSettings);

                        if (handler.statusCode == true)
                        {
                            Debug.Log(handler.text);
                            //station.visited = true;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("User has visited this station before.");
            }
        }
        else
        {
            Debug.Log("StationController.CurrentStation not set to a valid station");
        }
    }
}
