using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProximityHandler : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    LocationController locationController;
    Location[] locations;
    Location locationInRange;

    float fadeSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        locationController = LocationController.GetInstance();
        locations = locationController.locationList.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (locations != null)
        {
            locationInRange = CheckRangeToLocations();
        }

        if (locationInRange != null)
        {
            FadeInPanel();
        }
    }

    double CalculateDistanceInMeters(float lat, float longi)
    {
        float userLat = Input.location.lastData.latitude;
        float userLong = Input.location.lastData.longitude;
        int R = 6371;
        var lat_rad_1 = Mathf.Deg2Rad * userLat;
        var lat_rad_2 = Mathf.Deg2Rad * lat;
        var d_lat_rad = Mathf.Deg2Rad * (lat - userLat);
        var d_long_rad = Mathf.Deg2Rad * (longi - userLong);
        var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var total_dist = R * c * 1000;

        return total_dist;
    }

    void FadeInPanel()
    {
        if (panel.GetComponent<CanvasGroup>().alpha < 1)
        {
            panel.GetComponent<CanvasGroup>().alpha += Time.deltaTime * fadeSpeed;
        }

        panel.transform.GetChild(0).GetComponentInChildren<Text>().text = "See stations at " + locationInRange.name + " " 
            + CalculateDistanceInMeters((float)locationInRange.latitude,(float) locationInRange.longitude);
    }

    void FadeOutPanel()
    {
        if (panel.GetComponent<CanvasGroup>().alpha > 0)
        {
            panel.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * fadeSpeed;
        }
    }

    Location CheckRangeToLocations()
    {
        Location locInRange = null;

        for (int i = 0; i < locations.Length; i++)
        {
            if (CalculateDistanceInMeters((float)locations[i].latitude, (float)locations[i].longitude) < 1000)
            {
                locInRange = locations[i];
            }
        }

        return locInRange;
    }
}
