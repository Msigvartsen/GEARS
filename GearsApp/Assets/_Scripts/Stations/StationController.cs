using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using ConstantsNS;

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
                        Debug.Log("Station id = " + stat.station_ID + " with location_ID = " + stat.location_ID);
                    }
                }
            }
        }
    }
}
