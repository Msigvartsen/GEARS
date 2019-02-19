using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
            if(req == "0")
            {
                Debug.Log(req + ": ERROR: NO LOCATIONS RETRIEVED FROM DATABASE");
            }
            else
            {

                string phpobject = req.Substring(0, req.IndexOf("|"));
                string locationObjects = req.Substring(req.IndexOf("|") +1 );
                locationObjects = "{\"Items\":" + locationObjects + "}";

                PHPErrorHandler obj = JsonUtility.FromJson<PHPErrorHandler>(phpobject);
                Debug.Log("ERROR PHP HANDLER: " + obj.statusCode);
                Location[] locations = JsonHelper.FromJson<Location>(locationObjects);
                foreach (Location loc in locations)
                {
                    Debug.Log(loc.location_ID + ": " + loc.name);
                    locationList.Add(loc);
                }
            }
        }
    }
}
