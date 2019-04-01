using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private Button listButton;
    public Location location;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GetComponentInChildren<Text>().text = location.name;
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenLocationTab);

        float locationLat = (float)gameObject.GetComponent<LocationListItem>().location.latitude;
        float locationLong = (float)gameObject.GetComponent<LocationListItem>().location.longitude;

        if (CalculateDistance(locationLat, locationLong) < 5)
        {
            GetComponentsInChildren<Text>()[1].text = "< 5 km";
        }
        else
        {
            GetComponentsInChildren<Text>()[1].text = CalculateDistance(locationLat, locationLong) + " km";
        }
    }

    private void OpenLocationTab()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = location;
        LoadingScreen.LoadScene("Location");
    }

    private double CalculateDistance(float locationLat, float locationLong)
    {
        float userLat = Input.location.lastData.latitude;
        float userLong = Input.location.lastData.longitude;
        int R = 6371;
        var lat_rad_1 = Mathf.Deg2Rad * userLat;
        var lat_rad_2 = Mathf.Deg2Rad * locationLat;
        var d_lat_rad = Mathf.Deg2Rad * (locationLat - userLat);
        var d_long_rad = Mathf.Deg2Rad * (locationLong - userLong);
        var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var total_dist = R * c;

        // Round to closest 0.5 km
        if (total_dist - (int)total_dist > 0.74f)
            total_dist = (int)total_dist + 1;
        else if (total_dist - (int)total_dist > 0.24f)
            total_dist = (int)total_dist + 0.5f;
        else
            total_dist = (int)total_dist;

        return total_dist;
    }
}
