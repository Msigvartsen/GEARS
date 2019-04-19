using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProximityHandler : MonoBehaviour
{
    [SerializeField]
    GameObject focusButton;

    [SerializeField]
    int maxRange = 1000;

    bool inRange = false;
    bool showButton = false;
    LocationController locationController;
    Location[] locations;
    Location locationInRange;

    float fadeSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        locationController = LocationController.GetInstance();
        locations = locationController.locationList.ToArray();
        focusButton.GetComponent<CanvasGroup>().alpha = 0;
        focusButton.GetComponent<Button>().onClick.AddListener(FocusLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (locations != null)
        {
            locationInRange = CheckRangeToLocations();
        }

        if (inRange)
        {
            if (showButton)
                FadeInPanel();
            else
                FadeOutPanel();
        }
    }

    double CalculateDistanceInMeters(float lat, float longi)
    {
        double total_dist = 0.0;

        if(Input.location.status == LocationServiceStatus.Running)
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
            total_dist = R * c * 1000;
        }

        return total_dist;
    }

    void FadeInPanel()
    {
        if (focusButton.GetComponent<CanvasGroup>().alpha < 1)
        {
            focusButton.GetComponent<CanvasGroup>().alpha += Time.deltaTime * fadeSpeed;
        }

        focusButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "See stations at " + locationInRange.name;
    }

    void FadeOutPanel()
    {
        if (focusButton.GetComponent<CanvasGroup>().alpha > 0)
        {
            focusButton.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * fadeSpeed;
        }
    }

    Location CheckRangeToLocations()
    {
        Location locInRange = null;

        for (int i = 0; i < locations.Length; i++)
        {
            if (CalculateDistanceInMeters((float)locations[i].latitude, (float)locations[i].longitude) < maxRange)
            {
                locInRange = locations[i];
                inRange = true;
                showButton = true;
            }
        }

        return locInRange;
    }

    void FocusLocation()
    {
        if (locationInRange != null)
        {
            GetComponent<SpawnOnMap>().MoveCameraToLocation(locationInRange);
        }
    }
}
