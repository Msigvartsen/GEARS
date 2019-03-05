/*
 * Based on the Google Maps for Unity Asset
 * https://www.assetstore.unity3d.com/en/#!/content/3573
 * However the relience on UniWeb has been removed
 * 
 * 
    Getting Started
    ---------------
    1. Assign the GoogleMap component to your game object.

    2. Setup the parameters in the inspector.

    2.1 If you want to control the center point and zoom level, make sure that
 
       the Auto Locate Center box is unchecked. Otherwise the center point is
 
       calculated using Markers and Path parameters.

    3. Each location field can be an address or longitude / latitude.

    4. The markers add pins onto the map, with a single letter label. This label

       will only display on mid size markers.

    5. The paths add straight lines on the map, between a set of locations.

    6. For in depth information on how the GoogleMap component uses the Google
    Maps API, see: 
    https://developers.google.com/maps/documentation/staticmaps/#quick_example
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GoogleMaps : MonoBehaviour
{
    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }

    public string GoogleApiKey;
    public bool loadOnStart = true;
    public bool autoLocateCenter = true;
    public GoogleMapLocation centerLocation;
    public int zoom = 13;
    public MapType mapType;
    public int size = 512;
    public bool doubleResolution = false;
    public GoogleMapMarker[] markers;
    public GoogleMapPath[] paths;


    void Start()
    {
        if (loadOnStart)
            Refresh();
    }

    public void Refresh()
    {
        if (autoLocateCenter && (markers.Length == 0 && paths.Length == 0))
        {
            Debug.LogError("Auto Center will only work if paths or markers are used.");
        }

        StartCoroutine(_Refresh());
    }

    IEnumerator _Refresh()
    {
        string url = "http://maps.googleapis.com/maps/api/staticmap";
        string qs = "";
        if (!autoLocateCenter)
        {
            
            if (centerLocation.address != "")
                qs += "center=" + UnityWebRequest.UnEscapeURL(centerLocation.address);
            else
                qs += "center=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));

            qs += "&zoom=" + zoom.ToString();
        }
        qs += "&size=" + UnityWebRequest.UnEscapeURL(string.Format("{0}x{0}", size));
        qs += "&scale=" + (doubleResolution ? "2" : "1");
        qs += "&maptype=" + mapType.ToString().ToLower();
        var usingSensor = false;

#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif

        qs += "&sensor=" + (usingSensor ? "true" : "false");

        foreach (var i in markers)
        {
            qs += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", i.size.ToString().ToLower(), i.color, i.label);

            foreach (var loc in i.locations)
            {
                if (loc.address != "")
                    qs += "|" + UnityWebRequest.UnEscapeURL(loc.address);
                else
                    qs += "|" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
            }
        }

        foreach (var i in paths)
        {
            qs += "&path=" + string.Format("weight:{0}|color:{1}", i.weight, i.color);

            if (i.fill)
                qs += "|fillcolor:" + i.fillColor;

            foreach (var loc in i.locations)
            {
                if (loc.address != "")
                    qs += "|" + UnityWebRequest.UnEscapeURL(loc.address);
                else
                    qs += "|" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
            }
        }

        qs += "&key=" + UnityWebRequest.UnEscapeURL(GoogleApiKey);

        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url + "?" + qs);
        Debug.Log(url + "?" + qs);

        // Create a texture in DXT1 format
        //GetComponent<Renderer>().material.mainTexture = new Texture2D(size, size, TextureFormat.DXT1, false);
        GetComponent<RawImage>().texture = new Texture2D(size, size, TextureFormat.DXT1, false);

        //renderer.material.mainTexture = new Texture2D(size, size, TextureFormat.DXT1, false);
        var texture = DownloadHandlerTexture.GetContent(req);
        while (!req.isDone)
            yield return null;
        if (req.error == null)
            GetComponent<RawImage>().texture = texture;
                //req.LoadImageIntoTexture((Texture2D)GetComponent<RawImage>().texture);
    }
}

public enum GoogleMapColor
{
    black,
    brown,
    green,
    purple,
    yellow,
    blue,
    gray,
    orange,
    red,
    white
}

[System.Serializable]
public class GoogleMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
    public enum GoogleMapMarkerSize
    {
        Tiny,
        Small,
        Mid
    }
    public GoogleMapMarkerSize size;
    public GoogleMapColor color;
    public string label;
    public GoogleMapLocation[] locations;

}

[System.Serializable]
public class GoogleMapPath
{
    public int weight = 5;
    public GoogleMapColor color;
    public bool fill = false;
    public GoogleMapColor fillColor;
    public GoogleMapLocation[] locations;
}