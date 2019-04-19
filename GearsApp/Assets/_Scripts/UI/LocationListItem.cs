﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private Button listButton;
    public Location location;
    [SerializeField]
    private GameObject imagePanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI placeName;
    [SerializeField]
    private TMPro.TextMeshProUGUI lengthToLocation;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //DistanceBewteenUserAndLocation();
    }

    private void Init()
    {
        string length = DistanceBetweenUserAndLocation();

        placeName.text = location.name;
        lengthToLocation.text = length;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go.name == "ThumbnailMask")
            {
                GameObject child = go.transform.GetChild(0).gameObject;
                if (child.name == "Thumbnail")
                {
                    child.GetComponent<RawImage>().texture = location.thumbnail;
                }
            }
        }
    }

    private string DistanceBetweenUserAndLocation()
    {
        float locationLat = (float)gameObject.GetComponent<LocationListItem>().location.latitude;
        float locationLong = (float)gameObject.GetComponent<LocationListItem>().location.longitude;

        string length = string.Empty;
        if (Input.location.isEnabledByUser)
        {
            if (CalculateDistance(locationLat, locationLong) < 5)
            {
                length = "< 5 km";
            }
            else
            {
                length = CalculateDistance(locationLat, locationLong).ToString() + " km";
            }
        }
        else
        {
            length = "Location status not found";
        }

        return length;
    }

    public void UpdateFavorite()
    {
        Debug.Log("Update favorite");
        if (transform.parent.GetComponent<LocationListManager>().displayFavoriteOnly)
        {
            Debug.Log("Hide favorite");

            imagePanel.SetActive(false);
        }
    }

    public void OpenLocationTab()
    {
        Debug.Log("BUTTON PRESSED");
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = location;
        LoadingScreen.LoadScene("LocationNew");
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
