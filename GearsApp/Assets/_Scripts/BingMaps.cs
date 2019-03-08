using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BingMaps : MonoBehaviour
{

    public string apikey;
    [Header("Map Location")]
    public double latitude;
    public double longitude;
    public int zoom;
    public int size;
    bool map = false;

    void Start()
    {
        StartCoroutine(StartLocationService());
        //if(map)
        //    StartCoroutine(Map());
    }
    private void Update()
    {
        if (map)
            StartCoroutine(Map());
    }

    // https://dev.virtualearth.net/REST/v1/Imagery/Map/imagerySet/centerPoint/zoomLevel?mapSize={mapSize}&pushpin={pushpin}&mapLayer={mapLayer}&format={format}&mapMetadata={mapMetadata}&key={BingMapsAPIKey}

    IEnumerator Map()
    {
        map = false;
        apikey = "AlX-3jOqVPhYaOe3SWIaYEVvUwAj_adfgEWeZDcrsLSazMMaUvh0eigR5VSt5lkb";
        //string test = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/Bellevue%20Washington?mapLayer=TrafficFlow&key="+apikey;
        string url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/";
        string center = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," + longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string requestURL = url +
                center + "/" +
                zoom.ToString() + "?" +
                "mapSize=" + "1440" + "," + "2960" +
                "&key=" + apikey;

        Debug.Log("URL " + requestURL);
        using (WWW req = new WWW(requestURL))
        {
            yield return req;
            GetComponent<RawImage>().texture = new Texture2D(size, size, TextureFormat.DXT1, false);
            Debug.Log(req.text);
            while (!req.isDone)
                yield return null;
            if (req.error == null)
                req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);
        }
    }

    IEnumerator StartLocationService()
    {
        // First, check if user has location service enabled
        Debug.Log("Start Location Service");
        //yield return new WaitForSeconds(5);

        if (!Input.location.isEnabledByUser)
        {
            latitude = 60.793911;
            longitude = 11.07599;
            map = true;
            yield break;

        }
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if(Input.location.status == LocationServiceStatus.Running)
        {
            Debug.Log(Input.location.lastData.latitude);
            GetComponentInChildren<Text>().text = Input.location.lastData.latitude.ToString();
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            map = true;

        }
        else
        {
            latitude = 60.793911;
            longitude = 11.07599;
            map = true;
            Debug.Log("Unable to determine device location");
            yield break;
        }
        
        //if (Input.location.status == LocationServiceStatus.Failed)
        //{
        //    Debug.Log("Unable to determine device location");
        //    yield break;
        //}
        //else
        //{
        //    // Access granted and location value could be retrieved
        //    Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        //}

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}
