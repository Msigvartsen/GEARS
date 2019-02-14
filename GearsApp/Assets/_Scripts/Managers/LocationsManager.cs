﻿using System.Collections;
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
            Destroy(this.gameObject);
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
           
            Debug.Log(req);
            if(req == "0")
            {
                Debug.Log(req);
            }
            else
            {
                req = "{\"Items\":" + req + "}";
                Location[] locations = JsonHelper.FromJson<Location>(req);
                foreach (Location loc in locations)
                {
                    Debug.Log(loc.location_ID + ": " + loc.name);
                    locationList.Add(loc);
                }
            }
        }
    }
}