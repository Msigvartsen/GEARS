﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListManager : MonoBehaviour
{
    public enum ArrayType { Locations, Favorites };
    public ArrayType arrayType;
    private GameObject parent;
    private GameObject[] itemList;
    private Location[] locationArray;
    private string prefabName;
    private bool isInitiated = false;

    private void Init()
    {
        parent = transform.gameObject;
        locationArray = LocationController.GetInstance().locationList.ToArray();
        prefabName = "LocationListItem";
        isInitiated = true;
    }


    public void AddLocationList()
    {
        if (!isInitiated)
        {
            Init();
            UpdateLocationList();
        }
    }

    private void UpdateLocationList()
    {
        Location[] locations = new Location[0];

        if (arrayType == ArrayType.Favorites)
        {
            List<Location> list = new List<Location>();
            for (int i = 0; i < locationArray.Length; i++)
            {
                if (locationArray[i].favorite == true)
                {
                    Debug.Log("Adding to Favlist: " + locationArray[i].name);
                    list.Add(locationArray[i]);
                }
            }
            locations = list.ToArray();
        }
        else
        {
            locations = locationArray;
        }

        itemList = new GameObject[locations.Length];
        Debug.Log("LENGTH OF LOCATIONS BEFORE CREATING LIST: " + locations.Length);
        for (int i = 0; i < locations.Length; i++)
        {
            Debug.Log(i + " location " + locations[i].name);
            itemList[i] = GetListItem(i, locations);
        }
    }

    private GameObject GetListItem(int index, Location[] locations)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().location = locations[index];
        Debug.Log("Location in new prefab = " + locations[index].name);
        //GameObject favoriteButton = GameObject.FindGameObjectWithTag("FavoriteButton");
        //favoriteButton.GetComponent<Toggle>().isOn = locations[index].favorite;

        return go;
    }
}
