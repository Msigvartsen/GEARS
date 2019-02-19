using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LocationsManager : MonoBehaviour
{
    public List<Location> locationList;
    private static LocationsManager _instance;
    public static LocationsManager GetInstance()
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


        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/gears/locations.php"))
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
                Response res = JsonConvert.DeserializeObject<Response>(req);

                if (res.handler.statusCode == false)
                {
                    Debug.Log(req + ": ERROR: NO LOCATIONS RETRIEVED FROM DATABASE");
                }
                else
                {
                    Debug.Log("Code:" + res.handler.text);
                    foreach (Location loc in res.locations)
                    {
                        Debug.Log("Locs = " + loc.name);
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class Response
{
    public PHPErrorHandler handler;
    [JsonProperty("location")]
    public List<Location> locations;
}
